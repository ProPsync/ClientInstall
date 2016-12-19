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
using System.Diagnostics;

namespace CoreInstall
{
    public partial class Form1 : Form
    {
        string config = "";

        string dns = "";
        string mediarepo = "";
        string libraryrepo = "";
        string prefrepo = "";

        Boolean syncmedia = true;
        Boolean synclibrary = true;
        Boolean syncpref = true;
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

                    if (Boolean.Parse(SubstringExtensions.Between(config, "<synclibrary>", @"</synclibrary>")))
                    {
                        checkBox1.Checked = true;
                    } else
                    {
                        checkBox1.Checked = false;
                        checkBox1.Enabled = false;
                    }
                    if (Boolean.Parse(SubstringExtensions.Between(config, "<syncmedia>", @"</syncmedia>")))
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                        checkBox2.Enabled = false;
                    }
                    if (Boolean.Parse(SubstringExtensions.Between(config, "<syncpref>", @"</syncpref>")))
                    {
                        checkBox3.Checked = true;
                    }
                    else
                    {
                        checkBox3.Checked = false;
                        checkBox3.Enabled = false;
                    }

                    libraryrepo = SubstringExtensions.Between(config, "<libraryrepo>", @"</libraryrepo>");
                    mediarepo = SubstringExtensions.Between(config, "<mediarepo>", @"</mediarepo>");
                    prefrepo = SubstringExtensions.Between(config, "<prefrepo>", @"</prefrepo>");

                    dns = SubstringExtensions.Between(config, "<dns>", @"</dns>");

                }
            }catch (Exception ex)
            {
                MessageBox.Show("There was an error connecting to the server.  Please check the URL and your connection and try again.  Technical details: " + Environment.NewLine + ex.Message.ToString());
            }

           
        }

        private void doinstall()
        {
            //Check for and install git here

            try
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = Environment.SystemDirectory + @"\cmd.exe";
                cmd.StartInfo.Arguments = (@"/C ssh-keygen -t rsa -b 4096 -C """ + textBox5.Text + @""" -f %userprofile%\.ssh\id_rsa -q -N """"");
                cmd.Start();
                cmd.WaitForExit();
                cmd.StartInfo.Arguments = (@"/C eval $(ssh-agent -s)");
                cmd.Start();
                cmd.WaitForExit();
                cmd.StartInfo.Arguments = (@"/C ssh-add %userprofile%\.ssh\id_rsa");
                cmd.Start();
                cmd.WaitForExit();
                cmd.StartInfo.Arguments = (@"/C cat %userprofile%\.ssh\id_rsa.pub");
                cmd.StartInfo.UseShellExecute = false;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.Start();
                cmd.WaitForExit();
                string pubkey = cmd.StandardOutput.ReadToEnd();
                using (var client = new SshClient(dns, textBox2.Text, textBox3.Text))
                {
                    client.Connect();
                    client.RunCommand("mkdir ~/.ssh");
                    client.RunCommand("chmod -R 600 ~/.ssh");
                    foreach (var myString in pubkey.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        client.RunCommand("printf '" + myString + @"'""""\n"""">>~/.ssh/authorized_keys");
                    }
                    client.Disconnect();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to " + dns + ": " + ex.Message.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doinstall();
        }
    }
}
static class SubstringExtensions
{
    /// <summary>
    /// Get string value between [first] a and [last] b.
    /// </summary>
    public static string Between(this string value, string a, string b)
    {
        int posA = value.IndexOf(a);
        int posB = value.LastIndexOf(b);
        if (posA == -1)
        {
            return "";
        }
        if (posB == -1)
        {
            return "";
        }
        int adjustedPosA = posA + a.Length;
        if (adjustedPosA >= posB)
        {
            return "";
        }
        return value.Substring(adjustedPosA, posB - adjustedPosA);
    }

    /// <summary>
    /// Get string value after [first] a.
    /// </summary>
    public static string Before(this string value, string a)
    {
        int posA = value.IndexOf(a);
        if (posA == -1)
        {
            return "";
        }
        return value.Substring(0, posA);
    }

    /// <summary>
    /// Get string value after [last] a.
    /// </summary>
    public static string After(this string value, string a)
    {
        int posA = value.LastIndexOf(a);
        if (posA == -1)
        {
            return "";
        }
        int adjustedPosA = posA + a.Length;
        if (adjustedPosA >= value.Length)
        {
            return "";
        }
        return value.Substring(adjustedPosA);
    }
}