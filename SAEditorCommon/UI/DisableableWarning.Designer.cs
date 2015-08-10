namespace SonicRetro.SAModel.SAEditorCommon.UI
{
	partial class DisableableWarning
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
			this.MainTextLabel = new System.Windows.Forms.Label();
			this.EnableCheckBox = new System.Windows.Forms.CheckBox();
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// MainTextLabel
			// 
			this.MainTextLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.MainTextLabel.Location = new System.Drawing.Point(12, 31);
			this.MainTextLabel.Name = "MainTextLabel";
			this.MainTextLabel.Size = new System.Drawing.Size(298, 106);
			this.MainTextLabel.TabIndex = 0;
			this.MainTextLabel.Text = "label1";
			this.MainTextLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// EnableCheckBox
			// 
			this.EnableCheckBox.AutoSize = true;
			this.EnableCheckBox.Checked = true;
			this.EnableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.EnableCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.EnableCheckBox.Location = new System.Drawing.Point(15, 140);
			this.EnableCheckBox.Name = "EnableCheckBox";
			this.EnableCheckBox.Size = new System.Drawing.Size(174, 17);
			this.EnableCheckBox.TabIndex = 1;
			this.EnableCheckBox.Text = "Show this warning in the future.";
			this.EnableCheckBox.UseVisualStyleBackColor = true;
			// 
			// acceptButton
			// 
			this.acceptButton.Location = new System.Drawing.Point(235, 175);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 2;
			this.acceptButton.Text = "OK";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(154, 175);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// DisableableWarning
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(322, 210);
			this.ControlBox = false;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.EnableCheckBox);
			this.Controls.Add(this.MainTextLabel);
			this.Name = "DisableableWarning";
			this.Text = "Warning";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.CheckBox EnableCheckBox;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		public System.Windows.Forms.Label MainTextLabel;
	}
}