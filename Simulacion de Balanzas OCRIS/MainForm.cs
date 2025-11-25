using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq; // Necesario para .Count()
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulacion_de_Balanzas_OCRIS
{
    public partial class MainForm : Form
    {
        // --- CONSTANTES Y VARIABLES GLOBALES ---
        private const string RACK_ID = "RACK-A-01";
        private const string FILE_PRODUCTOS = "productos_custom.txt";

        // Timers del sistema
        private Timer _keepAliveTimer;  // Para enviar latidos (Heartbeat)
        private Timer _authTimer;       // Para el timeout de autenticación

        // Estructuras de datos
        private Queue<string> _offlineCache = new Queue<string>(); // Caché para fallos de red
        private HashSet<int> _balanzasPendientes = new HashSet<int>(); // IDs de balanzas modificadas pendientes de envío
        private List<BalanzaFisica> _balanzas = new List<BalanzaFisica>();

        // Estado del Firmware
        enum FirmwareState { Booting, AssigningIndexes, Idle, EditMode, WaitingAuth }
        FirmwareState _currentState = FirmwareState.Booting;

        // Variables de configuración
        int _configIndexIterator = 0;
        int _selectedScaleId = -1; // Para saber cuál estamos editando (asignando producto)

        // Instancia del Servidor Real
        IServerAPI _server = new RealServer("https://ocris.stellarbanana.com");

        // --- CONSTRUCTOR E INICIALIZACIÓN ---
        public MainForm()
        {
            InitializeComponent();

            // 1. Inicializar sistemas internos
            InitializeAuthTimer();
            InitializeHeartbeat();

            // 2. Preparar Interfaz
            GenerarTecladoNumerico();
            InitializeSimulation(); // Carga balanzas y productos base

            // 3. Agregar botones de gestión
            AgregarControlesGestionProductos(); // Botones Nuevo/Borrar
            AgregarBotonApagarRack();           // Botón Apagar Sistema

            // 4. Conectar eventos manuales
            this.btnScanBarcode.Click += new EventHandler(this.btnScanBarcode_Click);
            this.btnRFID.Click += new EventHandler(this.btnRFID_Click);
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

            CargarProductosArrastrables(); // Carga productos base + custom

            Log("Sistema encendido.");
            UpdateOled("Inicializando...", "Conectando WiFi...");

            // Simular tiempo de arranque del ESP32
            Timer bootTimer = new Timer { Interval = 2000 };
            bootTimer.Tick += (s, e) => {
                bootTimer.Stop();
                StartIdAssignmentProcess();
            };
            bootTimer.Start();
        }

        private void InitializeAuthTimer()
        {
            _authTimer = new Timer();
            _authTimer.Interval = 5000; // 5 segundos para autenticarse
            _authTimer.Tick += (s, e) =>
            {
                _authTimer.Stop();
                ProcesarAjusteNoAutenticado();
            };
        }

        private void InitializeHeartbeat()
        {
            _keepAliveTimer = new Timer();
            _keepAliveTimer.Interval = 15000; // 15 seg (simula 15 min)
            _keepAliveTimer.Tick += (s, e) =>
            {
                // Ejecutar en hilo separado para no bloquear UI
                Task.Run(() =>
                {
                    bool vivo = _server.EnviarHeartbeat(RACK_ID, "ACTIVE");
                    this.Invoke(new Action(() => {
                        if (vivo) Log($"[HEARTBEAT] {RACK_ID} reportando estado: ACTIVE");
                        else Log("[HEARTBEAT] Error de conexión con servidor.");
                    }));
                });
            };
            _keepAliveTimer.Start();

            // Enviar el primer latido inmediatamente
            Task.Run(() => _server.EnviarHeartbeat(RACK_ID, "ACTIVE"));
        }

        // --- LÓGICA DE CONFIGURACIÓN (ARRANQUE) ---

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

        private void OnEnterPressed()
        {
            if (_currentState == FirmwareState.AssigningIndexes)
            {
                Log("Usuario presionó ENTER: Finalizando configuración manualmente.");
                if (_configIndexIterator < flpBalanzas.Controls.Count)
                {
                    ((ScaleControl)flpBalanzas.Controls[_configIndexIterator]).SetLedState(false, Color.Gray);
                }
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

                // Feedback visual: Amarillo = Listo para asignar producto
                for (int i = 0; i < balanzasConfiguradas; i++)
                {
                    ((ScaleControl)flpBalanzas.Controls[i]).SetLedState(true, Color.Yellow);
                }
            }
            else
            {
                UpdateOled("Funcionando", "Correctamente");
            }

            _currentState = FirmwareState.Idle;
        }

        // --- LÓGICA DE ASIGNACIÓN DE PRODUCTOS (EDIT MODE) ---

        private void OnScaleClicked(object sender, EventArgs e)
        {
            _currentState = FirmwareState.EditMode;
            var control = (ScaleControl)sender;
            _selectedScaleId = control.IdHardware; // Guardamos cuál balanza se tocó

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
                    // Asignamos producto a la balanza seleccionada
                    _balanzas[_selectedScaleId].SkuProducto = p.SKU;
                    Log($"Producto asignado: {p.Nombre}");
                    UpdateOled("Asignado", p.Nombre);

                    var control = (ScaleControl)flpBalanzas.Controls[_selectedScaleId];
                    control.UpdateDisplay(p.Nombre, _balanzas[_selectedScaleId].PesoActual);
                    control.SetLedState(false, Color.Gray); // Apagamos LED (Ya está configurada)

                    _currentState = FirmwareState.Idle;
                    _selectedScaleId = -1;
                }
                else
                {
                    UpdateOled("Error", "SKU No encontrado");
                }
            }
        }

        // --- LÓGICA DE OPERACIÓN (PESO Y SENSORES) ---

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

            // 2. Solicitar Peso Dinámico (Input Box)
            decimal? pesoIngresado = SolicitarPesoAlUsuario(p.Nombre);
            if (pesoIngresado == null) return; // Cancelado

            decimal pesoAOperar = pesoIngresado.Value;

            // 3. Calcular Nuevo Peso (Suma o Resta)
            if (chkModoRetiro.Checked)
            {
                pesoAOperar = -pesoAOperar;
                Log($"Retirando {Math.Abs(pesoAOperar)}kg de {p.Nombre}...");
            }
            else
            {
                Log($"Agregando {pesoAOperar}kg de {p.Nombre}...");
            }

            // 4. Actualizar estado físico
            _balanzasPendientes.Add(control.IdHardware); // Registrar cambio pendiente
            balanza.PesoActual += pesoAOperar;
            if (balanza.PesoActual < 0) balanza.PesoActual = 0; // Evitar negativos

            // 5. Feedback Visual (Alertas)
            VerificarAlertas(balanza, p, control);
            control.UpdateDisplay(p.Nombre, balanza.PesoActual);

            // 6. Entrar en espera de autenticación
            _currentState = FirmwareState.WaitingAuth;
            UpdateOled("Cambio Peso", "Pase Tarjeta RFID...");

            // Reiniciar timer de timeout
            _authTimer.Stop();
            _authTimer.Start();
        }

        private void VerificarAlertas(BalanzaFisica balanza, Producto p, ScaleControl control)
        {
            if (p.UmbralMin > 0 && balanza.PesoActual < p.UmbralMin)
                control.SetLedState(true, Color.Red); // Alerta Stock Bajo
            else if (p.UmbralMax > 0 && balanza.PesoActual > p.UmbralMax)
                control.SetLedState(true, Color.Magenta); // Alerta Stock Alto
            else
                control.SetLedState(false, Color.Gray); // Todo OK
        }

        // --- LÓGICA DE AUTENTICACIÓN Y ENVÍO ---

        private void btnRFID_Click(object sender, EventArgs e)
        {
            if (_currentState == FirmwareState.WaitingAuth)
            {
                _authTimer.Stop(); // Usuario llegó a tiempo
                string idTarjeta = "USER-A1";
                Log($"RFID Detectado: {idTarjeta}");
                UpdateOled("Autenticado", "Procesando...");

                // Enviar cambios de TODAS las balanzas pendientes
                foreach (int idBalanza in _balanzasPendientes)
                {
                    var b = _balanzas[idBalanza];
                    IntentarEnviarTransaccion(idBalanza, b.PesoActual, idTarjeta);
                }

                _balanzasPendientes.Clear();
                _currentState = FirmwareState.Idle;
                UpdateOled("Autenticado", "OK - Datos Enviados");
            }
        }

        private void ProcesarAjusteNoAutenticado()
        {
            Log("ALERTA: Tiempo agotado. Registrando Ajuste(s) NO Autenticado(s).");
            UpdateOled("Alerta", "Transaccion Anonima");

            foreach (int idBalanza in _balanzasPendientes)
            {
                var b = _balanzas[idBalanza];
                IntentarEnviarTransaccion(idBalanza, b.PesoActual, "ANONIMO");
            }

            _balanzasPendientes.Clear();
            _currentState = FirmwareState.Idle;
        }

        private void IntentarEnviarTransaccion(int idBalanza, decimal peso, string usuario)
        {
            string skuActual = _balanzas[idBalanza].SkuProducto;

            if (chkSimularFalloRed.Checked)
            {
                Log($"⚠️ ERROR RED: Guardando Balanza {idBalanza} en caché...");
                _offlineCache.Enqueue($"{idBalanza}|{peso}|{usuario}|{skuActual}");
                UpdateOled("Offline", "Dato Guardado Local");
            }
            else
            {
                // 1. Procesar caché pendiente
                while (_offlineCache.Count > 0)
                {
                    string datoViejo = _offlineCache.Dequeue();
                    string[] partes = datoViejo.Split('|');

                    // Recuperar datos del string guardado
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

        // --- GESTIÓN DE PRODUCTOS (UI DINÁMICA) ---

        private void CargarProductosArrastrables()
        {
            // 1. Productos estáticos
            string[] skus = { "111", "222", "333" };
            string[] nombres = { "Maíz", "Vasos", "Refresco" };

            for (int i = 0; i < skus.Length; i++)
                CrearBotonProductoArrastrable(skus[i], nombres[i]);

            // 2. Productos custom desde archivo
            if (File.Exists(FILE_PRODUCTOS))
            {
                string[] lineas = File.ReadAllLines(FILE_PRODUCTOS);
                foreach (string linea in lineas)
                {
                    string[] partes = linea.Split('|');
                    if (partes.Length >= 2)
                    {
                        CrearBotonProductoArrastrable(partes[0], partes[1]);
                        ((RealServer)_server).RegistrarProductoLocal(partes[0], partes[1]);
                    }
                }
            }
        }

        private void AgregarControlesGestionProductos()
        {
            // Botón AGREGAR
            Button btnAdd = new Button();
            btnAdd.Text = "+ NUEVO PRODUCTO";
            btnAdd.BackColor = Color.LightSkyBlue;
            btnAdd.Font = new Font(this.Font, FontStyle.Bold);
            btnAdd.Width = 280; btnAdd.Height = 40;
            btnAdd.Margin = new Padding(3, 3, 3, 5);
            btnAdd.Click += BtnAdd_Click;

            // Botón BORRAR
            Button btnDel = new Button();
            btnDel.Text = "- BORRAR PRODUCTO";
            btnDel.BackColor = Color.LightCoral;
            btnDel.Font = new Font(this.Font, FontStyle.Bold);
            btnDel.Width = 280; btnDel.Height = 40;
            btnDel.Margin = new Padding(3, 3, 3, 20);
            btnDel.Click += BtnDel_Click;

            flpProductos.Controls.Add(btnAdd);
            flpProductos.Controls.Add(btnDel);
            flpProductos.Controls.SetChildIndex(btnAdd, 0);
            flpProductos.Controls.SetChildIndex(btnDel, 1);
        }

        private void AgregarBotonApagarRack()
        {
            Button btnApagar = new Button();
            btnApagar.Text = "⚠️ APAGAR RACK (SISTEMA)";
            btnApagar.BackColor = Color.OrangeRed;
            btnApagar.ForeColor = Color.White;
            btnApagar.Font = new Font(this.Font, FontStyle.Bold);
            btnApagar.Width = 280; btnApagar.Height = 45;
            btnApagar.Margin = new Padding(3, 3, 3, 20);
            btnApagar.Click += BtnApagarRack_Click;

            flpProductos.Controls.Add(btnApagar);
            flpProductos.Controls.SetChildIndex(btnApagar, 2);
        }

        // --- EVENTOS DE BOTONES DE GESTIÓN ---

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var datos = SolicitarDatosProducto();
            if (datos == null) return;

            string nuevoSku = datos.Value.sku;
            string nuevoNombre = datos.Value.nombre;

            try
            {
                using (StreamWriter sw = File.AppendText(FILE_PRODUCTOS))
                {
                    sw.WriteLine($"{nuevoSku}|{nuevoNombre}");
                }
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }

            CrearBotonProductoArrastrable(nuevoSku, nuevoNombre);
            ((RealServer)_server).RegistrarProductoLocal(nuevoSku, nuevoNombre);
            Log($"Producto Agregado: {nuevoNombre} ({nuevoSku})");
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            var productosDisponibles = flpProductos.Controls.OfType<Button>()
                .Where(b => b.Tag != null && !b.Text.Contains("PRODUCTO") && !b.Text.Contains("APAGAR")).ToList();

            if (productosDisponibles.Count == 0) { MessageBox.Show("No hay productos."); return; }

            string skuSel = SolicitarSeleccionProducto(productosDisponibles);
            if (string.IsNullOrEmpty(skuSel)) return;

            var btn = flpProductos.Controls.OfType<Button>().FirstOrDefault(b => b.Tag?.ToString() == skuSel);
            if (btn != null) { flpProductos.Controls.Remove(btn); btn.Dispose(); }

            EliminarProductoDeArchivo(skuSel);
            Log($"Producto eliminado: {skuSel}");
        }

        private void BtnApagarRack_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("¿Desea APAGAR todo el Rack?\nSe enviará la señal de desconexión.",
                "Confirmar Apagado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                Log("Iniciando secuencia de apagado del RACK...");
                if (_keepAliveTimer != null) _keepAliveTimer.Stop();
                if (_authTimer != null) _authTimer.Stop();

                Task.Run(() =>
                {
                    bool exito = _server.EnviarHeartbeat(RACK_ID, "SHUTDOWN");
                    this.Invoke(new Action(() => {
                        if (exito) Log("[SERVER] Rack desconectado correctamente.");
                        else Log("[SERVER] Sin respuesta, apagando localmente.");
                    }));
                });

                foreach (Control c in flpBalanzas.Controls)
                {
                    if (c is ScaleControl scale)
                    {
                        scale.SetLedState(false, Color.Black);
                        scale.UpdateDisplay("SISTEMA\nAPAGADO", 0);
                        scale.BackColor = Color.FromArgb(20, 20, 20);
                        scale.Enabled = false;
                    }
                }

                flpProductos.Enabled = false;
                btnScanBarcode.Enabled = false;
                btnRFID.Enabled = false;
                UpdateOled("SISTEMA", "DESCONECTADO");

                ((Button)sender).Enabled = false;
                ((Button)sender).Text = "SYSTEM OFF";
                ((Button)sender).BackColor = Color.Gray;
            }
        }

        // --- HELPERS VISUALES Y POPUPS ---

        private void CrearBotonProductoArrastrable(string sku, string nombre)
        {
            Button btn = new Button();
            btn.Text = nombre;
            btn.Tag = sku;
            btn.Size = new Size(280, 40);
            btn.BackColor = Color.WhiteSmoke;
            btn.MouseDown += (s, e) => { ((Button)s).DoDragDrop(((Button)s).Tag.ToString(), DragDropEffects.Copy); };
            flpProductos.Controls.Add(btn);
        }

        private void GenerarTecladoNumerico()
        {
            for (int i = 1; i <= 9; i++) CrearBotonPinpad(i.ToString());
            CrearBotonPinpad("0");
            Button btnEnter = CrearBotonPinpad("ENTER");
            btnEnter.BackColor = Color.LightGreen;
            btnEnter.Width = 86;
        }

        private Button CrearBotonPinpad(string texto)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Width = 40; btn.Height = 40; btn.Margin = new Padding(3);
            btn.Click += (s, e) => {
                if (texto == "ENTER") OnEnterPressed();
                else if (int.TryParse(texto, out int num)) SimularPinpadInput(num);
            };
            flowLayoutPanelKeypad.Controls.Add(btn);
            return btn;
        }

        private void ApagarTodosLosLeds()
        {
            foreach (ScaleControl c in flpBalanzas.Controls) c.SetLedState(false, Color.Gray);
        }

        private void UpdateOled(string t, string s) => lblOled.Text = $"{t}\n-----------------\n{s}";
        private void Log(string m) => rtbLog.AppendText($"[{DateTime.Now:mm:ss}] {m}\n");

        private void EliminarProductoDeArchivo(string sku)
        {
            if (!File.Exists(FILE_PRODUCTOS)) return;
            var lineas = File.ReadAllLines(FILE_PRODUCTOS).Where(l => !l.StartsWith(sku + "|")).ToList();
            File.WriteAllLines(FILE_PRODUCTOS, lineas);
        }

        // Popups personalizados
        private (string sku, string nombre)? SolicitarDatosProducto()
        {
            Form p = new Form() { Width = 350, Height = 250, Text = "Nuevo Producto", StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.FixedDialog };
            TextBox tSku = new TextBox() { Left = 20, Top = 45, Width = 280 };
            TextBox tNom = new TextBox() { Left = 20, Top = 105, Width = 280 };
            Button bOk = new Button() { Text = "Guardar", Left = 200, Top = 150, DialogResult = DialogResult.OK };
            p.Controls.AddRange(new Control[] { new Label() { Text = "SKU:", Left = 20, Top = 20 }, tSku, new Label() { Text = "Nombre:", Left = 20, Top = 80 }, tNom, bOk });
            p.AcceptButton = bOk;

            if (p.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(tSku.Text) && !string.IsNullOrWhiteSpace(tNom.Text)) return (tSku.Text, tNom.Text);
            }
            return null;
        }

        private string SolicitarSeleccionProducto(List<Button> productos)
        {
            Form p = new Form() { Width = 350, Height = 180, Text = "Eliminar", StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.FixedDialog };
            ComboBox cmb = new ComboBox() { Left = 20, Top = 50, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var prod in productos) cmb.Items.Add(new { Text = $"{prod.Text} ({prod.Tag})", Value = prod.Tag.ToString() });
            cmb.DisplayMember = "Text"; cmb.ValueMember = "Value"; if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
            Button bOk = new Button() { Text = "Eliminar", Left = 180, Top = 90, Width = 100, DialogResult = DialogResult.OK, BackColor = Color.LightCoral };
            p.Controls.AddRange(new Control[] { new Label() { Text = "Seleccione:", Left = 20, Top = 20 }, cmb, bOk });
            p.AcceptButton = bOk;

            if (p.ShowDialog() == DialogResult.OK && cmb.SelectedItem != null)
            {
                dynamic item = cmb.SelectedItem;
                return item.Value;
            }
            return null;
        }

        private decimal? SolicitarPesoAlUsuario(string nombre)
        {
            Form p = new Form() { Width = 350, Height = 180, Text = "Peso", StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.FixedDialog };
            TextBox txt = new TextBox() { Left = 20, Top = 50, Width = 280 };
            Button bOk = new Button() { Text = "Aceptar", Left = 180, Top = 90, DialogResult = DialogResult.OK };
            p.Controls.AddRange(new Control[] { new Label() { Text = $"¿Cantidad de {nombre} (kg)?", Left = 20, Top = 20, Width = 300 }, txt, bOk });
            p.AcceptButton = bOk;

            while (true)
            {
                if (p.ShowDialog() != DialogResult.OK) return null;
                if (decimal.TryParse(txt.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal val) && val > 0) return val;
                MessageBox.Show("Ingrese un número válido mayor a 0.");
            }
        }
    }
}