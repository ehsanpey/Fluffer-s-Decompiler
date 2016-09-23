#region Using
using CETRAINER_DECRYPT;
using System;
using System.Windows.Forms;
#endregion


namespace Fluffers_CE_Decompiler
{
    public partial class MainForm : Form
    {

        static CEDecompiler cheatEngine;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Agree Checkbox
        private void Agree_Checkbox_CheckedChanged(object sender)
        {
            if (Agree_Checkbox.Checked)
            {
                Decompile_Button.Enabled = true;
            }
            else
            {
                Decompile_Button.Enabled = false;
            }
        }
        #endregion

        #region Decompile Button
        private void Decompile_Button_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileLocation_Label.Text = openFileDialog.FileName;
                string filePath = openFileDialog.FileName;
                cheatEngine = new CEDecompiler();
                cheatEngine.DecryptTrainer(filePath);
            }

        }
        #endregion
    }
}
