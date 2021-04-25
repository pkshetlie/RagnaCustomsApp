using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace RagnaCustoms
{
    public partial class Form2 : Form
    {
        public Form2(string[] args)
        {
            InitializeComponent();
            this.Text = "RagnaCustoms.com";
            var assembly = Assembly.GetExecutingAssembly();
            if (args.Length > 0)
            {
                if (args[1].Contains("ragnac://install/"))
                {
                    InstallSong(args[1].Replace("ragnac://install/", ""));
                }
            }
        }

        private void InstallSong(string songId)
        {
            progressBar1.Value = 1;
            //debug_console.Items.Add("Récuperation des informations");
            Song stuff;
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString("https://ragnacustoms.com/api/song/" + songId);
                stuff = JsonConvert.DeserializeObject<Song>(json);
                //debug_console.Items.Add($"Début de la récuperation de {stuff.title}");
                progressBar1.Value = 2;
                var coverUrl = "https://ragnacustoms.com/covers/" + songId + stuff.CoverImageExtension;
                this.pictureBox1.Load(coverUrl);
                this.label1.Text = $"{stuff.Author} - {stuff.Name}";
                this.label2.Text = $"{Resources.strings.Difficulties} : {stuff.Difficulties}";
                this.label3.Text = $"{Resources.strings.Mapped_by} : {stuff.Mapper}";
            }
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Ragnarock/CustomSongs/";
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Ragnarock/"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Ragnarock/");
            }
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Ragnarock/CustomSongs/"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Ragnarock/CustomSongs/");
            }
            using (var webClient = new System.Net.WebClient())
            {
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                webClient.QueryString.Add("ZipPath", userProfile + songId + ".zip");
                webClient.QueryString.Add("SongTitle", RemoveSpecialCharacters(stuff.Name).Trim());
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

                webClient.DownloadFileAsync(new Uri("https://ragnacustoms.com/songs/download/" + songId), userProfile + songId + ".zip");
            }

        }




        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            var ratio = Math.Truncate((decimal)e.ProgressPercentage / 100 * 88);
            progressBar1.Value = 2 + (int)ratio;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Value = 90;

            //debug_console.Items.Add($"Fichier compressé récupéré");
            //debug_console.Items.Add($"Décompression en cours");
            var zipPath = ((System.Net.WebClient)(sender)).QueryString["ZipPath"];
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                var songFolder = ((System.Net.WebClient)(sender)).QueryString["SongTitle"];


                var userPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fullPath = Path.Combine(userPath, "Ragnarock/CustomSongs");

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    try
                    {
                        var name = entry.FullName;
                        if (!name.Contains("/autosaves/"))
                        {
                            var spl = name.Split('/');
                            if (spl.Length == 1)
                            {
                                if (!Directory.Exists(Path.Combine(fullPath, songFolder)))
                                {
                                    Directory.CreateDirectory(Path.Combine(fullPath, songFolder));
                                }
                                entry.ExtractToFile(Path.Combine(fullPath, songFolder, entry.FullName));
                            }
                            else
                            {
                                spl[0] = songFolder;
                                if (!Directory.Exists(Path.Combine(fullPath, spl[0])))
                                {
                                    Directory.CreateDirectory(Path.Combine(fullPath, spl[0]));
                                }
                                if (!String.IsNullOrEmpty(entry.Name))
                                {
                                    entry.ExtractToFile(Path.Combine(fullPath, String.Join("/", spl)), true);
                                }
                            }
                        }
                    }
                    catch (Exception o_O)
                    {

                    }
                }
            }
            File.Delete(zipPath);
            progressBar1.Value = 95;
            progressBar1.Value = 100;
            //Items.Add($"Décompression terminé");
            //debug_console.Items.Add($"A vos marteaux !");
            //dézip
            button1.Enabled = true;
            if (Program.Settings.AutoClose)
            {
                Application.Exit();
            }
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            if (Program.Settings.AutoClose)
            {
                button1.Visible = false;
            }
            else
            {
                button1.Enabled = false;
            }
            button1.Text = Resources.strings.Close;
        }

        private void debug_console_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.twitch.tv/rhokapa/about");
            Process.Start(sInfo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
