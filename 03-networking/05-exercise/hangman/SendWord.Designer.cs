namespace hangman
{
    partial class SendWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendWord));
            this.btnSend = new System.Windows.Forms.Button();
            this.chFile = new System.Windows.Forms.CheckBox();
            this.txtWords = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(12, 80);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(132, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // chFile
            // 
            this.chFile.AutoSize = true;
            this.chFile.Location = new System.Drawing.Point(12, 12);
            this.chFile.Name = "chFile";
            this.chFile.Size = new System.Drawing.Size(70, 17);
            this.chFile.TabIndex = 1;
            this.chFile.Text = "Send File";
            this.chFile.UseVisualStyleBackColor = true;
            this.chFile.CheckedChanged += new System.EventHandler(this.ChFile_CheckedChanged);
            // 
            // txtWords
            // 
            this.txtWords.Location = new System.Drawing.Point(12, 35);
            this.txtWords.Name = "txtWords";
            this.txtWords.Size = new System.Drawing.Size(132, 20);
            this.txtWords.TabIndex = 2;
            this.txtWords.TextChanged += new System.EventHandler(this.TxtWords_TextChanged);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(39, 33);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "Select File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Visible = false;
            this.btnOpenFile.Click += new System.EventHandler(this.BtnOpenFile_Click);
            // 
            // SendWord
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(157, 115);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtWords);
            this.Controls.Add(this.chFile);
            this.Controls.Add(this.btnSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendWord";
            this.Text = "SendWord";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.CheckBox chFile;
        private System.Windows.Forms.TextBox txtWords;
        private System.Windows.Forms.Button btnOpenFile;
    }
}