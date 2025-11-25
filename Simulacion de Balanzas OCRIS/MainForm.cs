using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Simulacion_de_Balanzas_OCRIS // IMPORTANTE: Cambia por tu namespace
{
    public partial class MainForm : Form
    {
        private Queue<string> _offlineCache = new Queue<string>(); // Simula la memoria del ESP32
        private Timer _authTimer;
        private int _lastModifiedScaleId = -1;
        enum FirmwareState { Booting, AssigningIndexes, Idle, EditMode, WaitingAuth }

        FirmwareState _currentState = FirmwareState.Booting;
        List<BalanzaFisica> _balanzas = new List<BalanzaFisica>();
        IServerAPI _server = new MockServer();

        int _configIndexIterator = 0;
        int _selectedScaleId = -1;

        private void InitializeAuthTimer()
        {
            _authTimer = new Timer();
            _authTimer.Interval = 5000; // 5 segundos de espera según lógica de flujo
            _authTimer.Tick += (s, e) =>
            {
                _authTimer.Stop();
                ProcesarAjusteNoAutenticado();
            };
        }

        private void ProcesarAjusteNoAutenticado()
        {
            Log("ALERTA: Tiempo agotado. Registrando Ajuste NO Autenticado.");
            UpdateOled("Alerta", "Transaccion Anonima");

            // Usamos la variable correcta
            if (_lastModifiedScaleId != -1)
            {
                IntentarEnviarTransaccion(_lastModifiedScaleId, _balanzas[_lastModifiedScaleId].PesoActual, "ANONIMO");
            }

            _currentState = FirmwareState.Idle;
            _lastModifiedScaleId = -1;
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeAuthTimer();
            GenerarTecladoNumerico(); // Crea botones del 0-9
            InitializeSimulation();
            // Conectar eventos de botones manualmente si el Designer no lo hizo
            this.btnScanBarcode.Click += new System.EventHandler(this.btnScanBarcode_Click);
            this.btnRFID.Click += new System.EventHandler(this.btnRFID_Click);
        }

        private void InitializeSimulation()
        {
            // Crear las 8 balanzas físicas visuales
            for (int i = 0; i < 8; i++)
            {
                var balanzaLogica = new BalanzaFisica { IdHardware = i, PesoActual = 0 };
                _balanzas.Add(balanzaLogica);

                // Pasamos el ID al constructor del control
                var control = new ScaleControl(i);
                control.ProductDropped += OnProductDroppedOnScale;
                control.ScaleClicked += OnScaleClicked;
                control.Margin = new Padding(5);
                flpBalanzas.Controls.Add(control);
            }

            CargarProductosArrastrables();

            Log("Sistema encendido.");
            UpdateOled("Inicializando...", "Conectando WiFi...");

            Timer bootTimer = new Timer { Interval = 2000 };
            bootTimer.Tick += (s, e) => {
                bootTimer.Stop();
                StartIdAssignmentProcess();
            };
            bootTimer.Start();
        }

        // --- MÉTODOS AUXILIARES DE UI ---

        private void GenerarTecladoNumerico()
        {
            // Botones 1-9
            for (int i = 1; i <= 9; i++) CrearBotonPinpad(i.ToString());

            // Botón 0
            CrearBotonPinpad("0");

            // --- NUEVO: Botón ENTER ---
            Button btnEnter = CrearBotonPinpad("ENTER");
            btnEnter.BackColor = Color.LightGreen; // Para que destaque
            btnEnter.Width = 86; // Un poco más ancho (ocupa 2 espacios si quieres)
        }

        private Button CrearBotonPinpad(string texto)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Width = 40;
            btn.Height = 40;
            btn.Margin = new System.Windows.Forms.Padding(3);

            btn.Click += (s, e) => {
                // Si es ENTER, llamamos a su función especial
                if (texto == "ENTER")
                {
                    OnEnterPressed();
                }
                // Si es número, seguimos con la lógica normal
                else if (int.TryParse(texto, out int num))
                {
                    SimularPinpadInput(num);
                }
            };

            flowLayoutPanelKeypad.Controls.Add(btn);
            return btn; // Retornamos el botón por si queremos cambiarle color/tamaño
        }

        private void OnEnterPressed()
        {
            // Solo funciona si estamos en la fase de asignación
            if (_currentState == FirmwareState.AssigningIndexes)
            {
                Log("Usuario presionó ENTER: Finalizando configuración manualmente.");

                // Apagar el LED de la balanza que estaba esperando input actualmente
                if (_configIndexIterator < flpBalanzas.Controls.Count)
                {
                    var control = (ScaleControl)flpBalanzas.Controls[_configIndexIterator];
                    control.SetLedState(false, Color.Gray);
                }

                // Forzamos la finalización
                FinalizarAsignacionIndices();
            }
        }
        
        private void CargarProductosArrastrables()
        {
            // Creamos "cajas" visuales que representan el stock físico disponible para poner en balanzas
            string[] skus = { "111", "222", "333" };
            string[] nombres = { "Maíz (1kg)", "Vasos (0.5kg)", "Refresco (1.5kg)" };

            for (int i = 0; i < skus.Length; i++)
            {
                Button btnProd = new Button();
                btnProd.Text = nombres[i];
                btnProd.Tag = skus[i]; // Guardamos el SKU en el Tag
                btnProd.Size = new Size(280, 40);
                btnProd.BackColor = Color.WhiteSmoke;

                // Evento MouseDown inicia el arrastre (Drag)
                btnProd.MouseDown += (s, e) => {
                    Button b = s as Button;
                    b.DoDragDrop(b.Tag.ToString(), DragDropEffects.Copy);
                };

                flpProductos.Controls.Add(btnProd);
            }
        }

        // --- LÓGICA DEL FIRMWARE (Máquina de Estados) ---

        private void StartIdAssignmentProcess()
        {
            _currentState = FirmwareState.AssigningIndexes;
            _configIndexIterator = 0;
            PromptNextScaleAssignment();
        }

        private void PromptNextScaleAssignment()
        {
            if (_configIndexIterator < 8)
            {
                UpdateOled("Asignacion Indices", $"Ingrese indice Balanza #{_configIndexIterator}");
                var control = (ScaleControl)flpBalanzas.Controls[_configIndexIterator];
                control.SetLedState(true, Color.Green);
            }
            else
            {
                // Ya llegamos al límite natural (8), finalizamos.
                FinalizarAsignacionIndices();
            }
        }

        private void FinalizarAsignacionIndices()
        {
            ApagarTodosLosLeds();

            // 1. Contar cuántas configuramos realmente (para el log)
            int balanzasConfiguradas = _configIndexIterator;

            // 2. Validar estado según PDF (Pág 47)
            int balanzasSinProducto = _balanzas.Count(b => !b.ProductoAsignado);

            if (balanzasSinProducto > 0)
            {
                // Mostramos el mensaje de estado, pero NO bloqueamos el sistema.
                // Según el flujo corregido, ahora entramos en modo "Espera de Productos".
                UpdateOled("Configuracion", "Lista. Esperando Prod.");
                Log($"Configuración terminada. {balanzasConfiguradas} balanzas activas.");

                // Feedback visual: Poner todas las balanzas configuradas en Amarillo (Esperando producto)
                for (int i = 0; i < balanzasConfiguradas; i++)
                {
                    var control = (ScaleControl)flpBalanzas.Controls[i];
                    control.SetLedState(true, Color.Yellow);
                }
            }
            else
            {
                UpdateOled("Funcionando", "Correctamente");
            }

            // 3. Liberar el sistema para operación (Escanear productos)
            _currentState = FirmwareState.Idle;
        }

        private void ApagarTodosLosLeds()
        {
            foreach (ScaleControl c in flpBalanzas.Controls) c.SetLedState(false, Color.Gray);
        }

        private void SimularPinpadInput(int numeroIngresado)
        {
            if (_currentState == FirmwareState.AssigningIndexes)
            {
                _balanzas[_configIndexIterator].IndiceLogico = numeroIngresado; // [cite: 1464]
                Log($"Balanza HW:{_configIndexIterator} -> Logico:{numeroIngresado}");

                ((ScaleControl)flpBalanzas.Controls[_configIndexIterator]).SetLedState(false, Color.Gray);
                _configIndexIterator++;
                PromptNextScaleAssignment();
            }
        }

        private void OnScaleClicked(object sender, EventArgs e)
        {
            _currentState = FirmwareState.EditMode;
            var control = (ScaleControl)sender;
            _selectedScaleId = control.IdHardware;

            UpdateOled("Modo Edicion", "Escanee Codigo Barras"); // [cite: 1473]
            control.SetLedState(true, Color.Orange);
        }

        // Conecta este método al evento Click del botón btnScanBarcode en el Designer o aquí
        private void btnScanBarcode_Click(object sender, EventArgs e)
        {
            SimularEscaneoBarcode(txtBarcode.Text);
        }

        private void SimularEscaneoBarcode(string skuEscaneado)
        {
            if (_currentState == FirmwareState.EditMode && _selectedScaleId != -1)
            {
                Producto p = _server.ObtenerProductoPorSku(skuEscaneado); // Consulta a BD [cite: 1476]

                if (p != null)
                {
                    _balanzas[_selectedScaleId].SkuProducto = p.SKU;
                    Log($"Producto asignado: {p.Nombre}");
                    UpdateOled("Asignado", p.Nombre);

                    var control = (ScaleControl)flpBalanzas.Controls[_selectedScaleId];
                    control.UpdateDisplay(p.Nombre, _balanzas[_selectedScaleId].PesoActual);
                    control.SetLedState(false, Color.Gray);

                    _currentState = FirmwareState.Idle;
                    _selectedScaleId = -1;
                }
                else
                {
                    UpdateOled("Error", "SKU No encontrado");
                }
            }
        }

        // --- SIMULACIÓN FÍSICA (Sensores) ---

        private void OnProductDroppedOnScale(object sender, string sku)
        {
            var control = (ScaleControl)sender;
            var balanza = _balanzas[control.IdHardware];

            // Guardamos cuál es la balanza que se está modificando ahora mismo
            _lastModifiedScaleId = control.IdHardware;

            if (balanza.SkuProducto == sku)
            {
                Producto p = _server.ObtenerProductoPorSku(sku);
                balanza.PesoActual += p.PesoUnitario;
                control.UpdateDisplay(p.Nombre, balanza.PesoActual);

                _currentState = FirmwareState.WaitingAuth;
                UpdateOled("Cambio Peso", "Pase Tarjeta RFID...");
            }
            else
            {
                Log("ALERTA: Producto incorrecto colocado en balanza.");
            }

            _currentState = FirmwareState.WaitingAuth;
            UpdateOled("Cambio Detectado", "Pase Tarjeta o Espere...");

            // --- CORRECCIÓN DE TIMER: REINICIAR CONTEO ---
            _authTimer.Stop();  // Detener si ya estaba corriendo
            _authTimer.Start(); // Iniciar desde 0
                                // ---------------------------------------------
        }

        // Conecta este método al botón btnRFID
        private void btnRFID_Click(object sender, EventArgs e)
        {
            if (_currentState == FirmwareState.WaitingAuth)
            {
                _authTimer.Stop(); // Detenemos el timer porque el usuario YA llegó

                string idTarjeta = "USER-A1";
                Log($"RFID Detectado: {idTarjeta}");
                UpdateOled("Autenticado", "OK");

                // Usamos la variable que guardamos, no el 0 hardcodeado
                if (_lastModifiedScaleId != -1)
                {
                    IntentarEnviarTransaccion(_lastModifiedScaleId, _balanzas[_lastModifiedScaleId].PesoActual, idTarjeta);
                }

                _currentState = FirmwareState.Idle;
                _lastModifiedScaleId = -1; // Reseteamos
            }
        }

        private void UpdateOled(string t, string s) => lblOled.Text = $"{t}\n-----------------\n{s}";
        private void Log(string m) => rtbLog.AppendText($"[{DateTime.Now:mm:ss}] {m}\n");

        private void IntentarEnviarTransaccion(int idBalanza, decimal peso, string usuario)
        {
            // 1. Si el Checkbox está marcado, simulamos que NO hay red
            if (chkSimularFalloRed.Checked)
            {
                Log("⚠️ ERROR RED: Guardando en caché local...");
                // Guardamos los datos en la cola como una cadena de texto
                _offlineCache.Enqueue($"{idBalanza}|{peso}|{usuario}");
                UpdateOled("Offline", "Dato Guardado Local");
            }
            else
            {
                // 2. Si HAY red, primero revisamos si hay cosas pendientes en la memoria (Redundancia)
                while (_offlineCache.Count > 0)
                {
                    string datoViejo = _offlineCache.Dequeue();

                    // Separamos el texto guardado para recuperar los valores: "0|1.5|USER"
                    string[] partes = datoViejo.Split('|');
                    int bId = int.Parse(partes[0]);
                    decimal bPeso = decimal.Parse(partes[1]);
                    string bUser = partes[2];

                    Log($"[RECONEXION] Sincronizando dato pendiente: Balanza {bId}");
                    _server.EnviarTransaccion(bId, bPeso, bUser);
                }

                // 3. Finalmente enviamos el dato actual
                bool exito = _server.EnviarTransaccion(idBalanza, peso, usuario);
                if (exito) UpdateOled("Conectado", "Transaccion OK");
            }
        }
    }
}