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
using System.Timers;

namespace CoreInstall
{
    public partial class Form1 : Form
    {
        string config = "";

        splash sph = new splash();

        System.Timers.Timer t = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
        }
        private void eulachecker()
        {
            Boolean running = true;
            do
            {
                if (vars.agreeeula == true)
                {
                    if (checkBox5.InvokeRequired)
                    {
                        checkBox5.BeginInvoke((MethodInvoker)delegate ()
                        {
                            this.checkBox5.Checked = true;
                        });
                    }else
                    {
                        this.checkBox5.Checked = true;
                    }
                    
                    running = false;
                }else
                {
                    System.Threading.Thread.Sleep(200);
                }
            } while (running == true);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (vars.agreeeula == true)
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
                        }
                        else
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
                        if (Boolean.Parse(SubstringExtensions.Between(config, "<automode>", @"</automode>")))
                        {
                            checkBox4.Checked = true;
                        }
                        else
                        {
                            checkBox4.Checked = false;
                            checkBox4.Enabled = false;
                        }

                        vars.libraryrepo = SubstringExtensions.Between(config, "<libraryrepo>", @"</libraryrepo>");
                        vars.mediarepo = SubstringExtensions.Between(config, "<mediarepo>", @"</mediarepo>");
                        vars.prefrepo = SubstringExtensions.Between(config, "<prefrepo>", @"</prefrepo>");

                        vars.dns = SubstringExtensions.Between(config, "<dns>", @"</dns>");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error connecting to the server.  Please check the URL and your connection and try again.  Technical details: " + Environment.NewLine + ex.Message.ToString());
                }
            }else
            {
                MessageBox.Show("Please agree to the EULA first.");
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


                    cmd.StartInfo.Arguments = (@"/C git config --global user.name """ + textBox4.Text + @"""");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C git config --global user.email " + textBox5.Text);
                    cmd.Start();
                    cmd.WaitForExit();

                    try
                    {
                        if (!(System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Semrau Software Consulting\ProPsync\")))
                        {
                            if (!(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Semrau Software Consulting\")))
                            {
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Semrau Software Consulting");
                            }
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Semrau Software Consulting\ProPsync\");
                        }

                        using (var client = new WebClient())
                        {
                            client.DownloadFile("https://downloads.semrauconsulting.com/propsync/ProPsync-CoreGUI.exe", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Semrau Software Consulting\ProPsync\ProPsync-CoreGUI.exe");
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error installing core GUI.  Please download it and install it in a location of your choice." + Environment.NewLine + Environment.NewLine + ex.Message.ToString());
                    }

                    try
                    {
                        string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        using (StreamWriter writer = new StreamWriter(deskDir + @"\ProPsync.url"))
                        {
                            string app = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Semrau Software Consulting\ProPsync\ProPsync-CoreGUI.exe";
                            writer.WriteLine("[InternetShortcut]");
                            writer.WriteLine("URL=file:///" + app);
                            writer.WriteLine("IconIndex=0");
                            string icon = app.Replace('\\', '/');
                            writer.WriteLine("IconFile=" + icon);
                            writer.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating shortcut on desktop.  Application can be executed from " + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Semrau Software Consulting\ProPsync\ProPsync-CoreGUI.exe" + @"." + Environment.NewLine + Environment.NewLine + ex.Message.ToString());
                    }

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
            vars.automode = checkBox4.Checked;
            vars.username = textBox2.Text;
            vars.fullname = textBox4.Text;
            vars.email = textBox5.Text;
            this.Opacity = 0;
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
            }else
            {
                vars.agreeeula = false;
                showsplash();
                
                t.Interval = 3000;
                t.AutoReset = true;
                t.Elapsed += new ElapsedEventHandler(closesplash);
                t.Start();

            }





                vars.ignoredpreffiles = @"CrashReports/
Log.txt
LogCloudSyncApp.txt";
        }


        private void showsplash()
        {
            this.Visible = false;
            this.Enabled = false;
            this.Opacity = 0;
            sph.Show();
            
            //System.Threading.Thread.Sleep(3000);
            //sph.Close();
        }
        private void closesplash(object sender, ElapsedEventArgs e)
        {
            sph.Close();
            this.Enabled = true;
            this.Visible = true;
            this.Opacity = 100;
            t.Stop();
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            eula ela = new eula();
            ela.Show();
            System.Threading.Thread trd = new System.Threading.Thread(eulachecker);
            trd.Start();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                vars.agreeeula = true;
            }else
            {
                vars.agreeeula = false;
            }
        }
    }
}
public static class vars
{
    public static string version { get; set; }
    public static Boolean syncmedia { get; set; }
    public static Boolean synclibrary { get; set; }
    public static Boolean syncpref { get; set; }
    public static Boolean automode { get; set; }

    public static string fullname { get; set; }
    public static string username { get; set; }
    public static string email { get; set; }
    public static string dns { get; set; }
    public static string mediarepo { get; set; }
    public static string libraryrepo { get; set; }
    public static string prefrepo { get; set; }
    public static string ignoredpreffiles { get; set; }
    public static Boolean agreeeula { get; set; }
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