using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.IO;
using Renci.SshNet;

namespace CoreInstall
{
    public partial class Form1 : Form
    {
        string config = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.StartsWith(@"http://"))
                {
                    textBox1.Text = textBox1.Text.Replace(@"http://", "");
                }
                if (textBox1.Text.StartsWith(@"https://"))
                {
                    textBox1.Text = textBox1.Text.Replace(@"https://", "");
                }
                if (textBox1.Text.StartsWith(@"ssh://"))
                {
                    textBox1.Text = textBox1.Text.Replace(@"ssh://", "");
                }
                if (textBox1.Text.StartsWith(@"www"))
                {
                    textBox1.Text = textBox1.Text.Replace(@"www", "");
                }
                WebRequest req = WebRequest.Create(@"http://" + textBox1.Text + @"/config.txt");
                WebResponse response = req.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                config = reader.ReadToEnd();
                if (!(config == ""))
                {
                    panel1.Visible = true;
                    button1.Enabled = false;
                    textBox1.Enabled = false;
                }
            }catch (Exception ex)
            {
                MessageBox.Show("There was an error connecting to the server.  Please check the URL and your connection and try again.  Technical details: " + Environment.NewLine + ex.Message.ToString());
            }
        }
    }
}
