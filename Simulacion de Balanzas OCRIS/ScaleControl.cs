using System;
using System.Drawing;
using System.Windows.Forms;

namespace Simulacion_de_Balanzas_OCRIS // IMPORTANTE: Cambia por tu namespace
{
    public partial class ScaleControl : UserControl
    {
        public int IdHardware { get; private set; }

        public ScaleControl(int id)
        {
            InitializeComponent(); // Esto cargará el diseño que pegaremos en el siguiente paso
            IdHardware = id;
            this.AllowDrop = true; // Permite arrastrar productos físicos sobre la balanza

            this.DragEnter += ScaleControl_DragEnter;
            this.DragDrop += ScaleControl_DragDrop;
            this.Click += ScaleControl_Click;
        }

        public void SetLedState(bool on, Color color)
        {
            panelLed.BackColor = on ? color : Color.Gray;
        }

        public void UpdateDisplay(string texto, decimal peso)
        {
            lblInfo.Text = $"{texto}\n{peso} kg";
        }

        private void ScaleControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
        }

        // Eventos personalizados para comunicar al Firmware principal
        public event EventHandler<string> ProductDropped;
        public event EventHandler ScaleClicked;

        private void ScaleControl_DragDrop(object sender, DragEventArgs e)
        {
            string sku = (string)e.Data.GetData(DataFormats.Text);
            ProductDropped?.Invoke(this, sku);
        }

        private void ScaleControl_Click(object sender, EventArgs e)
        {
            ScaleClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}