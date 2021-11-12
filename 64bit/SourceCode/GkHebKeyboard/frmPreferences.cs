using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GkHebKeyboard
{
    public partial class frmPreferences : Form
    {
        RegistryKey baseKey;

        public frmPreferences()
        {
            InitializeComponent();
        }

        public void initialisePreferences(RegistryKey inKey, classGlobal inControl)
        {
            Object regValue;
            float fontSize;

            baseKey = inKey;
            if (baseKey != null)
            {
                regValue = baseKey.GetValue("Opening Language");
                if (regValue != null)
                {
                    if (Convert.ToInt32(regValue.ToString()) == 2)
                    {
                        rbtnGreek.Checked = true;
                        rbtnHebrew.Checked = false;
                    }
                    else
                    {
                        rbtnGreek.Checked = false;
                        rbtnHebrew.Checked = true;
                    }
                }
                regValue = baseKey.GetValue("Starting Font Size");
                if (regValue == null) fontSize = 12F;
                else fontSize = (float)Convert.ToDecimal(regValue.ToString());
                cbFontSize.SelectedItem = fontSize.ToString();
                regValue = baseKey.GetValue("Save Location");
                if (regValue != null)
                {
                    txtDefaultSaveLocation.Text = regValue.ToString();
                }
            }
        }

        private void btnSaveLocation_Click(object sender, EventArgs e)
        {
            if (dlgFolder.ShowDialog() == DialogResult.OK)
            {
                txtDefaultSaveLocation.Text = dlgFolder.SelectedPath;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (baseKey == null)
            {
                baseKey = Registry.CurrentUser.CreateSubKey(@"software\LFCConsulting\GkHebKeyboard");
            }
            if (rbtnHebrew.Checked)
            {
                baseKey.SetValue("Opening Language", 1, RegistryValueKind.DWord);
            }
            else
            {
                baseKey.SetValue("Opening Language", 2, RegistryValueKind.DWord);
            }
            baseKey.SetValue("Starting Font Size", Convert.ToInt32(cbFontSize.SelectedItem), RegistryValueKind.DWord);
            baseKey.SetValue("Save Location", txtDefaultSaveLocation.Text, RegistryValueKind.String);
            this.Close();
        }
    }
}
