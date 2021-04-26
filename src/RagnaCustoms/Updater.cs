using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace RagnaCustoms
{
    public partial class Updater : Form
    {
        public Updater()
        {
            InitializeComponent();
            newVersion.Text = Program.newVersion;
            currentVersion.Text = Program.Version;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://ragnacustoms.com/application");
            Process.Start(sInfo);
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://ragnacustoms.com/application/changelog");
            Process.Start(sInfo);
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            InstallNewVersion();
        }

        private void InstallNewVersion()
        {
            
            using (var webClient = new System.Net.WebClient())
            {
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                webClient.QueryString.Add("ZipPath",Application.StartupPath + "/RagnaCustoms.zip");
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

                webClient.DownloadFileAsync(new Uri("https://ragnacustoms.com/apps/RagnaCustoms.zip"), Application.StartupPath + "/RagnaCustoms.zip");
            }

        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {          
            var zipPath = ((System.Net.WebClient)(sender)).QueryString["ZipPath"];
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                string fullPath = Path.Combine(Application.StartupPath);

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
#if !DEBUG
                    try
                    {     
#endif
                                entry.ExtractToFile(Path.Combine(fullPath, entry.FullName.Replace("RagnaCustoms/","")));
#if !DEBUG
          }
                    catch (Exception o_O)
                    {
                                    MessageBox.Show(o_O.ToString(), "RagnaCustoms.com", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
   
                    var x = 12;
                    }
#endif
                }
            }
            File.Delete(zipPath);       
            Application.Exit();
        }

        private void Updater_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
