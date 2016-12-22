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

                    vars.libraryrepo = SubstringExtensions.Between(config, "<libraryrepo>", @"</libraryrepo>");
                    vars.mediarepo = SubstringExtensions.Between(config, "<mediarepo>", @"</mediarepo>");
                    vars.prefrepo = SubstringExtensions.Between(config, "<prefrepo>", @"</prefrepo>");

                    vars.dns = SubstringExtensions.Between(config, "<dns>", @"</dns>");

                }
            }catch (Exception ex)
            {
                MessageBox.Show("There was an error connecting to the server.  Please check the URL and your connection and try again.  Technical details: " + Environment.NewLine + ex.Message.ToString());
            }

           
        }



        private void doinstall()
        {
            //Check for and install git here
            if ((Directory.Exists(@"C:\Program Files\Git")) || (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Git")))
            {
                try
                {
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = Environment.SystemDirectory + @"\cmd.exe";
                    cmd.StartInfo.Arguments = (@"/C mkdir %userprofile%\.ssh");
                    cmd.Start();
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
                    try
                    {
                        using (var client = new SshClient(vars.dns, textBox2.Text, textBox3.Text))
                        {
                            client.Connect();
                            client.RunCommand("mkdir ~/.ssh");
                            client.RunCommand("chmod -R 700 ~/.ssh");
                            foreach (var myString in pubkey.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                client.RunCommand("echo '" + myString + @"' >> ~/.ssh/authorized_keys");
                            }
                            client.Disconnect();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to server: " + ex.Message.ToString());
                        Application.Exit();
                    }


                    cmd.StartInfo.Arguments = (@"/C git config --global user.name """"" + textBox4.Text + @"""");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C git config --global user.email " + textBox5.Text);
                    cmd.Start();
                    cmd.WaitForExit();

                    if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Renewed Vision\ProPresenter 6"))
                    {
                        dopp6install();
                    }
                    else
                    {
                        if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Renewed Vision\ProPresenter 5"))
                        {
                            dopp5install();
                        }
                        else
                        {
                            vars.version = "";
                            versionchooser vc = new versionchooser();
                            vc.Show();
                            do
                            {
                                this.Enabled = false;
                                System.Threading.Thread.Sleep(1000);
                                Application.DoEvents();
                            } while (vars.version == "");
                            this.Enabled = true;
                            if (vars.version == "6")
                            {
                                dopp6install();
                            }
                            else
                            {
                                if (vars.version == "5")
                                {
                                    dopp5install();
                                }
                                else
                                {
                                    MessageBox.Show("There seems to be a pretty big problem determining the version.  Please try installation again.");
                                    Application.Exit();
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to " + vars.dns + ": " + ex.Message.ToString());
                }
            }
            else
            {
                installgit();
            }
            
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doinstall();
        }

        private void dopp6install()
        {
            vars.synclibrary = checkBox1.Checked;
            vars.syncmedia = checkBox2.Checked;
            vars.syncpref = checkBox3.Checked;
            vars.username = textBox2.Text;
            vars.fullname = textBox4.Text;
            vars.email = textBox5.Text;
            this.Visible = false;
            pp6install pp6i = new pp6install();
            pp6i.Show();
        }
        private void dopp5install()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!((Directory.Exists(@"C:\Program Files\Git")) || (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Git"))))
            {
                installgit();
            }





                vars.ignoredpreffiles = @"CrashReports/
Log.txt
LogCloudSyncApp.txt";
        }

        private void installgit()
        {
            MessageBox.Show("Git will now be downloaded and the installer will be launched.  It might take just a few minutes for the download.  Once it is installed, please restart this setup.  Please check our documentation as to how you should install Git... we are heavily dependant on it being installed in a certain way - especially surrounding the extra Unix tools that it can install and what shell it executes from.");
            if (Environment.Is64BitOperatingSystem)
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://github.com/git-for-windows/git/releases/download/v2.11.0.windows.1/Git-2.11.0-64-bit.exe", Environment.CurrentDirectory + @"\GitSetup.exe");
                    Process proc = new Process();
                    proc.StartInfo.FileName = Environment.CurrentDirectory + @"\GitSetup.exe";
                    proc.Start();
                }
            }
            else
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://github.com/git-for-windows/git/releases/download/v2.11.0.windows.1/Git-2.11.0-32-bit.exe", Environment.CurrentDirectory + @"\GitSetup.exe");
                    Process proc = new Process();
                    proc.StartInfo.FileName = Environment.CurrentDirectory + @"\GitSetup.exe";
                    proc.Start();
                }
            }
            Application.Exit();
        }
    }
}
public static class vars
{
    public static string version { get; set; }
    public static Boolean syncmedia { get; set; }
    public static Boolean synclibrary { get; set; }
    public static Boolean syncpref { get; set; }

    public static string fullname { get; set; }
    public static string username { get; set; }
    public static string email { get; set; }
    public static string dns { get; set; }
    public static string mediarepo { get; set; }
    public static string libraryrepo { get; set; }
    public static string prefrepo { get; set; }
    public static string ignoredpreffiles { get; set; }
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