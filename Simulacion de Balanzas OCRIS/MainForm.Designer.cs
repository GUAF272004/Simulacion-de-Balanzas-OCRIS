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
            this.tlpCentering = new System.Windows.Forms.TableLayoutPanel();
            this.pnlControlsContainer = new System.Windows.Forms.Panel();
            this.btnSystemPower = new System.Windows.Forms.Button();
            this.lblUserSelect = new System.Windows.Forms.Label();
            this.cmbUsuarios = new System.Windows.Forms.ComboBox();
            this.chkModoRetiro = new System.Windows.Forms.CheckBox();
            this.chkSimularFalloRed = new System.Windows.Forms.CheckBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.btnScanBarcode = new System.Windows.Forms.Button();
            this.btnRFID = new System.Windows.Forms.Button();
            this.flowLayoutPanelKeypad = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxOled = new System.Windows.Forms.GroupBox();
            this.pnlOledScreen = new System.Windows.Forms.Panel();
            this.lblOled = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.panelRight.SuspendLayout();
            this.groupBoxPinpad.SuspendLayout();
            this.tlpCentering.SuspendLayout();
            this.pnlControlsContainer.SuspendLayout();
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
            this.panelRight.Size = new System.Drawing.Size(1069, 529);
            this.panelRight.TabIndex = 1;
            // 
            // flpProductos
            // 
            this.flpProductos.AutoScroll = true;
            this.flpProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.flpProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpProductos.Location = new System.Drawing.Point(15, 507);
            this.flpProductos.Name = "flpProductos";
            this.flpProductos.Size = new System.Drawing.Size(1039, 7);
            this.flpProductos.TabIndex = 2;
            // 
            // lblProductos
            // 
            this.lblProductos.AutoSize = true;
            this.lblProductos.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductos.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblProductos.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProductos.Location = new System.Drawing.Point(15, 462);
            this.lblProductos.Name = "lblProductos";
            this.lblProductos.Padding = new System.Windows.Forms.Padding(0, 15, 0, 5);
            this.lblProductos.Size = new System.Drawing.Size(185, 45);
            this.lblProductos.TabIndex = 3;
            this.lblProductos.Text = "📦 Inventario Físico:";
            // 
            // groupBoxPinpad
            // 
            this.groupBoxPinpad.Controls.Add(this.tlpCentering);
            this.groupBoxPinpad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxPinpad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBoxPinpad.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBoxPinpad.Location = new System.Drawing.Point(15, 132);
            this.groupBoxPinpad.Name = "groupBoxPinpad";
            this.groupBoxPinpad.Size = new System.Drawing.Size(1039, 330);
            this.groupBoxPinpad.TabIndex = 1;
            this.groupBoxPinpad.TabStop = false;
            this.groupBoxPinpad.Text = "Controles Físicos";
            // 
            // tlpCentering
            // 
            this.tlpCentering.ColumnCount = 3;
            this.tlpCentering.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.33333F));
            this.tlpCentering.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 540F));
            this.tlpCentering.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tlpCentering.Controls.Add(this.pnlControlsContainer, 1, 0);
            this.tlpCentering.Controls.Add(this.flowLayoutPanelKeypad, 0, 0);
            this.tlpCentering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCentering.Location = new System.Drawing.Point(3, 23);
            this.tlpCentering.Name = "tlpCentering";
            this.tlpCentering.RowCount = 1;
            this.tlpCentering.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCentering.Size = new System.Drawing.Size(1033, 304);
            this.tlpCentering.TabIndex = 0;
            // 
            // pnlControlsContainer
            // 
            this.pnlControlsContainer.Controls.Add(this.btnSystemPower);
            this.pnlControlsContainer.Controls.Add(this.lblUserSelect);
            this.pnlControlsContainer.Controls.Add(this.cmbUsuarios);
            this.pnlControlsContainer.Controls.Add(this.chkModoRetiro);
            this.pnlControlsContainer.Controls.Add(this.chkSimularFalloRed);
            this.pnlControlsContainer.Controls.Add(this.txtBarcode);
            this.pnlControlsContainer.Controls.Add(this.btnScanBarcode);
            this.pnlControlsContainer.Controls.Add(this.btnRFID);
            this.pnlControlsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlsContainer.Location = new System.Drawing.Point(287, 0);
            this.pnlControlsContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControlsContainer.Name = "pnlControlsContainer";
            this.pnlControlsContainer.Size = new System.Drawing.Size(540, 304);
            this.pnlControlsContainer.TabIndex = 0;
            // 
            // btnSystemPower
            // 
            this.btnSystemPower.BackColor = System.Drawing.Color.ForestGreen;
            this.btnSystemPower.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSystemPower.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSystemPower.ForeColor = System.Drawing.Color.White;
            this.btnSystemPower.Location = new System.Drawing.Point(115, 223);
            this.btnSystemPower.Name = "btnSystemPower";
            this.btnSystemPower.Size = new System.Drawing.Size(370, 45);
            this.btnSystemPower.TabIndex = 10;
            this.btnSystemPower.Text = "🔌 SISTEMA: ENCENDIDO";
            this.btnSystemPower.UseVisualStyleBackColor = false;
            // 
            // lblUserSelect
            // 
            this.lblUserSelect.AutoSize = true;
            this.lblUserSelect.Location = new System.Drawing.Point(111, 21);
            this.lblUserSelect.Name = "lblUserSelect";
            this.lblUserSelect.Size = new System.Drawing.Size(128, 20);
            this.lblUserSelect.TabIndex = 8;
            this.lblUserSelect.Text = "Tarjeta Empleado:";
            // 
            // cmbUsuarios
            // 
            this.cmbUsuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.cmbUsuarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbUsuarios.ForeColor = System.Drawing.Color.White;
            this.cmbUsuarios.FormattingEnabled = true;
            this.cmbUsuarios.Location = new System.Drawing.Point(245, 18);
            this.cmbUsuarios.Name = "cmbUsuarios";
            this.cmbUsuarios.Size = new System.Drawing.Size(240, 28);
            this.cmbUsuarios.TabIndex = 9;
            // 
            // chkModoRetiro
            // 
            this.chkModoRetiro.AutoSize = true;
            this.chkModoRetiro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkModoRetiro.ForeColor = System.Drawing.Color.Orange;
            this.chkModoRetiro.Location = new System.Drawing.Point(300, 108);
            this.chkModoRetiro.Name = "chkModoRetiro";
            this.chkModoRetiro.Size = new System.Drawing.Size(127, 27);
            this.chkModoRetiro.TabIndex = 5;
            this.chkModoRetiro.Text = "Modo Retiro";
            this.chkModoRetiro.UseVisualStyleBackColor = true;
            // 
            // chkSimularFalloRed
            // 
            this.chkSimularFalloRed.AutoSize = true;
            this.chkSimularFalloRed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkSimularFalloRed.ForeColor = System.Drawing.Color.IndianRed;
            this.chkSimularFalloRed.Location = new System.Drawing.Point(140, 108);
            this.chkSimularFalloRed.Name = "chkSimularFalloRed";
            this.chkSimularFalloRed.Size = new System.Drawing.Size(126, 27);
            this.chkSimularFalloRed.TabIndex = 4;
            this.chkSimularFalloRed.Text = "Fallo De Red";
            this.chkSimularFalloRed.UseVisualStyleBackColor = true;
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.txtBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBarcode.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtBarcode.ForeColor = System.Drawing.Color.White;
            this.txtBarcode.Location = new System.Drawing.Point(115, 138);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(370, 31);
            this.txtBarcode.TabIndex = 1;
            this.txtBarcode.Text = "SKU Manual";
            this.txtBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnScanBarcode
            // 
            this.btnScanBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnScanBarcode.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnScanBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanBarcode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnScanBarcode.ForeColor = System.Drawing.Color.White;
            this.btnScanBarcode.Location = new System.Drawing.Point(115, 173);
            this.btnScanBarcode.Name = "btnScanBarcode";
            this.btnScanBarcode.Size = new System.Drawing.Size(370, 35);
            this.btnScanBarcode.TabIndex = 2;
            this.btnScanBarcode.Text = "🔫 Escanear Código";
            this.btnScanBarcode.UseVisualStyleBackColor = false;
            // 
            // btnRFID
            // 
            this.btnRFID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnRFID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRFID.FlatAppearance.BorderSize = 0;
            this.btnRFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRFID.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRFID.ForeColor = System.Drawing.Color.White;
            this.btnRFID.Location = new System.Drawing.Point(115, 53);
            this.btnRFID.Name = "btnRFID";
            this.btnRFID.Size = new System.Drawing.Size(370, 45);
            this.btnRFID.TabIndex = 3;
            this.btnRFID.Text = "💳 Pasar Tarjeta RFID (Confirmar)";
            this.btnRFID.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanelKeypad
            // 
            this.flowLayoutPanelKeypad.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelKeypad.Name = "flowLayoutPanelKeypad";
            this.flowLayoutPanelKeypad.Size = new System.Drawing.Size(281, 298);
            this.flowLayoutPanelKeypad.TabIndex = 0;
            // 
            // groupBoxOled
            // 
            this.groupBoxOled.Controls.Add(this.pnlOledScreen);
            this.groupBoxOled.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxOled.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBoxOled.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBoxOled.Location = new System.Drawing.Point(15, 15);
            this.groupBoxOled.Name = "groupBoxOled";
            this.groupBoxOled.Size = new System.Drawing.Size(1039, 117);
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
            this.pnlOledScreen.Size = new System.Drawing.Size(1033, 91);
            this.pnlOledScreen.TabIndex = 0;
            // 
            // lblOled
            // 
            this.lblOled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOled.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblOled.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblOled.Location = new System.Drawing.Point(5, 5);
            this.lblOled.Name = "lblOled";
            this.lblOled.Size = new System.Drawing.Size(1021, 79);
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
            this.rtbLog.Size = new System.Drawing.Size(1869, 126);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(1869, 655);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.flpBalanzas);
            this.Controls.Add(this.rtbLog);
            this.Name = "MainForm";
            this.Text = "OCRIS | OuroCore Realtime Inventory System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.groupBoxPinpad.ResumeLayout(false);
            this.tlpCentering.ResumeLayout(false);
            this.pnlControlsContainer.ResumeLayout(false);
            this.pnlControlsContainer.PerformLayout();
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

        // Controles nuevos para el centrado
        private System.Windows.Forms.TableLayoutPanel tlpCentering;
        private System.Windows.Forms.Panel pnlControlsContainer;

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelKeypad;
        private System.Windows.Forms.Button btnRFID;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Button btnScanBarcode;
        private System.Windows.Forms.FlowLayoutPanel flpProductos;
        private System.Windows.Forms.Label lblProductos;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.CheckBox chkSimularFalloRed;
        private System.Windows.Forms.CheckBox chkModoRetiro;
        private System.Windows.Forms.ComboBox cmbUsuarios;
        private System.Windows.Forms.Label lblUserSelect;
        private System.Windows.Forms.Button btnSystemPower;
    }
}