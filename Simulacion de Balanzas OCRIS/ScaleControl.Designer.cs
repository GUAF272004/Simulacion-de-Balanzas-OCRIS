namespace Simulacion_de_Balanzas_OCRIS
{
    partial class ScaleControl
    {
        private System.ComponentModel.IContainer components = null;

        // Limpieza de recursos
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.panelLed = new System.Windows.Forms.Panel();
            this.pictureBoxItem = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxItem)).BeginInit();
            this.SuspendLayout();

            // 
            // panelLed (El indicador LED)
            // 
            this.panelLed.BackColor = System.Drawing.Color.Gray; // Apagado por defecto
            this.panelLed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelLed.Location = new System.Drawing.Point(125, 10);
            this.panelLed.Name = "panelLed";
            this.panelLed.Size = new System.Drawing.Size(15, 15);
            this.panelLed.TabIndex = 0;

            // 
            // pictureBoxItem (Imagen del producto)
            // 
            this.pictureBoxItem.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBoxItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxItem.Location = new System.Drawing.Point(35, 35);
            this.pictureBoxItem.Name = "pictureBoxItem";
            this.pictureBoxItem.Size = new System.Drawing.Size(80, 80);
            this.pictureBoxItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxItem.TabIndex = 1;
            this.pictureBoxItem.TabStop = false;

            // 
            // lblInfo (Texto de peso y SKU)
            // 
            this.lblInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblInfo.ForeColor = System.Drawing.Color.White;
            this.lblInfo.Location = new System.Drawing.Point(5, 120);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(140, 40);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "SIN ASIGNAR\r\n0.00 kg";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // ScaleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50))))); // Color oscuro tipo hardware
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pictureBoxItem);
            this.Controls.Add(this.panelLed);
            this.Name = "ScaleControl";
            this.Size = new System.Drawing.Size(150, 170);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxItem)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panelLed;
        private System.Windows.Forms.PictureBox pictureBoxItem;
    }
}