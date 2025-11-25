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
            this.chkModoRetiro = new System.Windows.Forms.CheckBox();
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
            this.flpBalanzas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.flpBalanzas.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpBalanzas.Location = new System.Drawing.Point(0, 0);
            this.flpBalanzas.Margin = new System.Windows.Forms.Padding(0);
            this.flpBalanzas.Name = "flpBalanzas";
            this.flpBalanzas.Padding = new System.Windows.Forms.Padding(15);
            this.flpBalanzas.Size = new System.Drawing.Size(800, 529);
            this.flpBalanzas.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panelRight.Controls.Add(this.flpProductos);
            this.panelRight.Controls.Add(this.lblProductos);
            this.panelRight.Controls.Add(this.groupBoxPinpad);
            this.panelRight.Controls.Add(this.groupBoxOled);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(800, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(15);
            this.panelRight.Size = new System.Drawing.Size(613, 529);
            this.panelRight.TabIndex = 1;
            // 
            // flpProductos
            // 
            this.flpProductos.AutoScroll = true;
            this.flpProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.flpProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpProductos.Location = new System.Drawing.Point(15, 469);
            this.flpProductos.Name = "flpProductos";
            this.flpProductos.Size = new System.Drawing.Size(583, 45);
            this.flpProductos.TabIndex = 2;
            // 
            // lblProductos
            // 
            this.lblProductos.AutoSize = true;
            this.lblProductos.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductos.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblProductos.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProductos.Location = new System.Drawing.Point(15, 424);
            this.lblProductos.Name = "lblProductos";
            this.lblProductos.Padding = new System.Windows.Forms.Padding(0, 15, 0, 5);
            this.lblProductos.Size = new System.Drawing.Size(185, 45);
            this.lblProductos.TabIndex = 3;
            this.lblProductos.Text = "📦 Inventario Físico:";
            // 
            // groupBoxPinpad
            // 
            this.groupBoxPinpad.Controls.Add(this.chkModoRetiro);
            this.groupBoxPinpad.Controls.Add(this.chkSimularFalloRed);
            this.groupBoxPinpad.Controls.Add(this.flowLayoutPanelKeypad);
            this.groupBoxPinpad.Controls.Add(this.txtBarcode);
            this.groupBoxPinpad.Controls.Add(this.btnScanBarcode);
            this.groupBoxPinpad.Controls.Add(this.btnRFID);
            this.groupBoxPinpad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxPinpad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBoxPinpad.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBoxPinpad.Location = new System.Drawing.Point(15, 139);
            this.groupBoxPinpad.Name = "groupBoxPinpad";
            this.groupBoxPinpad.Size = new System.Drawing.Size(583, 285);
            this.groupBoxPinpad.TabIndex = 1;
            this.groupBoxPinpad.TabStop = false;
            this.groupBoxPinpad.Text = "Controles Físicos";
            // 
            // chkModoRetiro
            // 
            this.chkModoRetiro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkModoRetiro.AutoSize = true;
            this.chkModoRetiro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkModoRetiro.ForeColor = System.Drawing.Color.Orange;
            this.chkModoRetiro.Location = new System.Drawing.Point(380, 90);
            this.chkModoRetiro.Name = "chkModoRetiro";
            this.chkModoRetiro.Size = new System.Drawing.Size(127, 27);
            this.chkModoRetiro.TabIndex = 5;
            this.chkModoRetiro.Text = "Modo Retiro";
            this.chkModoRetiro.UseVisualStyleBackColor = true;
            // 
            // chkSimularFalloRed
            // 
            this.chkSimularFalloRed.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkSimularFalloRed.AutoSize = true;
            this.chkSimularFalloRed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkSimularFalloRed.ForeColor = System.Drawing.Color.IndianRed;
            this.chkSimularFalloRed.Location = new System.Drawing.Point(225, 90);
            this.chkSimularFalloRed.Name = "chkSimularFalloRed";
            this.chkSimularFalloRed.Size = new System.Drawing.Size(126, 27);
            this.chkSimularFalloRed.TabIndex = 4;
            this.chkSimularFalloRed.Text = "Fallo De Red";
            this.chkSimularFalloRed.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelKeypad
            // 
            this.flowLayoutPanelKeypad.Location = new System.Drawing.Point(20, 30);
            this.flowLayoutPanelKeypad.Name = "flowLayoutPanelKeypad";
            this.flowLayoutPanelKeypad.Size = new System.Drawing.Size(331, 220);
            this.flowLayoutPanelKeypad.TabIndex = 0;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.txtBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBarcode.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtBarcode.ForeColor = System.Drawing.Color.White;
            this.txtBarcode.Location = new System.Drawing.Point(220, 135);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(320, 31);
            this.txtBarcode.TabIndex = 1;
            this.txtBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnScanBarcode
            // 
            this.btnScanBarcode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnScanBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnScanBarcode.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnScanBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanBarcode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnScanBarcode.ForeColor = System.Drawing.Color.White;
            this.btnScanBarcode.Location = new System.Drawing.Point(220, 180);
            this.btnScanBarcode.Name = "btnScanBarcode";
            this.btnScanBarcode.Size = new System.Drawing.Size(320, 40);
            this.btnScanBarcode.TabIndex = 2;
            this.btnScanBarcode.Text = "🔫 Escanear Código";
            this.btnScanBarcode.UseVisualStyleBackColor = false;
            // 
            // btnRFID
            // 
            this.btnRFID.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRFID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnRFID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRFID.FlatAppearance.BorderSize = 0;
            this.btnRFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRFID.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRFID.ForeColor = System.Drawing.Color.White;
            this.btnRFID.Location = new System.Drawing.Point(220, 30);
            this.btnRFID.Name = "btnRFID";
            this.btnRFID.Size = new System.Drawing.Size(320, 45);
            this.btnRFID.TabIndex = 3;
            this.btnRFID.Text = "💳 Pasar Tarjeta RFID (Confirmar)";
            this.btnRFID.UseVisualStyleBackColor = false;
            // 
            // groupBoxOled
            // 
            this.groupBoxOled.Controls.Add(this.pnlOledScreen);
            this.groupBoxOled.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxOled.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBoxOled.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBoxOled.Location = new System.Drawing.Point(15, 15);
            this.groupBoxOled.Name = "groupBoxOled";
            this.groupBoxOled.Size = new System.Drawing.Size(583, 124);
            this.groupBoxOled.TabIndex = 0;
            this.groupBoxOled.TabStop = false;
            this.groupBoxOled.Text = "Pantalla OLED";
            // 
            // pnlOledScreen
            // 
            this.pnlOledScreen.BackColor = System.Drawing.Color.Black;
            this.pnlOledScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOledScreen.Controls.Add(this.lblOled);
            this.pnlOledScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOledScreen.Location = new System.Drawing.Point(3, 23);
            this.pnlOledScreen.Name = "pnlOledScreen";
            this.pnlOledScreen.Padding = new System.Windows.Forms.Padding(5);
            this.pnlOledScreen.Size = new System.Drawing.Size(577, 98);
            this.pnlOledScreen.TabIndex = 0;
            // 
            // lblOled
            // 
            this.lblOled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOled.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblOled.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblOled.Location = new System.Drawing.Point(5, 5);
            this.lblOled.Name = "lblOled";
            this.lblOled.Size = new System.Drawing.Size(565, 86);
            this.lblOled.TabIndex = 0;
            this.lblOled.Text = "SISTEMA APAGADO";
            this.lblOled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.rtbLog.ForeColor = System.Drawing.Color.DarkGray;
            this.rtbLog.Location = new System.Drawing.Point(0, 529);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(1413, 126);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(1413, 655);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.flpBalanzas);
            this.Controls.Add(this.rtbLog);
            this.Name = "MainForm";
            this.Text = "OCRIS | OuroCore Realtime Inventory System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.groupBoxPinpad.ResumeLayout(false);
            this.groupBoxPinpad.PerformLayout();
            this.groupBoxOled.ResumeLayout(false);
            this.pnlOledScreen.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // Controles...
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
        private System.Windows.Forms.CheckBox chkModoRetiro;
    }
}