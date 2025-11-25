namespace Simulacion_de_Balanzas_OCRIS
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.flpBalanzas = new System.Windows.Forms.FlowLayoutPanel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.flpProductos = new System.Windows.Forms.FlowLayoutPanel();
            this.lblProductos = new System.Windows.Forms.Label();
            this.groupBoxPinpad = new System.Windows.Forms.GroupBox();
            this.chkSimularFalloRed = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelKeypad = new System.Windows.Forms.FlowLayoutPanel();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.btnScanBarcode = new System.Windows.Forms.Button();
            this.btnRFID = new System.Windows.Forms.Button();
            this.groupBoxOled = new System.Windows.Forms.GroupBox();
            this.pnlOledScreen = new System.Windows.Forms.Panel();
            this.lblOled = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.panelRight.SuspendLayout();
            this.groupBoxPinpad.SuspendLayout();
            this.groupBoxOled.SuspendLayout();
            this.pnlOledScreen.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpBalanzas
            // 
            this.flpBalanzas.AutoScroll = true;
            this.flpBalanzas.BackColor = System.Drawing.Color.LightGray;
            this.flpBalanzas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flpBalanzas.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpBalanzas.Location = new System.Drawing.Point(0, 0);
            this.flpBalanzas.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flpBalanzas.Name = "flpBalanzas";
            this.flpBalanzas.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.flpBalanzas.Size = new System.Drawing.Size(566, 414);
            this.flpBalanzas.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.White;
            this.panelRight.Controls.Add(this.flpProductos);
            this.panelRight.Controls.Add(this.lblProductos);
            this.panelRight.Controls.Add(this.groupBoxPinpad);
            this.panelRight.Controls.Add(this.groupBoxOled);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(566, 0);
            this.panelRight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.panelRight.Size = new System.Drawing.Size(494, 414);
            this.panelRight.TabIndex = 1;
            // 
            // flpProductos
            // 
            this.flpProductos.AutoScroll = true;
            this.flpProductos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpProductos.Location = new System.Drawing.Point(8, 317);
            this.flpProductos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flpProductos.Name = "flpProductos";
            this.flpProductos.Size = new System.Drawing.Size(478, 88);
            this.flpProductos.TabIndex = 2;
            // 
            // lblProductos
            // 
            this.lblProductos.AutoSize = true;
            this.lblProductos.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProductos.Location = new System.Drawing.Point(8, 289);
            this.lblProductos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProductos.Name = "lblProductos";
            this.lblProductos.Padding = new System.Windows.Forms.Padding(0, 9, 0, 4);
            this.lblProductos.Size = new System.Drawing.Size(119, 28);
            this.lblProductos.TabIndex = 3;
            this.lblProductos.Text = "Arrastrar Productos:";
            // 
            // groupBoxPinpad
            // 
            this.groupBoxPinpad.Controls.Add(this.chkSimularFalloRed);
            this.groupBoxPinpad.Controls.Add(this.flowLayoutPanelKeypad);
            this.groupBoxPinpad.Controls.Add(this.txtBarcode);
            this.groupBoxPinpad.Controls.Add(this.btnScanBarcode);
            this.groupBoxPinpad.Controls.Add(this.btnRFID);
            this.groupBoxPinpad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxPinpad.Location = new System.Drawing.Point(8, 96);
            this.groupBoxPinpad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxPinpad.Name = "groupBoxPinpad";
            this.groupBoxPinpad.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxPinpad.Size = new System.Drawing.Size(478, 193);
            this.groupBoxPinpad.TabIndex = 1;
            this.groupBoxPinpad.TabStop = false;
            this.groupBoxPinpad.Text = "Controles Físicos";
            // 
            // chkSimularFalloRed
            // 
            this.chkSimularFalloRed.AutoSize = true;
            this.chkSimularFalloRed.Location = new System.Drawing.Point(390, 62);
            this.chkSimularFalloRed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkSimularFalloRed.Name = "chkSimularFalloRed";
            this.chkSimularFalloRed.Size = new System.Drawing.Size(88, 17);
            this.chkSimularFalloRed.TabIndex = 4;
            this.chkSimularFalloRed.Text = "Fallo De Red";
            this.chkSimularFalloRed.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelKeypad
            // 
            this.flowLayoutPanelKeypad.Location = new System.Drawing.Point(8, 19);
            this.flowLayoutPanelKeypad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanelKeypad.Name = "flowLayoutPanelKeypad";
            this.flowLayoutPanelKeypad.Size = new System.Drawing.Size(260, 167);
            this.flowLayoutPanelKeypad.TabIndex = 0;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(272, 69);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(112, 20);
            this.txtBarcode.TabIndex = 1;
            // 
            // btnScanBarcode
            // 
            this.btnScanBarcode.Location = new System.Drawing.Point(272, 95);
            this.btnScanBarcode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnScanBarcode.Name = "btnScanBarcode";
            this.btnScanBarcode.Size = new System.Drawing.Size(112, 26);
            this.btnScanBarcode.TabIndex = 2;
            this.btnScanBarcode.Text = "🔫 Escanear";
            this.btnScanBarcode.UseVisualStyleBackColor = true;
            // 
            // btnRFID
            // 
            this.btnRFID.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnRFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRFID.Location = new System.Drawing.Point(272, 19);
            this.btnRFID.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRFID.Name = "btnRFID";
            this.btnRFID.Size = new System.Drawing.Size(112, 35);
            this.btnRFID.TabIndex = 3;
            this.btnRFID.Text = "💳 Pasar Tarjeta RFID";
            this.btnRFID.UseVisualStyleBackColor = false;
            // 
            // groupBoxOled
            // 
            this.groupBoxOled.Controls.Add(this.pnlOledScreen);
            this.groupBoxOled.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxOled.Location = new System.Drawing.Point(8, 9);
            this.groupBoxOled.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxOled.Name = "groupBoxOled";
            this.groupBoxOled.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxOled.Size = new System.Drawing.Size(478, 87);
            this.groupBoxOled.TabIndex = 0;
            this.groupBoxOled.TabStop = false;
            this.groupBoxOled.Text = "Pantalla OLED (Simulación)";
            // 
            // pnlOledScreen
            // 
            this.pnlOledScreen.BackColor = System.Drawing.Color.Black;
            this.pnlOledScreen.Controls.Add(this.lblOled);
            this.pnlOledScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOledScreen.Location = new System.Drawing.Point(2, 15);
            this.pnlOledScreen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlOledScreen.Name = "pnlOledScreen";
            this.pnlOledScreen.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlOledScreen.Size = new System.Drawing.Size(474, 70);
            this.pnlOledScreen.TabIndex = 0;
            // 
            // lblOled
            // 
            this.lblOled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOled.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblOled.ForeColor = System.Drawing.Color.Lime;
            this.lblOled.Location = new System.Drawing.Point(4, 4);
            this.lblOled.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOled.Name = "lblOled";
            this.lblOled.Size = new System.Drawing.Size(466, 62);
            this.lblOled.TabIndex = 0;
            this.lblOled.Text = "SISTEMA APAGADO";
            this.lblOled.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.rtbLog.ForeColor = System.Drawing.Color.Silver;
            this.rtbLog.Location = new System.Drawing.Point(0, 414);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(1060, 118);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 532);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.flpBalanzas);
            this.Controls.Add(this.rtbLog);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "Simulador OCRIS - OuroCore Realtime Inventory System";
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.groupBoxPinpad.ResumeLayout(false);
            this.groupBoxPinpad.PerformLayout();
            this.groupBoxOled.ResumeLayout(false);
            this.pnlOledScreen.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpBalanzas;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.GroupBox groupBoxOled;
        private System.Windows.Forms.Panel pnlOledScreen;
        private System.Windows.Forms.Label lblOled;
        private System.Windows.Forms.GroupBox groupBoxPinpad;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelKeypad;
        private System.Windows.Forms.Button btnRFID;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Button btnScanBarcode;
        private System.Windows.Forms.FlowLayoutPanel flpProductos;
        private System.Windows.Forms.Label lblProductos;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.CheckBox chkSimularFalloRed;
    }
}

