namespace ClientForm
{
    partial class Config
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblIpInfo = new Label();
            lblPortInfo = new Label();
            txtIP = new TextBox();
            txtPort = new TextBox();
            btnSave = new Button();
            SuspendLayout();
            // 
            // lblIpInfo
            // 
            lblIpInfo.AutoSize = true;
            lblIpInfo.Location = new Point(12, 20);
            lblIpInfo.Name = "lblIpInfo";
            lblIpInfo.Size = new Size(17, 15);
            lblIpInfo.TabIndex = 0;
            lblIpInfo.Text = "IP";
            // 
            // lblPortInfo
            // 
            lblPortInfo.AutoSize = true;
            lblPortInfo.Location = new Point(12, 79);
            lblPortInfo.Name = "lblPortInfo";
            lblPortInfo.Size = new Size(35, 15);
            lblPortInfo.TabIndex = 1;
            lblPortInfo.Text = "PORT";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(12, 38);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(100, 23);
            txtIP.TabIndex = 2;
            txtIP.TextChanged += TxtIP_TextChanged;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(12, 97);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 23);
            txtPort.TabIndex = 3;
            txtPort.TextChanged += TxtPort_TextChanged;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(12, 151);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 23);
            btnSave.TabIndex = 4;
            btnSave.Text = "SAVE";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // Config
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(126, 197);
            Controls.Add(btnSave);
            Controls.Add(txtPort);
            Controls.Add(txtIP);
            Controls.Add(lblPortInfo);
            Controls.Add(lblIpInfo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Config";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Config";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIpInfo;
        private Label lblPortInfo;
        private TextBox txtIP;
        private TextBox txtPort;
        private Button btnSave;
    }
}