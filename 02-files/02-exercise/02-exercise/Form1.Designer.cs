namespace _02_exercise
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
            this.txtTextToSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblPath = new System.Windows.Forms.Label();
            this.lblTextToSearch = new System.Windows.Forms.Label();
            this.chbCase = new System.Windows.Forms.CheckBox();
            this.txtExtensions = new System.Windows.Forms.TextBox();
            this.lblWords = new System.Windows.Forms.Label();
            this.lblExtensions = new System.Windows.Forms.Label();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 25);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(284, 23);
            this.txtPath.TabIndex = 0;
            // 
            // txtTextToSearch
            // 
            this.txtTextToSearch.Location = new System.Drawing.Point(334, 25);
            this.txtTextToSearch.Name = "txtTextToSearch";
            this.txtTextToSearch.Size = new System.Drawing.Size(200, 23);
            this.txtTextToSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(557, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(12, 7);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(31, 15);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "Path";
            // 
            // lblTextToSearch
            // 
            this.lblTextToSearch.AutoSize = true;
            this.lblTextToSearch.Location = new System.Drawing.Point(334, 7);
            this.lblTextToSearch.Name = "lblTextToSearch";
            this.lblTextToSearch.Size = new System.Drawing.Size(79, 15);
            this.lblTextToSearch.TabIndex = 4;
            this.lblTextToSearch.Text = "Text to search";
            // 
            // chbCase
            // 
            this.chbCase.AutoSize = true;
            this.chbCase.Location = new System.Drawing.Point(334, 55);
            this.chbCase.Name = "chbCase";
            this.chbCase.Size = new System.Drawing.Size(100, 19);
            this.chbCase.TabIndex = 6;
            this.chbCase.Text = "Sensitive Case";
            this.chbCase.UseVisualStyleBackColor = true;
            // 
            // txtExtensions
            // 
            this.txtExtensions.Location = new System.Drawing.Point(648, 25);
            this.txtExtensions.Name = "txtExtensions";
            this.txtExtensions.Size = new System.Drawing.Size(140, 23);
            this.txtExtensions.TabIndex = 7;
            // 
            // lblWords
            // 
            this.lblWords.AutoSize = true;
            this.lblWords.Location = new System.Drawing.Point(12, 62);
            this.lblWords.Name = "lblWords";
            this.lblWords.Size = new System.Drawing.Size(41, 15);
            this.lblWords.TabIndex = 8;
            this.lblWords.Text = "Words";
            // 
            // lblExtensions
            // 
            this.lblExtensions.AutoSize = true;
            this.lblExtensions.Location = new System.Drawing.Point(648, 9);
            this.lblExtensions.Name = "lblExtensions";
            this.lblExtensions.Size = new System.Drawing.Size(63, 15);
            this.lblExtensions.TabIndex = 9;
            this.lblExtensions.Text = "Extensions";
            // 
            // lstFiles
            // 
            this.lstFiles.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.ItemHeight = 16;
            this.lstFiles.Location = new System.Drawing.Point(12, 80);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(776, 356);
            this.lstFiles.TabIndex = 10;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.lblExtensions);
            this.Controls.Add(this.lblWords);
            this.Controls.Add(this.txtExtensions);
            this.Controls.Add(this.chbCase);
            this.Controls.Add(this.lblTextToSearch);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtTextToSearch);
            this.Controls.Add(this.txtPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "The Fastest Word Counter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtPath;
        private TextBox txtTextToSearch;
        private Button btnSearch;
        private Label lblPath;
        private Label lblTextToSearch;
        private CheckBox chbCase;
        private TextBox txtExtensions;
        private Label lblWords;
        private Label lblExtensions;
        private ListBox lstFiles;
    }
}