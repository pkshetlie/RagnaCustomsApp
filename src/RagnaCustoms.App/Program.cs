using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RagnaCustoms.App
{
    static class Program
    {
        const string RagnacInstallCommand = "ragnac://install/";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
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
                var songView = new SongForm();
                var songPresenter = new SongPresenter(songView, downloadingPresenter, songProvider);

                Application.Run(songView);
            }
        }
    }
}
