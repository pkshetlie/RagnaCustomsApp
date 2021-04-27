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
        static async Task Main(string[] args)
        {
            if (args.Contains("--install"))
            {
                var songProvider = new SongProvider();
                var presenter = new SongPresenter(view: null, songProvider);
                var uri = args.ElementAtOrDefault(1);
                var songIdStr = uri.Replace(RagnacInstallCommand, string.Empty);
                var songId = int.Parse(songIdStr);

                await presenter.DownloadAsync(songId);
            }
            else
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var songProvider = new SongProvider();
                var view = new SongForm();
                var presenter = new SongPresenter(view, songProvider);

                Application.Run(view);
            }
        }
    }
}
