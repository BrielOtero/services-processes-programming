namespace _01_exercise
{
    partial class Form1
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
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnChangePath = new System.Windows.Forms.Button();
            this.lstSubdirectories = new System.Windows.Forms.ListBox();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.PlaceholderText = "INSERT NEW PATH";
            this.txtPath.Size = new System.Drawing.Size(277, 23);
            this.txtPath.TabIndex = 0;
            // 
            // btnChangePath
            // 
            this.btnChangePath.Location = new System.Drawing.Point(313, 12);
            this.btnChangePath.Name = "btnChangePath";
            this.btnChangePath.Size = new System.Drawing.Size(86, 23);
            this.btnChangePath.TabIndex = 1;
            this.btnChangePath.Text = "Change Path";
            this.btnChangePath.UseVisualStyleBackColor = true;
            this.btnChangePath.Click += new System.EventHandler(this.btnChangePath_Click);
            // 
            // lstSubdirectories
            // 
            this.lstSubdirectories.FormattingEnabled = true;
            this.lstSubdirectories.ItemHeight = 15;
            this.lstSubdirectories.Location = new System.Drawing.Point(31, 89);
            this.lstSubdirectories.Name = "lstSubdirectories";
            this.lstSubdirectories.Size = new System.Drawing.Size(120, 94);
            this.lstSubdirectories.TabIndex = 2;
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.ItemHeight = 15;
            this.lstFiles.Location = new System.Drawing.Point(169, 89);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(120, 94);
            this.lstFiles.TabIndex = 3;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnChangePath;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.lstSubdirectories);
            this.Controls.Add(this.btnChangePath);
            this.Controls.Add(this.txtPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtPath;
        private Button btnChangePath;
        private ListBox lstSubdirectories;
        private ListBox lstFiles;
    }
}