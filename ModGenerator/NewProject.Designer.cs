﻿namespace ModGenerator
{
	partial class NewProject
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
			this.cancelButton = new System.Windows.Forms.Button();
			this.acceptButton = new System.Windows.Forms.Button();
			this.GameDirectoryLabel = new System.Windows.Forms.Label();
			this.gamePathTextBox = new System.Windows.Forms.TextBox();
			this.projectNameLabel = new System.Windows.Forms.Label();
			this.ProjectNameText = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.GameTypeLabel = new System.Windows.Forms.Label();
			this.SADXRadioButton = new System.Windows.Forms.RadioButton();
			this.SA2PCButton = new System.Windows.Forms.RadioButton();
			this.splitBackgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.advancedSettingsButton = new System.Windows.Forms.Button();
			this.helpButton = new System.Windows.Forms.Button();
			this.authorLabel = new System.Windows.Forms.Label();
			this.authorText = new System.Windows.Forms.TextBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.descriptionText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(179, 208);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 9;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.Enabled = false;
			this.acceptButton.Location = new System.Drawing.Point(262, 208);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 10;
			this.acceptButton.Text = "OK";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// GameDirectoryLabel
			// 
			this.GameDirectoryLabel.AutoSize = true;
			this.GameDirectoryLabel.Location = new System.Drawing.Point(23, 126);
			this.GameDirectoryLabel.Name = "GameDirectoryLabel";
			this.GameDirectoryLabel.Size = new System.Drawing.Size(83, 13);
			this.GameDirectoryLabel.TabIndex = 2;
			this.GameDirectoryLabel.Text = "Game Directory:";
			// 
			// gamePathTextBox
			// 
			this.gamePathTextBox.Location = new System.Drawing.Point(26, 142);
			this.gamePathTextBox.Name = "gamePathTextBox";
			this.gamePathTextBox.Size = new System.Drawing.Size(228, 20);
			this.gamePathTextBox.TabIndex = 3;
			this.gamePathTextBox.TextChanged += new System.EventHandler(this.gamePathTextBox_TextChanged);
			// 
			// projectNameLabel
			// 
			this.projectNameLabel.AutoSize = true;
			this.projectNameLabel.Location = new System.Drawing.Point(23, 9);
			this.projectNameLabel.Name = "projectNameLabel";
			this.projectNameLabel.Size = new System.Drawing.Size(74, 13);
			this.projectNameLabel.TabIndex = 4;
			this.projectNameLabel.Text = "Project Name:";
			// 
			// ProjectNameText
			// 
			this.ProjectNameText.Location = new System.Drawing.Point(26, 25);
			this.ProjectNameText.Name = "ProjectNameText";
			this.ProjectNameText.Size = new System.Drawing.Size(228, 20);
			this.ProjectNameText.TabIndex = 0;
			this.ProjectNameText.TextChanged += new System.EventHandler(this.ProjectNameText_TextChanged);
			// 
			// browseButton
			// 
			this.browseButton.Location = new System.Drawing.Point(260, 141);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(75, 23);
			this.browseButton.TabIndex = 4;
			this.browseButton.Text = "&Browse";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// GameTypeLabel
			// 
			this.GameTypeLabel.AutoSize = true;
			this.GameTypeLabel.Location = new System.Drawing.Point(23, 165);
			this.GameTypeLabel.Name = "GameTypeLabel";
			this.GameTypeLabel.Size = new System.Drawing.Size(38, 13);
			this.GameTypeLabel.TabIndex = 7;
			this.GameTypeLabel.Text = "Game:";
			// 
			// SADXRadioButton
			// 
			this.SADXRadioButton.AutoSize = true;
			this.SADXRadioButton.Checked = true;
			this.SADXRadioButton.Location = new System.Drawing.Point(26, 181);
			this.SADXRadioButton.Name = "SADXRadioButton";
			this.SADXRadioButton.Size = new System.Drawing.Size(101, 17);
			this.SADXRadioButton.TabIndex = 5;
			this.SADXRadioButton.TabStop = true;
			this.SADXRadioButton.Text = "SADXPC (2004)";
			this.SADXRadioButton.UseVisualStyleBackColor = true;
			this.SADXRadioButton.CheckedChanged += new System.EventHandler(this.SADXRadioButton_CheckedChanged);
			// 
			// SA2PCButton
			// 
			this.SA2PCButton.AutoSize = true;
			this.SA2PCButton.Location = new System.Drawing.Point(208, 181);
			this.SA2PCButton.Name = "SA2PCButton";
			this.SA2PCButton.Size = new System.Drawing.Size(59, 17);
			this.SA2PCButton.TabIndex = 6;
			this.SA2PCButton.TabStop = true;
			this.SA2PCButton.Text = "SA2PC";
			this.SA2PCButton.UseVisualStyleBackColor = true;
			this.SA2PCButton.CheckedChanged += new System.EventHandler(this.SA2PCButton_CheckedChanged);
			// 
			// splitBackgroundWorker
			// 
			this.splitBackgroundWorker.WorkerReportsProgress = true;
			this.splitBackgroundWorker.WorkerSupportsCancellation = true;
			this.splitBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.splitBackgroundWorker_DoWork);
			this.splitBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.splitBackgroundWorker_ProgressChanged);
			this.splitBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.splitBackgroundWorker_RunWorkerCompleted);
			// 
			// advancedSettingsButton
			// 
			this.advancedSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.advancedSettingsButton.Location = new System.Drawing.Point(8, 208);
			this.advancedSettingsButton.Name = "advancedSettingsButton";
			this.advancedSettingsButton.Size = new System.Drawing.Size(73, 23);
			this.advancedSettingsButton.TabIndex = 7;
			this.advancedSettingsButton.Text = "&Advanced";
			this.advancedSettingsButton.UseVisualStyleBackColor = true;
			this.advancedSettingsButton.Click += new System.EventHandler(this.advancedSettingsButton_Click);
			// 
			// helpButton
			// 
			this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.helpButton.Location = new System.Drawing.Point(87, 208);
			this.helpButton.Name = "helpButton";
			this.helpButton.Size = new System.Drawing.Size(75, 23);
			this.helpButton.TabIndex = 8;
			this.helpButton.Text = "&Help";
			this.helpButton.UseVisualStyleBackColor = true;
			this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
			// 
			// authorLabel
			// 
			this.authorLabel.AutoSize = true;
			this.authorLabel.Location = new System.Drawing.Point(23, 48);
			this.authorLabel.Name = "authorLabel";
			this.authorLabel.Size = new System.Drawing.Size(41, 13);
			this.authorLabel.TabIndex = 12;
			this.authorLabel.Text = "Author:";
			// 
			// authorText
			// 
			this.authorText.Location = new System.Drawing.Point(26, 64);
			this.authorText.Name = "authorText";
			this.authorText.Size = new System.Drawing.Size(228, 20);
			this.authorText.TabIndex = 1;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.AutoSize = true;
			this.descriptionLabel.Location = new System.Drawing.Point(23, 87);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(63, 13);
			this.descriptionLabel.TabIndex = 14;
			this.descriptionLabel.Text = "Description:";
			// 
			// descriptionText
			// 
			this.descriptionText.Location = new System.Drawing.Point(26, 103);
			this.descriptionText.Name = "descriptionText";
			this.descriptionText.Size = new System.Drawing.Size(309, 20);
			this.descriptionText.TabIndex = 2;
			// 
			// NewProject
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(349, 243);
			this.Controls.Add(this.descriptionText);
			this.Controls.Add(this.descriptionLabel);
			this.Controls.Add(this.authorText);
			this.Controls.Add(this.authorLabel);
			this.Controls.Add(this.helpButton);
			this.Controls.Add(this.advancedSettingsButton);
			this.Controls.Add(this.SA2PCButton);
			this.Controls.Add(this.SADXRadioButton);
			this.Controls.Add(this.GameTypeLabel);
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.ProjectNameText);
			this.Controls.Add(this.projectNameLabel);
			this.Controls.Add(this.gamePathTextBox);
			this.Controls.Add(this.GameDirectoryLabel);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "NewProject";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Project";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewProject_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Label GameDirectoryLabel;
		private System.Windows.Forms.TextBox gamePathTextBox;
		private System.Windows.Forms.Label projectNameLabel;
		private System.Windows.Forms.TextBox ProjectNameText;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label GameTypeLabel;
        private System.ComponentModel.BackgroundWorker splitBackgroundWorker;
        public System.Windows.Forms.RadioButton SADXRadioButton;
        public System.Windows.Forms.RadioButton SA2PCButton;
        private System.Windows.Forms.Button advancedSettingsButton;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.TextBox authorText;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionText;
	}
}