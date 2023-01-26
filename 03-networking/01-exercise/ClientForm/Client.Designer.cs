namespace ClientForm
{
    partial class Client
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            btnTime = new Button();
            btnDate = new Button();
            btnAll = new Button();
            btnClose = new Button();
            txtPassword = new TextBox();
            lblResponse = new Label();
            lblCommandInfo = new Label();
            lblPasswordInfo = new Label();
            btnConfig = new Button();
            lblConfigInfo = new Label();
            SuspendLayout();
            // 
            // btnTime
            // 
            btnTime.Location = new Point(23, 46);
            btnTime.Name = "btnTime";
            btnTime.Size = new Size(75, 23);
            btnTime.TabIndex = 0;
            btnTime.Text = "TIME";
            btnTime.UseVisualStyleBackColor = true;
            btnTime.Click += SendCommand;
            // 
            // btnDate
            // 
            btnDate.Location = new Point(23, 75);
            btnDate.Name = "btnDate";
            btnDate.Size = new Size(75, 23);
            btnDate.TabIndex = 1;
            btnDate.Text = "DATE";
            btnDate.UseVisualStyleBackColor = true;
            btnDate.Click += SendCommand;
            // 
            // btnAll
            // 
            btnAll.Location = new Point(23, 104);
            btnAll.Name = "btnAll";
            btnAll.Size = new Size(75, 23);
            btnAll.TabIndex = 2;
            btnAll.Text = "ALL";
            btnAll.UseVisualStyleBackColor = true;
            btnAll.Click += SendCommand;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(23, 133);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 3;
            btnClose.Text = "CLOSE";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += SendCommand;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(160, 133);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(172, 23);
            txtPassword.TabIndex = 4;
            // 
            // lblResponse
            // 
            lblResponse.Location = new Point(39, 159);
            lblResponse.Name = "lblResponse";
            lblResponse.Size = new Size(357, 50);
            lblResponse.TabIndex = 5;
            lblResponse.Text = "Waiting for response";
            lblResponse.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCommandInfo
            // 
            lblCommandInfo.AutoSize = true;
            lblCommandInfo.Location = new Point(23, 19);
            lblCommandInfo.Name = "lblCommandInfo";
            lblCommandInfo.Size = new Size(105, 15);
            lblCommandInfo.TabIndex = 6;
            lblCommandInfo.Text = "Choose command";
            // 
            // lblPasswordInfo
            // 
            lblPasswordInfo.AutoSize = true;
            lblPasswordInfo.Location = new Point(160, 108);
            lblPasswordInfo.Name = "lblPasswordInfo";
            lblPasswordInfo.Size = new Size(57, 15);
            lblPasswordInfo.TabIndex = 7;
            lblPasswordInfo.Text = "Password";
            // 
            // btnConfig
            // 
            btnConfig.Location = new Point(336, 46);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(75, 23);
            btnConfig.TabIndex = 8;
            btnConfig.Text = "CONFIG";
            btnConfig.UseVisualStyleBackColor = true;
            btnConfig.Click += BtnConfig_Click;
            // 
            // lblConfigInfo
            // 
            lblConfigInfo.AutoSize = true;
            lblConfigInfo.Location = new Point(324, 19);
            lblConfigInfo.Name = "lblConfigInfo";
            lblConfigInfo.Size = new Size(100, 15);
            lblConfigInfo.TabIndex = 9;
            lblConfigInfo.Text = "Change ip or port";
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(448, 218);
            Controls.Add(lblConfigInfo);
            Controls.Add(btnConfig);
            Controls.Add(lblPasswordInfo);
            Controls.Add(lblCommandInfo);
            Controls.Add(lblResponse);
            Controls.Add(txtPassword);
            Controls.Add(btnClose);
            Controls.Add(btnAll);
            Controls.Add(btnDate);
            Controls.Add(btnTime);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Client";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnTime;
        private Button btnDate;
        private Button btnAll;
        private Button btnClose;
        private TextBox txtPassword;
        private Label lblResponse;
        private Label lblCommandInfo;
        private Label lblPasswordInfo;
        private Button btnConfig;
        private Label lblConfigInfo;
    }
}