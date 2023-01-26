namespace _02_exercise
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
            btnSettings = new Button();
            lblShow = new Label();
            btnAdd = new Button();
            btnList = new Button();
            SuspendLayout();
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(713, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(75, 23);
            btnSettings.TabIndex = 0;
            btnSettings.Text = "SETTINGS";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += BtnSettings_Click;
            // 
            // lblShow
            // 
            lblShow.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblShow.Location = new Point(12, 43);
            lblShow.Name = "lblShow";
            lblShow.Size = new Size(776, 398);
            lblShow.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 12);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "ADD";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += Btn_Click;
            // 
            // btnList
            // 
            btnList.Location = new Point(93, 12);
            btnList.Name = "btnList";
            btnList.Size = new Size(75, 23);
            btnList.TabIndex = 3;
            btnList.Text = "LIST";
            btnList.UseVisualStyleBackColor = true;
            btnList.Click += Btn_Click;
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnList);
            Controls.Add(btnAdd);
            Controls.Add(lblShow);
            Controls.Add(btnSettings);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Client";
            Text = "SHIFTS";
            ResumeLayout(false);
        }

        #endregion

        private Button btnSettings;
        private Label lblShow;
        private Button btnAdd;
        private Button btnList;
    }
}