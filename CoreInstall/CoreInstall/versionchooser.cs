using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreInstall
{
    public partial class versionchooser : Form
    {
        public versionchooser()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                vars.version = "6";
                this.Close();
            }else
            {
                if (radioButton2.Checked == true)
                {
                    vars.version = "5";
                    this.Close();
                }else
                {
                    MessageBox.Show("Please select a version before attempting to continue.");
                }
            }
        }
    }
}
