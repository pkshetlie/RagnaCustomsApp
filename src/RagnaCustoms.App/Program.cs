using RagnaCustoms.App.Properties;
using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;
using RagnaCustoms.Views;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RagnaCustoms.App
{
    static class Program
    {
        const string RagnacInstallCommand = "ragnac://install/";
        const string RagnacApiCommand = "ragnac://api/";

#if DEBUG 
        public const string UploadSessionUri = "https://127.0.0.1:8000/api/score/v2?XDEBUG_SESSION_START=PHPSTORM";
#else
        public const string UploadSessionUri = "https://ragnacustoms.com/api/score/v2";
#endif
#if DEBUG
        const string UploadOverlayUri = "https://127.0.0.1:8000/api/overlay/?XDEBUG_SESSION_START=PHPSTORM";
#else
        const string UploadOverlayUri = "https://ragnacustoms.com/api/overlay/";
#endif

        static readonly string RagnarockSongLogsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Ragnarock",
            "Saved",
            "Logs",
            "Ragnarock.log"
        );

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var songProvider = new SongProvider();
            var downloadingView = new DownloadingForm();
            var downloadingPresenter = new DownloadingPresenter(downloadingView, songProvider);
            var configuration = new Configuration();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(configuration.Lang??"en", true);
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
                        var songId = int.Parse(id);
                        downloadingView = new DownloadingForm();
                        downloadingPresenter = new DownloadingPresenter(downloadingView, songProvider);
                        downloadingPresenter.Download(songId, songsId.Count()>=2 ? configuration.AutoCloseDownload:true);
                        Application.Run(downloadingView);

                    }

                }
                else if (uri.StartsWith(RagnacApiCommand))
                {
                    var api = uri.Replace(RagnacApiCommand, string.Empty);
                    configuration.ApiKey = api;
                    MessageBox.Show(Resources.Program_Api_Set_api_key,"RagnaCustoms", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else 
            {
                // Starts background services
                var sessionUploader = new SessionUploader(configuration, UploadSessionUri);
                var songResultParser = new SessionParser(RagnarockSongLogsFilePath);

                songResultParser.OnNewSession += async (session) => await sessionUploader.UploadAsync(configuration.ApiKey, session);
                songResultParser.StartAsync();

                // Send score if Oculus is available
                Oculus.SendScore();

                var overlayUploader = new OverlayUploader(configuration, UploadOverlayUri);
                var songOverlayParser = new OverlayParser(RagnarockSongLogsFilePath);

                songOverlayParser.OnOverlayEndGame += async (session) => await overlayUploader.UploadAsync(configuration.ApiKey, session);
                songOverlayParser.OnOverlayNewGame += async (session) => await overlayUploader.UploadAsync(configuration.ApiKey, session);
                songOverlayParser.OnOverlayStartGame += async (session) => await overlayUploader.UploadAsync(configuration.ApiKey, session);
                songOverlayParser.StartAsync();

                // Create first view to display
                var songView = new SongForm();
                var songPresenter = new SongPresenter(configuration, songView, downloadingPresenter, songProvider);
                if (string.IsNullOrEmpty(configuration.ApiKey))
                {
                    MessageBox.Show(songView,Resources.Program_Api_Message1 , Resources.Program_Api_Message1_Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Application.Run(songView);
            }
        }
    }
}
