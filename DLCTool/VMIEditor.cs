﻿using System;
using System.IO;
using System.Windows.Forms;
using static DLCTool.SA1DLC;

namespace DLCTool
{
	public partial class VMIEditor : Form
	{
        public VMIFile vmi;
        public VMSFile vms;

		public VMIEditor(VMSFile data)
		{
            vmi = new VMIFile(data, "SADV_" + data.Identifier.ToString("D3"));
            vms = data;
			InitializeComponent();
            comboBoxWeekday.SelectedIndex = 0;
        }

        private void UpdateAllLabels()
        {
            textBoxDescription.Text = vmi.Description;
            textBoxCopyright.Text = vmi.Copyright;
            numericUpDownVersion.Value = vmi.Version;
            numericUpDownFileID.Value = vmi.FileID;
            textBoxResourceName.Text = vmi.ResourceName;
            textBoxFileName.Text = vmi.FileName;
            checkBoxCopyProtect.Checked = vmi.Flags.HasFlag(VMIFile.VMIFlags.Protected);
            checkBoxGameSave.Checked = vmi.Flags.HasFlag(VMIFile.VMIFlags.Game);
            numericUpDownYear.Value = vmi.Year;
            numericUpDownMonth.Value = vmi.Month;
            numericUpDownDay.Value = vmi.Day;
            comboBoxWeekday.SelectedIndex = vmi.Weekday;
            numericUpDownHour.Value = vmi.Hour;
            numericUpDownMinute.Value = vmi.Minute;
            numericUpDownSecond.Value = vmi.Second;
        }

        private void buttonGenerateVMI_Click(object sender, EventArgs e)
        {
            vmi.Description = vms.Description;
            vmi.Copyright = vms.AppName;
            vmi.Version = 0;
            vmi.FileID = 1;
            vmi.ResourceName = "SA1_" + vms.Identifier.ToString("D3");
            vmi.FileName = "SONICADV_" + vms.Identifier.ToString("D3");
            vmi.Flags = 0;
            SetCurrentTime();
            UpdateAllLabels();
        }

        private void SetCurrentTime()
        {
            vmi.Year = (ushort)DateTime.Now.Year;
            vmi.Month = (byte)DateTime.Now.Month;
            vmi.Day = (byte)DateTime.Now.Day;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    vmi.Weekday = 0;
                    break;
                case DayOfWeek.Monday:
                    vmi.Weekday = 1;
                    break;
                case DayOfWeek.Tuesday:
                    vmi.Weekday = 2;
                    break;
                case DayOfWeek.Wednesday:
                    vmi.Weekday = 3;
                    break;
                case DayOfWeek.Thursday:
                    vmi.Weekday = 4;
                    break;
                case DayOfWeek.Friday:
                    vmi.Weekday = 5;
                    break;
                case DayOfWeek.Saturday:
                    vmi.Weekday = 6;
                    break;
            }
            vmi.Hour = (byte)DateTime.Now.Hour;
            vmi.Minute = (byte)DateTime.Now.Minute;
            vmi.Second = (byte)DateTime.Now.Second;
        }

		private void buttonSetCurrentTime_Click(object sender, EventArgs e)
		{
            SetCurrentTime();
            UpdateAllLabels();
        }

		private void buttonLoadVMI_Click(object sender, EventArgs e)
		{
            using (OpenFileDialog od = new OpenFileDialog() { DefaultExt = "vmi", Filter = "VMI Files|*.vmi|All Files|*.*" })
                if (od.ShowDialog(this) == DialogResult.OK)
                {
                    vmi = new VMIFile(File.ReadAllBytes(od.FileName));
                    UpdateAllLabels();
                }
        }

        private void buttonSaveVMI_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sv = new SaveFileDialog() { FileName = vmi.ResourceName, Title = "Save VMI File", Filter = "VMI Files|*.vmi", DefaultExt = "vmi" })
            {
                if (sv.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sv.FileName, vmi.GetBytes());
                }
            }
        }

		private void textBoxDescription_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Download description for the Dreamcast browser.";
		}

		private void textBoxCopyright_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Copyright displayed in the Dreamcast browser.";
        }

		private void numericUpDownVersion_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Version info displayed in the Dreamcast browser.";
        }

		private void numericUpDownFileID_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Download file ID, always set to 1.";
        }

		private void textBoxResourceName_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Filename on the server without extension (8 bytes or shorter).";
        }

		private void textBoxFileName_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Filename on the VMU without extension, must begin with 'SONICADV_'.";
        }

		private void numericUpDownYear_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "";
        }

		private void checkBoxGameSave_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Marks the file as a 'Game' file. Not used by SA1 DLCs.";
        }

		private void checkBoxCopyProtect_Click(object sender, EventArgs e)
		{
            toolStripStatusHint.Text = "Prevents the file from being copied. Not used by SA1 DLCs.";
        }

		private void buttonClose_Click(object sender, EventArgs e)
		{
            Close();
		}
	}
}