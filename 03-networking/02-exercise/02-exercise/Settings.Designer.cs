namespace _02_exercise
{
    partial class Settings
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
            txtIp = new TextBox();
            txtPort = new TextBox();
            txtUser = new TextBox();
            lblIpInfo = new Label();
            lblPortInfo = new Label();
            lblUserInfo = new Label();
            btnSave = new Button();
            SuspendLayout();
            // 
            // txtIp
            // 
            txtIp.Location = new Point(12, 27);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(100, 23);
            txtIp.TabIndex = 0;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(12, 89);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 23);
            txtPort.TabIndex = 1;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(12, 159);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(100, 23);
            txtUser.TabIndex = 2;
            // 
            // lblIpInfo
            // 
            lblIpInfo.AutoSize = true;
            lblIpInfo.Location = new Point(12, 9);
            lblIpInfo.Name = "lblIpInfo";
            lblIpInfo.Size = new Size(17, 15);
            lblIpInfo.TabIndex = 3;
            lblIpInfo.Text = "IP";
            // 
            // lblPortInfo
            // 
            lblPortInfo.AutoSize = true;
            lblPortInfo.Location = new Point(12, 71);
            lblPortInfo.Name = "lblPortInfo";
            lblPortInfo.Size = new Size(35, 15);
            lblPortInfo.TabIndex = 4;
            lblPortInfo.Text = "PORT";
            // 
            // lblUserInfo
            // 
            lblUserInfo.AutoSize = true;
            lblUserInfo.Location = new Point(12, 141);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(34, 15);
            lblUserInfo.TabIndex = 5;
            lblUserInfo.Text = "USER";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(12, 216);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 23);
            btnSave.TabIndex = 6;
            btnSave.Text = "SAVE";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(124, 251);
            Controls.Add(btnSave);
            Controls.Add(lblUserInfo);
            Controls.Add(lblPortInfo);
            Controls.Add(lblIpInfo);
            Controls.Add(txtUser);
            Controls.Add(txtPort);
            Controls.Add(txtIp);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Config";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIp;
        private TextBox txtPort;
        private TextBox txtUser;
        private Label lblIpInfo;
        private Label lblPortInfo;
        private Label lblUserInfo;
        private Button btnSave;
    }
}