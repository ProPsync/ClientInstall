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
    public partial class eula : Form
    {
        public eula()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vars.agreeeula = true;
            
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void eula_Load(object sender, EventArgs e)
        {

        }
    }
}
