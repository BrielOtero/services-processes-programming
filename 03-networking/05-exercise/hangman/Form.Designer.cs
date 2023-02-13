namespace hangman
{
    partial class Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.dhHangman = new _04_exercise.DrawHanged();
            this.txtTryLetter = new System.Windows.Forms.TextBox();
            this.btnTryLetter = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pLetters = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(400, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiMenu
            // 
            this.tsmiMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewGame});
            this.tsmiMenu.Name = "tsmiMenu";
            this.tsmiMenu.Size = new System.Drawing.Size(50, 20);
            this.tsmiMenu.Text = "&Menu";
            // 
            // tsmiNewGame
            // 
            this.tsmiNewGame.Name = "tsmiNewGame";
            this.tsmiNewGame.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tsmiNewGame.Size = new System.Drawing.Size(175, 22);
            this.tsmiNewGame.Text = "&New Game";
            this.tsmiNewGame.Click += new System.EventHandler(this.TsmiNewGame_Click);
            // 
            // dhHangman
            // 
            this.dhHangman.Location = new System.Drawing.Point(36, 27);
            this.dhHangman.Mistakes = 0;
            this.dhHangman.Name = "dhHangman";
            this.dhHangman.Size = new System.Drawing.Size(327, 376);
            this.dhHangman.TabIndex = 2;
            this.dhHangman.Visible = false;
            this.dhHangman.Hanged += new System.EventHandler(this.DhHangman_Hanged);
            // 
            // txtTryLetter
            // 
            this.txtTryLetter.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTryLetter.Location = new System.Drawing.Point(249, 459);
            this.txtTryLetter.MaxLength = 1;
            this.txtTryLetter.Multiline = true;
            this.txtTryLetter.Name = "txtTryLetter";
            this.txtTryLetter.Size = new System.Drawing.Size(30, 30);
            this.txtTryLetter.TabIndex = 3;
            this.txtTryLetter.Visible = false;
            this.txtTryLetter.TextChanged += new System.EventHandler(this.TxtTryLetter_TextChanged);
            // 
            // btnTryLetter
            // 
            this.btnTryLetter.Location = new System.Drawing.Point(285, 459);
            this.btnTryLetter.Name = "btnTryLetter";
            this.btnTryLetter.Size = new System.Drawing.Size(103, 30);
            this.btnTryLetter.TabIndex = 4;
            this.btnTryLetter.Text = "TRY LETTER";
            this.btnTryLetter.UseVisualStyleBackColor = true;
            this.btnTryLetter.Visible = false;
            this.btnTryLetter.Click += new System.EventHandler(this.BtnTryLetter_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(100, 466);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(105, 16);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = "INSERT LETTER";
            this.lblInfo.Visible = false;
            // 
            // pLetters
            // 
            this.pLetters.Location = new System.Drawing.Point(12, 409);
            this.pLetters.Name = "pLetters";
            this.pLetters.Size = new System.Drawing.Size(376, 35);
            this.pLetters.TabIndex = 6;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(33, 466);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(38, 16);
            this.lblTime.TabIndex = 7;
            this.lblTime.Text = "00:00";
            this.lblTime.Visible = false;
            // 
            // Form
            // 
            this.AcceptButton = this.btnTryLetter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 501);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.pLetters);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnTryLetter);
            this.Controls.Add(this.txtTryLetter);
            this.Controls.Add(this.dhHangman);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form";
            this.Text = "Hangman";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewGame;
        private _04_exercise.DrawHanged dhHangman;
        private System.Windows.Forms.TextBox txtTryLetter;
        private System.Windows.Forms.Button btnTryLetter;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel pLetters;
        private System.Windows.Forms.Label lblTime;
    }
}

