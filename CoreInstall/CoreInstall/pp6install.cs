using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CoreInstall
{
    public partial class pp6install : Form
    {
        public pp6install()
        {
            InitializeComponent();
        }

        private void pp6install_Load(object sender, EventArgs e)
        {
            if (vars.synclibrary == false)
            {
                library.Visible = false;
            } else
            {
                if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\ProPresenter6"))
                {
                    textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\ProPresenter6";
                }
            }
            if (vars.syncmedia == false)
            {
                media.Visible = false;
            }else
            {
                if (System.IO.Directory.Exists(@"C:\ProgramData\Renewed Vision Media"))
                {
                    textBox2.Text = @"C:\ProgramData\Renewed Vision Media";
                }
            }
            if (vars.syncpref == false)
            {
                prefs.Visible = false;
            }else
            {
                if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RenewedVision\ProPresenter6"))
                {
                    textBox3.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RenewedVision\ProPresenter6";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog3.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox3.Text = folderBrowserDialog3.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!(System.IO.Directory.Exists(@"C:\ProgramData\Semrau Software Consulting\ProPsync\")))
            {
                System.IO.Directory.CreateDirectory(@"C:\ProgramData\Semrau Software Consulting\ProPsync\");
            }

            
            string backuplocation;
            backuplocation = @"C:\ProgramData\Semrau Software Consulting\ProPsync\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "---" + DateTime.Now.Hour + "-" + DateTime.Now.Minute;
            do
            {
                backuplocation = @"C:\ProgramData\Semrau Software Consulting\ProPsync\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "---" + DateTime.Now.Hour + "-" + DateTime.Now.Minute;
            } while (System.IO.Directory.Exists(backuplocation));

            

            if (vars.synclibrary == true)
            {
                if ((!(textBox1.Text == "")) && (System.IO.Directory.Exists(textBox1.Text))) {
                    DirectoryInfo dirinfo = new DirectoryInfo(textBox1.Text);
                    DirectoryCopy(textBox1.Text, backuplocation + dirinfo.Name, true);

                    System.IO.Directory.Delete(textBox1.Text, true);

                    Directory.CreateDirectory(textBox1.Text);

                    Process cmd = new Process();
                    cmd.StartInfo.FileName = Environment.SystemDirectory + @"\cmd.exe";
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox1.Text + " & git init");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox1.Text + " & git remote add origin ssh://" + vars.username + @"@" + vars.dns + vars.libraryrepo);
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox1.Text + " & git pull origin master");
                    cmd.Start();
                    cmd.WaitForExit();
                    DirectoryCopy(backuplocation + dirinfo.Name, textBox1.Text, true);
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox1.Text + " & git add --all");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox1.Text + @" & git commit -m ""Initial library commit for " + vars.fullname + @" on " + Environment.MachineName + @"""");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox1.Text + @" & git push origin master");
                    cmd.Start();
                    cmd.WaitForExit();
                }
            }
            if (vars.syncmedia == true)
            {
                if ((!(textBox2.Text == "")) && (System.IO.Directory.Exists(textBox2.Text)))
                {
                        DirectoryInfo dirinfo = new DirectoryInfo(textBox2.Text);
                        DirectoryCopy(textBox2.Text, backuplocation + dirinfo.Name, true);

                        System.IO.Directory.Delete(textBox2.Text, true);

                        Directory.CreateDirectory(textBox2.Text);

                        Process cmd = new Process();
                        cmd.StartInfo.FileName = Environment.SystemDirectory + @"\cmd.exe";
                        cmd.StartInfo.Arguments = (@"/C cd /d " + textBox2.Text + " & git init");
                        cmd.Start();
                        cmd.WaitForExit();
                        cmd.StartInfo.Arguments = (@"/C cd /d " + textBox2.Text + " & git remote add origin ssh://" + vars.username + @"@" + vars.dns + vars.mediarepo);
                        cmd.Start();
                        cmd.WaitForExit();
                        cmd.StartInfo.Arguments = (@"/C cd /d " + textBox2.Text + " & git pull origin master");
                        cmd.Start();
                        cmd.WaitForExit();
                        DirectoryCopy(backuplocation + dirinfo.Name, textBox2.Text, true);
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox2.Text + " & git add --all");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox2.Text + @" & git commit -m ""Initial media commit for " + vars.fullname + @" on " + Environment.MachineName + @"""");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox2.Text + @" & git push origin master");
                    cmd.Start();
                    cmd.WaitForExit();
                }
            }
            if (vars.syncpref == true)
            {
                if ((!(textBox3.Text == "")) && (System.IO.Directory.Exists(textBox3.Text)))
                {
                    DirectoryInfo dirinfo = new DirectoryInfo(textBox3.Text);
                    DirectoryCopy(textBox3.Text, backuplocation + dirinfo.Name, true);

                    System.IO.Directory.Delete(textBox3.Text, true);

                    Directory.CreateDirectory(textBox3.Text);

                    Process cmd = new Process();
                    cmd.StartInfo.FileName = Environment.SystemDirectory + @"\cmd.exe";
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox3.Text + " & git init");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox3.Text + " & git remote add origin ssh://" + vars.username + @"@" + vars.dns + vars.prefrepo);
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox3.Text + " & git pull origin master");
                    cmd.Start();
                    cmd.WaitForExit();
                    DirectoryCopy(backuplocation + dirinfo.Name, textBox3.Text, true);
                    System.IO.File.WriteAllText(textBox3.Text + @"\.gitignore", vars.ignoredpreffiles);
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox3.Text + " & git add --all");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox3.Text + @" & git commit -m ""Initial preference commit for " + vars.fullname + @" on " + Environment.MachineName + @"""");
                    cmd.Start();
                    cmd.WaitForExit();
                    cmd.StartInfo.Arguments = (@"/C cd /d " + textBox3.Text + @" & git push origin master");
                    cmd.Start();
                    cmd.WaitForExit();
                }
            }

            Microsoft.Win32.RegistryKey key;
            try
            {
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Semrau Software Consulting");
            }catch (Exception ex)
            {
                Console.Write("Error: couldn't create reg key: " + ex.Message.ToString());
            }
            try
            {
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("Semrau Software Consulting", true).CreateSubKey("ProPsync");
            }
            catch (Exception ex)
            {
                Console.Write("Error: couldn't create reg key: " + ex.Message.ToString());
            }

            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProPsync", true);
            key.SetValue("dns", vars.dns);
            key.SetValue("mediarepo", vars.mediarepo);
            key.SetValue("libraryrepo", vars.libraryrepo);
            key.SetValue("prefrepo", vars.prefrepo);
            key.SetValue("syncmedia", vars.syncmedia);
            key.SetValue("syncpref", vars.syncpref);
            key.SetValue("pro-ver", "6");
            key.SetValue("username", vars.username);
            key.SetValue("libpath", textBox1.Text);
            key.SetValue("mediapath", textBox2.Text);
            key.SetValue("prefpath", textBox3.Text);

            key.Close();

            MessageBox.Show("Completed!");
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (!(File.Exists(destDirName + @"\" + file.Name)))
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, false);
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
