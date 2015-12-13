namespace SonicRetro.SAModel.SAMDL
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.objToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.sa1mdlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colladaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cStructsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.objToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modelLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.UserControl();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.importObjDialog = new System.Windows.Forms.OpenFileDialog();
			this.importSA1MdlDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.selectModeButton = new System.Windows.Forms.ToolStripButton();
			this.moveModeButton = new System.Windows.Forms.ToolStripButton();
			this.rotateModeButton = new System.Windows.Forms.ToolStripButton();
			this.scaleModeButton = new System.Windows.Forms.ToolStripButton();
			this.gizmoSpaceComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.pivotComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.menuStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
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
            this.loadTexturesToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// loadTexturesToolStripMenuItem
			// 
			this.loadTexturesToolStripMenuItem.Name = "loadTexturesToolStripMenuItem";
			this.loadTexturesToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.loadTexturesToolStripMenuItem.Text = "Load Textures...";
			this.loadTexturesToolStripMenuItem.Click += new System.EventHandler(this.loadTexturesToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Enabled = false;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.saveToolStripMenuItem.Text = "&Save...";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// importToolStripMenuItem
			// 
			this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objToolStripMenuItem1,
            this.sa1mdlToolStripMenuItem});
			this.importToolStripMenuItem.Enabled = false;
			this.importToolStripMenuItem.Name = "importToolStripMenuItem";
			this.importToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.importToolStripMenuItem.Text = "&Import";
			// 
			// objToolStripMenuItem1
			// 
			this.objToolStripMenuItem1.Name = "objToolStripMenuItem1";
			this.objToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
			this.objToolStripMenuItem1.Text = "*.obj";
			this.objToolStripMenuItem1.Click += new System.EventHandler(this.objToolStripMenuItem1_Click);
			// 
			// sa1mdlToolStripMenuItem
			// 
			this.sa1mdlToolStripMenuItem.Name = "sa1mdlToolStripMenuItem";
			this.sa1mdlToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.sa1mdlToolStripMenuItem.Text = "*.sa1mdl";
			this.sa1mdlToolStripMenuItem.Click += new System.EventHandler(this.sa1mdlToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colladaToolStripMenuItem,
            this.cStructsToolStripMenuItem,
            this.objToolStripMenuItem});
			this.exportToolStripMenuItem.Enabled = false;
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.exportToolStripMenuItem.Text = "&Export";
			// 
			// colladaToolStripMenuItem
			// 
			this.colladaToolStripMenuItem.Name = "colladaToolStripMenuItem";
			this.colladaToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.colladaToolStripMenuItem.Text = "&Collada";
			this.colladaToolStripMenuItem.Click += new System.EventHandler(this.colladaToolStripMenuItem_Click);
			// 
			// cStructsToolStripMenuItem
			// 
			this.cStructsToolStripMenuItem.Name = "cStructsToolStripMenuItem";
			this.cStructsToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.cStructsToolStripMenuItem.Text = "C &Structs";
			this.cStructsToolStripMenuItem.Click += new System.EventHandler(this.cStructsToolStripMenuItem_Click);
			// 
			// objToolStripMenuItem
			// 
			this.objToolStripMenuItem.Name = "objToolStripMenuItem";
			this.objToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.objToolStripMenuItem.Text = "&Obj";
			this.objToolStripMenuItem.Click += new System.EventHandler(this.objToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyModelToolStripMenuItem,
            this.pasteModelToolStripMenuItem,
            this.editMaterialsToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.findToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// copyModelToolStripMenuItem
			// 
			this.copyModelToolStripMenuItem.Enabled = false;
			this.copyModelToolStripMenuItem.Name = "copyModelToolStripMenuItem";
			this.copyModelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyModelToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.copyModelToolStripMenuItem.Text = "&Copy Model";
			this.copyModelToolStripMenuItem.Click += new System.EventHandler(this.copyModelToolStripMenuItem_Click);
			// 
			// pasteModelToolStripMenuItem
			// 
			this.pasteModelToolStripMenuItem.Enabled = false;
			this.pasteModelToolStripMenuItem.Name = "pasteModelToolStripMenuItem";
			this.pasteModelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteModelToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.pasteModelToolStripMenuItem.Text = "&Paste Model";
			this.pasteModelToolStripMenuItem.Click += new System.EventHandler(this.pasteModelToolStripMenuItem_Click);
			// 
			// editMaterialsToolStripMenuItem
			// 
			this.editMaterialsToolStripMenuItem.Enabled = false;
			this.editMaterialsToolStripMenuItem.Name = "editMaterialsToolStripMenuItem";
			this.editMaterialsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.editMaterialsToolStripMenuItem.Text = "Edit &Materials...";
			this.editMaterialsToolStripMenuItem.Click += new System.EventHandler(this.editMaterialsToolStripMenuItem_Click);
			// 
			// preferencesToolStripMenuItem
			// 
			this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
			this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.preferencesToolStripMenuItem.Text = "Preferences";
			this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
			// 
			// findToolStripMenuItem
			// 
			this.findToolStripMenuItem.Enabled = false;
			this.findToolStripMenuItem.Name = "findToolStripMenuItem";
			this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.findToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.findToolStripMenuItem.Text = "&Find...";
			this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelLibraryToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// modelLibraryToolStripMenuItem
			// 
			this.modelLibraryToolStripMenuItem.Name = "modelLibraryToolStripMenuItem";
			this.modelLibraryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.modelLibraryToolStripMenuItem.Text = "Model &Library";
			this.modelLibraryToolStripMenuItem.Click += new System.EventHandler(this.modelLibraryToolStripMenuItem_Click);
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(320, 513);
			this.panel1.TabIndex = 1;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			this.panel1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.panel1_KeyDown);
			this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
			this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel1_MouseMove);
			this.panel1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.panel1_PreviewKeyDown);
			// 
			// timer1
			// 
			this.timer1.Interval = 33;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 49);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.panel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
			this.splitContainer1.Size = new System.Drawing.Size(584, 513);
			this.splitContainer1.SplitterDistance = 320;
			this.splitContainer1.TabIndex = 2;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(260, 513);
			this.tabControl1.TabIndex = 15;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.propertyGrid1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(252, 487);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Properties";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Margin = new System.Windows.Forms.Padding(0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.propertyGrid1.Size = new System.Drawing.Size(252, 487);
			this.propertyGrid1.TabIndex = 14;
			this.propertyGrid1.ToolbarVisible = false;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.treeView1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(252, 487);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Tree";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(252, 487);
			this.treeView1.TabIndex = 1;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// importObjDialog
			// 
			this.importObjDialog.Filter = "*.obj Files|*.obj";
			this.importObjDialog.Multiselect = true;
			// 
			// importSA1MdlDialog
			// 
			this.importSA1MdlDialog.Filter = "SA1MDL Files|*.sa1mdl";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectModeButton,
            this.moveModeButton,
            this.rotateModeButton,
            this.scaleModeButton,
            this.gizmoSpaceComboBox,
            this.pivotComboBox});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(584, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// selectModeButton
			// 
			this.selectModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.selectModeButton.Image = ((System.Drawing.Image)(resources.GetObject("selectModeButton.Image")));
			this.selectModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.selectModeButton.Name = "selectModeButton";
			this.selectModeButton.Size = new System.Drawing.Size(23, 22);
			this.selectModeButton.Text = "toolStripButton1";
			this.selectModeButton.Click += new System.EventHandler(this.selectModeButton_Click);
			// 
			// moveModeButton
			// 
			this.moveModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.moveModeButton.Image = ((System.Drawing.Image)(resources.GetObject("moveModeButton.Image")));
			this.moveModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.moveModeButton.Name = "moveModeButton";
			this.moveModeButton.Size = new System.Drawing.Size(23, 22);
			this.moveModeButton.Text = "toolStripButton2";
			this.moveModeButton.Click += new System.EventHandler(this.moveModeButton_Click);
			// 
			// rotateModeButton
			// 
			this.rotateModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.rotateModeButton.Image = ((System.Drawing.Image)(resources.GetObject("rotateModeButton.Image")));
			this.rotateModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.rotateModeButton.Name = "rotateModeButton";
			this.rotateModeButton.Size = new System.Drawing.Size(23, 22);
			this.rotateModeButton.Text = "toolStripButton3";
			this.rotateModeButton.Click += new System.EventHandler(this.rotateModeButton_Click);
			// 
			// scaleModeButton
			// 
			this.scaleModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.scaleModeButton.Image = ((System.Drawing.Image)(resources.GetObject("scaleModeButton.Image")));
			this.scaleModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.scaleModeButton.Name = "scaleModeButton";
			this.scaleModeButton.Size = new System.Drawing.Size(23, 22);
			this.scaleModeButton.Text = "toolStripButton4";
			this.scaleModeButton.Click += new System.EventHandler(this.scaleModeButton_Click);
			// 
			// gizmoSpaceComboBox
			// 
			this.gizmoSpaceComboBox.Items.AddRange(new object[] {
            "Global",
            "Local"});
			this.gizmoSpaceComboBox.Name = "gizmoSpaceComboBox";
			this.gizmoSpaceComboBox.Size = new System.Drawing.Size(121, 25);
			// 
			// pivotComboBox
			// 
			this.pivotComboBox.Items.AddRange(new object[] {
            "CenterMass",
            "Origin"});
			this.pivotComboBox.Name = "pivotComboBox";
			this.pivotComboBox.Size = new System.Drawing.Size(121, 25);
			this.pivotComboBox.DropDownClosed += new System.EventHandler(this.pivotComboBox_DropDownClosed);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 562);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "SAMDL";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.UserControl panel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTexturesToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStripMenuItem colladaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cStructsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteModelToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripMenuItem editMaterialsToolStripMenuItem;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modelLibraryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem objToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem sa1mdlToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog importObjDialog;
		private System.Windows.Forms.OpenFileDialog importSA1MdlDialog;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton selectModeButton;
		private System.Windows.Forms.ToolStripButton moveModeButton;
		private System.Windows.Forms.ToolStripButton rotateModeButton;
		private System.Windows.Forms.ToolStripButton scaleModeButton;
		private System.Windows.Forms.ToolStripComboBox gizmoSpaceComboBox;
		private System.Windows.Forms.ToolStripComboBox pivotComboBox;
    }
}

