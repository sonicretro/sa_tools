namespace DLLModGenerator
{
    partial class MainForm
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CheckAllButton = new System.Windows.Forms.Button();
			this.CheckModifiedButton = new System.Windows.Forms.Button();
			this.UncheckAllButton = new System.Windows.Forms.Button();
			this.ExportCPPOldButton = new System.Windows.Forms.Button();
			this.ExportCPPNewButton = new System.Windows.Forms.Button();
			this.ExportINIButton = new System.Windows.Forms.Button();
			this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(584, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(106, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.CheckBoxes = true;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listView1.FullRowSelect = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.Location = new System.Drawing.Point(12, 27);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(560, 286);
			this.listView1.TabIndex = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 240;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Changed";
			// 
			// CheckAllButton
			// 
			this.CheckAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CheckAllButton.AutoSize = true;
			this.CheckAllButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CheckAllButton.Location = new System.Drawing.Point(12, 319);
			this.CheckAllButton.Name = "CheckAllButton";
			this.CheckAllButton.Size = new System.Drawing.Size(62, 23);
			this.CheckAllButton.TabIndex = 2;
			this.CheckAllButton.Text = "Check All";
			this.CheckAllButton.UseVisualStyleBackColor = true;
			this.CheckAllButton.Click += new System.EventHandler(this.CheckAllButton_Click);
			// 
			// CheckModifiedButton
			// 
			this.CheckModifiedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CheckModifiedButton.AutoSize = true;
			this.CheckModifiedButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CheckModifiedButton.Location = new System.Drawing.Point(80, 319);
			this.CheckModifiedButton.Name = "CheckModifiedButton";
			this.CheckModifiedButton.Size = new System.Drawing.Size(91, 23);
			this.CheckModifiedButton.TabIndex = 3;
			this.CheckModifiedButton.Text = "Check Modified";
			this.CheckModifiedButton.UseVisualStyleBackColor = true;
			this.CheckModifiedButton.Click += new System.EventHandler(this.CheckModifiedButton_Click);
			// 
			// UncheckAllButton
			// 
			this.UncheckAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.UncheckAllButton.AutoSize = true;
			this.UncheckAllButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.UncheckAllButton.Location = new System.Drawing.Point(177, 319);
			this.UncheckAllButton.Name = "UncheckAllButton";
			this.UncheckAllButton.Size = new System.Drawing.Size(75, 23);
			this.UncheckAllButton.TabIndex = 4;
			this.UncheckAllButton.Text = "Uncheck All";
			this.UncheckAllButton.UseVisualStyleBackColor = true;
			this.UncheckAllButton.Click += new System.EventHandler(this.UncheckAllButton_Click);
			// 
			// ExportCPPOldButton
			// 
			this.ExportCPPOldButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ExportCPPOldButton.AutoSize = true;
			this.ExportCPPOldButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ExportCPPOldButton.Location = new System.Drawing.Point(258, 319);
			this.ExportCPPOldButton.Name = "ExportCPPOldButton";
			this.ExportCPPOldButton.Size = new System.Drawing.Size(92, 23);
			this.ExportCPPOldButton.TabIndex = 5;
			this.ExportCPPOldButton.Text = "Export C++ (old)";
			this.ExportCPPOldButton.UseVisualStyleBackColor = true;
			this.ExportCPPOldButton.Click += new System.EventHandler(this.ExportCPPOldButton_Click);
			// 
			// ExportCPPNewButton
			// 
			this.ExportCPPNewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ExportCPPNewButton.AutoSize = true;
			this.ExportCPPNewButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ExportCPPNewButton.Location = new System.Drawing.Point(356, 319);
			this.ExportCPPNewButton.Name = "ExportCPPNewButton";
			this.ExportCPPNewButton.Size = new System.Drawing.Size(98, 23);
			this.ExportCPPNewButton.TabIndex = 6;
			this.ExportCPPNewButton.Text = "Export C++ (new)";
			this.ExportCPPNewButton.UseVisualStyleBackColor = true;
			this.ExportCPPNewButton.Click += new System.EventHandler(this.ExportCPPNewButton_Click);
			// 
			// ExportINIButton
			// 
			this.ExportINIButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ExportINIButton.AutoSize = true;
			this.ExportINIButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ExportINIButton.Location = new System.Drawing.Point(460, 319);
			this.ExportINIButton.Name = "ExportINIButton";
			this.ExportINIButton.Size = new System.Drawing.Size(64, 23);
			this.ExportINIButton.TabIndex = 7;
			this.ExportINIButton.Text = "Export INI";
			this.ExportINIButton.UseVisualStyleBackColor = true;
			this.ExportINIButton.Click += new System.EventHandler(this.ExportINIButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 351);
			this.Controls.Add(this.ExportINIButton);
			this.Controls.Add(this.ExportCPPNewButton);
			this.Controls.Add(this.ExportCPPOldButton);
			this.Controls.Add(this.UncheckAllButton);
			this.Controls.Add(this.CheckModifiedButton);
			this.Controls.Add(this.CheckAllButton);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "DLL Mod Generator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button CheckAllButton;
        private System.Windows.Forms.Button CheckModifiedButton;
        private System.Windows.Forms.Button UncheckAllButton;
        private System.Windows.Forms.Button ExportCPPOldButton;
		private System.Windows.Forms.Button ExportCPPNewButton;
		private System.Windows.Forms.Button ExportINIButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowser;
    }
}

