using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace AirFoilDesign
{
    public partial class addDataPoint : Telerik.WinControls.UI.RadForm
    {
        private RadForm1 mainForm = null;
        public addDataPoint(Form callingForm)
        {
            mainForm = callingForm as RadForm1;
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            mainForm.return_dat_st = "Added manually";
            mainForm.return_dat = radTextBox1.Text;

            this.Close();
        }

        private void radTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (radTextBox1.TextLength != 0) radButton1.Enabled = true; else radButton1.Enabled = false;
        }
    }
}
