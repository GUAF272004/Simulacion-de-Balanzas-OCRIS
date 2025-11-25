using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // Necesario para .Count()
using System.Windows.Forms;

namespace Simulacion_de_Balanzas_OCRIS
{
    public partial class MainForm : Form
    {
        // Cola para guardar datos si falla el internet
        private Queue<string> _offlineCache = new Queue<string>();

        // Timer para el timeout de autenticación
        private Timer _authTimer;

        // CAMBIO IMPORTANTE: Usamos HashSet para guardar MULTIPLES IDs de balanzas modificadas.
        // Esto permite mover la balanza 1 y la 2, y que al pasar la tarjeta se guarden ambas.
        private HashSet<int> _balanzasPendientes = new HashSet<int>();

        enum FirmwareState { Booting, AssigningIndexes, Idle, EditMode, WaitingAuth }

        FirmwareState _currentState = FirmwareState.Booting;
        List<BalanzaFisica> _balanzas = new List<BalanzaFisica>();

        // Usamos la implementación real que conecta a tu URL
        IServerAPI _server = new RealServer("https://ocris.stellarbanana.com");

        int _configIndexIterator = 0;
        int _selectedScaleId = -1;

        public MainForm()
        {
            InitializeComponent();
            InitializeAuthTimer(); // Inicializamos el timer antes de usarlo
            GenerarTecladoNumerico();
            InitializeSimulation();

            // Aseguramos conexión de eventos
            this.btnScanBarcode.Click += new EventHandler(this.btnScanBarcode_Click);
            this.btnRFID.Click += new EventHandler(this.btnRFID_Click);
        }

        private void InitializeAuthTimer()
        {
            _authTimer = new Timer();
            _authTimer.Interval = 5000; // 5 segundos de espera
            _authTimer.Tick += (s, e) =>
            {
                _authTimer.Stop();
                ProcesarAjusteNoAutenticado();
            };
        }

        private decimal? SolicitarPesoAlUsuario(string nombreProducto)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Entrada de Peso Variable",
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Width = 300, Text = $"¿Qué cantidad de {nombreProducto} (kg) vas a mover?" };
            TextBox inputBox = new TextBox() { Left = 20, Top = 50, Width = 280 };
            Button confirmation = new Button() { Text = "Aceptar", Left = 180, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = "Cancelar", Left = 20, Width = 100, Top = 90, DialogResult = DialogResult.Cancel };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;

            // Bucle para validar que sea número
            while (true)
            {
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    // Intentar convertir texto a decimal (soporta punto o coma según tu PC)
                    if (decimal.TryParse(inputBox.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal pesoIngresado))
                    {
                        if (pesoIngresado <= 0)
                        {
                            MessageBox.Show("Por favor ingrese un valor mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue; // Volver a preguntar
                        }
                        return pesoIngresado; // Retornar el valor válido
                    }
                    else
                    {
                        MessageBox.Show("Valor inválido. Ingrese solo números (ej. 1.5).", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // El bucle while hace que la ventana se vuelva a mostrar o se mantenga
                        continue;
                    }
                }
                else
                {
                    return null; // El usuario canceló la operación
                }
            }
        }

        private void InitializeSimulation()
        {
            // Crear las 8 balanzas físicas visuales
            for (int i = 0; i < 8; i++)
            {
                var balanzaLogica = new BalanzaFisica { IdHardware = i, PesoActual = 0 };
                _balanzas.Add(balanzaLogica);

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

        // --- LÓGICA DE SENSORES Y PESO (Aquí está la lógica de Retiro y Múltiples Balanzas) ---

        private void OnProductDroppedOnScale(object sender, string sku)
        {
            var control = (ScaleControl)sender;
            var balanza = _balanzas[control.IdHardware];

            // 1. Validaciones de Seguridad
            if (!balanza.ProductoAsignado)
            {
                Log($"❌ ERROR: Balanza {control.IdHardware} sin asignar. Configure primero.");
                control.SetLedState(true, Color.Red);
                Timer errorTimer = new Timer { Interval = 1000 };
                errorTimer.Tick += (s, e) => { control.SetLedState(false, Color.Gray); errorTimer.Stop(); };
                errorTimer.Start();
                return;
            }

            if (balanza.SkuProducto != sku)
            {
                Log($"⚠️ ERROR: Producto incorrecto ({sku}) en balanza de {balanza.SkuProducto}.");
                return;
            }

            Producto p = _server.ObtenerProductoPorSku(sku);

            // --- CAMBIO: SOLICITUD DE PESO DINÁMICO ---

            // Pausamos un momento para pedir el dato al usuario
            decimal? pesoIngresado = SolicitarPesoAlUsuario(p.Nombre);

            // Si el usuario canceló el popup o cerró la ventana, abortamos la operación
            if (pesoIngresado == null) return;

            decimal pesoAOperar = pesoIngresado.Value;
            // -------------------------------------------

            // 2. Lógica para SUMAR o RESTAR peso
            if (chkModoRetiro.Checked)
            {
                pesoAOperar = -pesoAOperar; // Invertimos el peso si es retiro
                Log($"Retirando {Math.Abs(pesoAOperar)}kg de {p.Nombre}...");
            }
            else
            {
                Log($"Agregando {pesoAOperar}kg de {p.Nombre}...");
            }

            // 3. Registrar balanza pendiente
            _balanzasPendientes.Add(control.IdHardware);

            // 4. Actualizar estado físico simulado
            balanza.PesoActual += pesoAOperar;

            // Evitar pesos negativos visuales (Calibración a 0)
            if (balanza.PesoActual < 0) balanza.PesoActual = 0;

            // Feedback visual de Alertas
            VerificarAlertas(balanza, p, control);

            control.UpdateDisplay(p.Nombre, balanza.PesoActual);

            _currentState = FirmwareState.WaitingAuth;
            UpdateOled("Cambio Peso", "Pase Tarjeta RFID...");

            // Reiniciar el timer de inactividad
            _authTimer.Stop();
            _authTimer.Start();
        }
        private void VerificarAlertas(BalanzaFisica balanza, Producto p, ScaleControl control)
        {
            // Si el producto tiene umbrales definidos, cambiamos el LED
            if (p.UmbralMin > 0 && balanza.PesoActual < p.UmbralMin)
                control.SetLedState(true, Color.Red); // Alerta Stock Bajo
            else if (p.UmbralMax > 0 && balanza.PesoActual > p.UmbralMax)
                control.SetLedState(true, Color.Magenta); // Alerta Stock Alto
            else
                control.SetLedState(false, Color.Gray); // Todo OK
        }

        // --- AUTENTICACIÓN Y ENVÍO (Manejo de Lotes) ---

        private void btnRFID_Click(object sender, EventArgs e)
        {
            if (_currentState == FirmwareState.WaitingAuth)
            {
                _authTimer.Stop(); // Usuario llegó a tiempo
                string idTarjeta = "USER-A1";
                Log($"RFID Detectado: {idTarjeta}");
                UpdateOled("Autenticado", "Procesando...");

                // Recorremos TODAS las balanzas que se modificaron
                foreach (int idBalanza in _balanzasPendientes)
                {
                    var b = _balanzas[idBalanza];
                    // Enviamos cada una al servidor
                    IntentarEnviarTransaccion(idBalanza, b.PesoActual, idTarjeta);
                }

                // Limpiamos la lista y volvemos a reposo
                _balanzasPendientes.Clear();
                _currentState = FirmwareState.Idle;
                UpdateOled("Autenticado", "OK - Datos Enviados");
            }
        }

        private void ProcesarAjusteNoAutenticado()
        {
            Log("ALERTA: Tiempo agotado. Registrando Ajuste(s) NO Autenticado(s).");
            UpdateOled("Alerta", "Transaccion Anonima");

            // Enviamos todo lo pendiente como anónimo
            foreach (int idBalanza in _balanzasPendientes)
            {
                var b = _balanzas[idBalanza];
                IntentarEnviarTransaccion(idBalanza, b.PesoActual, "ANONIMO");
            }

            _balanzasPendientes.Clear();
            _currentState = FirmwareState.Idle;
        }

        // --- CONECTIVIDAD Y CACHÉ ---

        private void IntentarEnviarTransaccion(int idBalanza, decimal peso, string usuario)
        {
            string skuActual = _balanzas[idBalanza].SkuProducto;

            if (chkSimularFalloRed.Checked)
            {
                Log($"⚠️ ERROR RED: Guardando Balanza {idBalanza} en caché...");
                // Guardamos ID|PESO|USER|SKU
                _offlineCache.Enqueue($"{idBalanza}|{peso}|{usuario}|{skuActual}");
                UpdateOled("Offline", "Dato Guardado Local");
            }
            else
            {
                // 1. Procesar caché pendiente primero
                while (_offlineCache.Count > 0)
                {
                    string datoViejo = _offlineCache.Dequeue();
                    string[] partes = datoViejo.Split('|');

                    int bId = int.Parse(partes[0]);
                    decimal bPeso = decimal.Parse(partes[1]);
                    string bUser = partes[2];
                    string bSku = partes.Length > 3 ? partes[3] : "";

                    Log($"[RECONEXION] Sincronizando Balanza {bId}...");
                    _server.EnviarTransaccion(bId, bPeso, bUser, bSku);
                }

                // 2. Enviar dato actual
                bool exito = _server.EnviarTransaccion(idBalanza, peso, usuario, skuActual);

                if (exito) Log($"[SERVER] Transacción enviada: Balanza {idBalanza}");
                else UpdateOled("Error Server", "Fallo envio");
            }
        }

        // --- CONFIGURACIÓN E INTERFAZ (Pinpad, Escáner, etc.) ---

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
                FinalizarAsignacionIndices();
            }
        }

        private void FinalizarAsignacionIndices()
        {
            ApagarTodosLosLeds();
            int balanzasConfiguradas = _configIndexIterator;
            int balanzasSinProducto = _balanzas.Count(b => !b.ProductoAsignado);

            if (balanzasSinProducto > 0)
            {
                UpdateOled("Configuracion", "Lista. Esperando Prod.");
                Log($"Configuración terminada. {balanzasConfiguradas} balanzas activas.");

                // Ponemos en amarillo las que están listas
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

            _currentState = FirmwareState.Idle;
        }

        private void ApagarTodosLosLeds()
        {
            foreach (ScaleControl c in flpBalanzas.Controls) c.SetLedState(false, Color.Gray);
        }

        // --- PINPAD Y BOTONES ---

        private void GenerarTecladoNumerico()
        {
            for (int i = 1; i <= 9; i++) CrearBotonPinpad(i.ToString());
            CrearBotonPinpad("0");

            // Botón ENTER para finalizar configuración anticipadamente
            Button btnEnter = CrearBotonPinpad("ENTER");
            btnEnter.BackColor = Color.LightGreen;
            btnEnter.Width = 86;
        }

        private Button CrearBotonPinpad(string texto)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Width = 40;
            btn.Height = 40;
            btn.Margin = new Padding(3);

            btn.Click += (s, e) => {
                if (texto == "ENTER") OnEnterPressed();
                else if (int.TryParse(texto, out int num)) SimularPinpadInput(num);
            };

            flowLayoutPanelKeypad.Controls.Add(btn);
            return btn;
        }

        private void OnEnterPressed()
        {
            if (_currentState == FirmwareState.AssigningIndexes)
            {
                Log("Usuario presionó ENTER: Finalizando configuración manualmente.");
                if (_configIndexIterator < flpBalanzas.Controls.Count)
                {
                    var control = (ScaleControl)flpBalanzas.Controls[_configIndexIterator];
                    control.SetLedState(false, Color.Gray);
                }
                FinalizarAsignacionIndices();
            }
        }

        private void SimularPinpadInput(int numeroIngresado)
        {
            if (_currentState == FirmwareState.AssigningIndexes)
            {
                _balanzas[_configIndexIterator].IndiceLogico = numeroIngresado;
                Log($"Balanza HW:{_configIndexIterator} -> Logico:{numeroIngresado}");

                ((ScaleControl)flpBalanzas.Controls[_configIndexIterator]).SetLedState(false, Color.Gray);
                _configIndexIterator++;
                PromptNextScaleAssignment();
            }
        }

        // --- ESCÁNER Y EDICIÓN ---

        private void OnScaleClicked(object sender, EventArgs e)
        {
            _currentState = FirmwareState.EditMode;
            var control = (ScaleControl)sender;
            _selectedScaleId = control.IdHardware;

            UpdateOled("Modo Edicion", "Escanee Codigo Barras");
            control.SetLedState(true, Color.Orange);
        }

        private void btnScanBarcode_Click(object sender, EventArgs e)
        {
            SimularEscaneoBarcode(txtBarcode.Text);
        }

        private void SimularEscaneoBarcode(string skuEscaneado)
        {
            if (_currentState == FirmwareState.EditMode && _selectedScaleId != -1)
            {
                Producto p = _server.ObtenerProductoPorSku(skuEscaneado);

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

        // --- CARGA DE PRODUCTOS UI ---
        private void CargarProductosArrastrables()
        {
            string[] skus = { "111", "222", "333" };
            string[] nombres = { "Maíz", "Vasos", "Refresco" };

            for (int i = 0; i < skus.Length; i++)
            {
                Button btnProd = new Button();
                btnProd.Text = nombres[i];
                btnProd.Tag = skus[i];
                btnProd.Size = new Size(280, 40);
                btnProd.BackColor = Color.WhiteSmoke;

                btnProd.MouseDown += (s, e) => {
                    Button b = s as Button;
                    b.DoDragDrop(b.Tag.ToString(), DragDropEffects.Copy);
                };

                flpProductos.Controls.Add(btnProd);
            }
        }

        private void UpdateOled(string t, string s) => lblOled.Text = $"{t}\n-----------------\n{s}";
        private void Log(string m) => rtbLog.AppendText($"[{DateTime.Now:mm:ss}] {m}\n");
    }
}