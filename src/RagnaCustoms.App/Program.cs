using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;
using RagnaCustoms.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RagnaCustoms.App
{
    static class Program
    {
        const string RagnacInstallCommand = "ragnac://install/";

        const string UploadSessionUri = "https://ragnacustoms.com/api/score/v2";

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

            if (args.Contains("--install"))
            {
                var uri = args.ElementAtOrDefault(1);
                var songIdStr = uri.Replace(RagnacInstallCommand, string.Empty);
                var songId = int.Parse(songIdStr);

                downloadingPresenter.Download(songId);

                Application.Run(downloadingView); 
            }
            else
            {
                // Starts background services
                var configuration = new Configuration();
                var sessionUploader = new SessionUploader(configuration, UploadSessionUri);
                var songResultParser = new SessionParser(RagnarockSongLogsFilePath);
                songResultParser.OnNewSession += async (session) => await sessionUploader.UploadAsync(configuration.ApiKey, session);
                songResultParser.StartAsync();

                // Create first view to display
                var songView = new SongForm();
                var songPresenter = new SongPresenter(configuration, songView, downloadingPresenter, songProvider);

                Application.Run(songView);
            }
        }
    }
}
