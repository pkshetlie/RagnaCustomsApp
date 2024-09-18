using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using RagnaCustoms.App.Views;
using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;
using RagnaCustoms.Views;
using Sentry;
using Configuration = RagnaCustoms.Services.Configuration;

namespace RagnaCustoms.App
{
    internal static class Program
    {
        private const string RagnacInstallCommand = "ragnac://install/";
        private const string RagnacListCommand = "ragnac://list/";
        private const string RagnacApiCommand = "ragnac://api/";


        private const string UploadOverlayUri = "https://api.ragnacustoms.com/api/overlay/";
        private const string ApiCheckUri = "https://api.ragnacustoms.com/api/check";

        public static readonly string RagnarockSongLogsDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Ragnarock",
            "Saved",
            "Logs"
        );

        public static readonly string RagnarockSongLogsFilePath = Path.Combine(RagnarockSongLogsDirectoryPath, "Ragnarock.log");

        //--install "ragnac://list/2209"

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            using (SentrySdk.Init(o =>
                   {
                       o.Dsn = "https://f785a97e3b964c5594639f005dc7973e@o139094.ingest.sentry.io/6137970";
                       // When configuring for the first time, to see what the SDK is doing:
                       o.Debug = true;
                       // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                       // We recommend adjusting this value in production.
                       o.TracesSampleRate = 1.0;
                   }))
            {
                var configuration = new Configuration();
               
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var customDirectory = DirProvider.getCustomDirectory(); //on force la creation du dossier.
                //var customBkpDirectory = new DirProvider().RagnarockSongBkpDirectory; //on force la creation du dossier.
                var songProvider = new SongProvider();
                var downloadingView = new DownloadingForm();
                var downloadingPresenter = new DownloadingPresenter(downloadingView, songProvider);
               
                
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(configuration.Lang ?? "en", true);
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
                if (args.Contains("--install"))
                {
                    var uri = args.ElementAtOrDefault(1);
                    if (uri.StartsWith(RagnacInstallCommand))
                    {
                        var songIdStr = uri.Replace(RagnacInstallCommand, string.Empty);

                        var songsId = songIdStr.Split('-');
                        foreach (var id in songsId)
                        {
                            var songId = id;
                            downloadingView = new DownloadingForm();
                            downloadingPresenter = new DownloadingPresenter(downloadingView, songProvider);
                            downloadingPresenter.Download(songId,
                                songsId.Count() < 2 || configuration.AutoCloseDownload);
                            Application.Run(downloadingView);
                        }
                    }
                    else if (uri.StartsWith(RagnacListCommand))
                    {
                        var songIdStr = uri.Replace(RagnacListCommand, string.Empty);


                        var songListId = int.Parse(songIdStr);
                        downloadingView = new DownloadingForm();
                        downloadingPresenter = new DownloadingPresenter(downloadingView, songProvider);
                        downloadingPresenter.DownloadList(songListId, configuration.AutoCloseDownload);
                        Application.Run(downloadingView);

                    }
                    else if (uri.StartsWith(RagnacApiCommand))
                    {
                        var api = uri.Replace(RagnacApiCommand, string.Empty);
                        configuration.ApiKey = api;
                        MessageBox.Show(Resources.Program_Api_Set_api_key, "RagnaCustoms", MessageBoxButtons.OK,
                           MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                
                    // Create first view to display
                    var songView = new SongForm();
                    var songPresenter = new SongPresenter(songView, downloadingPresenter, songProvider);
                    

                    //MessageBox.Show(songView, Resources.Program_Api_Message1, Resources.Program_Api_Message1_Title,
                    //        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (configuration.TwitchBotAutoStart) new TwitchBotForm().Show();
                    if (string.IsNullOrEmpty(configuration.ApiKey) || !checkApiKey(configuration.ApiKey)) new LoginForm(songView).Show();
                    Application.Run(songView);
                   

                }
            }
        }

        public static bool checkApiKey(string apiKey)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            var result = client.PostAsJsonAsync(ApiCheckUri, new { });

            return result.Result.IsSuccessStatusCode;
        }
    }
}