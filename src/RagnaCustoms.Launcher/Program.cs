using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RagnaCustoms.Launcher
{
    static class Program
    {
        const int UnexpectedCompareResult = -2;
        const int NoLocalNorOnlineVersionAvailable = -1;
        const int ApplicationStartedAndUpToDate = 0;
        const int ApplicationStartedAndUpdated = 1;
        const int ApplicationStartedAndCannotBeUpdated = 2;

        const string DirectoryName = "RagnaCustoms.App";
        const string AppName = "RagnaCustoms.exe";

        const string GitHubProductName = "RagnaCustoms.Launcher";
        const string GitHubAppOwner = "pkshetlie";
        const string GitHubRepoName = "RagnaCustomsApp";
        const string GitHubAppAssetName = "RagnaCustoms.zip";

        static readonly string CurrentAppPath = Assembly.GetEntryAssembly().Location;
        static readonly string CurrentDirectoryPath = Path.GetDirectoryName(CurrentAppPath);
        static readonly string AppDirectoryPath = Path.Combine(CurrentDirectoryPath, DirectoryName);
        static readonly string AppPath = Path.Combine(AppDirectoryPath, AppName);

        static Release LatestRelease { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task<int> Main()
        {
            var localVersion = GetLocalVersion();
            var onlineVersion = await GetOnlineVersion();

            // Case 1 : Local does not exists, online cannot be reached
            if (localVersion is null && onlineVersion is null)
            {
                MessageBox.Show("No local nor online version found, try again later.", "Something goes wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return NoLocalNorOnlineVersionAvailable;
            }

            // Case 2 : Local does not exists, online can be reached
            else if (localVersion is null && onlineVersion is not null)
            {
                return DownloadAndStartApplication();
            }

            // Case 3 : Local exists, online cannot be reached
            else if (localVersion is not null && onlineVersion is null)
            {
                StartApplication();

                return ApplicationStartedAndCannotBeUpdated;
            }

            else
            {
                var comparedVersion = localVersion.CompareTo(onlineVersion);

                return comparedVersion switch
                {
                    // Case 4 : Local exists, online can be reached and new version is available
                    -1 => AskToUpdate(),

                    // Case 5 : Local exists, online can be reached and is up to date
                    0 or 1 => StartApplication(),

                    // Case not implemented
                    _ => UnexpectedCompareResult
                };
            }
        }

        /// <summary>
        /// Get local version of the application.
        /// </summary>
        static Version GetLocalVersion()
        {
            if (!File.Exists(AppPath)) return default;

            var localVersion = FileVersionInfo.GetVersionInfo(AppPath);
            var localVersionStr = localVersion.FileVersion;
            if (localVersionStr is null) return default;

            return new Version(localVersionStr);
        }

        /// <summary>
        /// Get online version of the application.
        /// </summary>
        static async Task<Version> GetOnlineVersion()
        {
            var client = new GitHubClient(new ProductHeaderValue(GitHubProductName));
            
            LatestRelease = await client.Repository.Release.GetLatest(GitHubAppOwner, GitHubRepoName);
            if (LatestRelease is null) return default;
            
            var versionStr = LatestRelease.TagName.Trim('v');
            if (!Version.TryParse(versionStr, out Version version)) return default;

            return version;
        }

        /// <summary>
        /// Download online application and start it.
        /// </summary>
        static int DownloadAndStartApplication()
        {
            // TODO: Add a loading view
            //System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //System.Windows.Forms.Application.EnableVisualStyles();
            //System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            //System.Windows.Forms.Application.Run(new CheckUpdateForm());

            DownloadApplication();
            StartApplication();

            return ApplicationStartedAndUpdated;
        }

        /// <summary>
        /// Download online application.
        /// </summary>
        static void DownloadApplication()
        {
            var bundledAppAsset = LatestRelease.Assets.FirstOrDefault(asset => asset.Name == GitHubAppAssetName);
            if (bundledAppAsset is null) return;

            var downloadUrl = bundledAppAsset.BrowserDownloadUrl;
            var tempFilePath = Path.GetTempFileName();

            using var client = new WebClient();
            client.DownloadFile(downloadUrl, tempFilePath);

            ZipFile.ExtractToDirectory(tempFilePath, AppDirectoryPath);
            File.Delete(tempFilePath);
        }

        /// <summary>
        /// Start the local application.
        /// </summary>
        static int StartApplication()
        {
            Process.Start(AppPath);

            return ApplicationStartedAndUpToDate;
        }

        /// <summary>
        /// Ask the user if the application can be updated.
        /// </summary>
        static int AskToUpdate()
        {
            var result = MessageBox.Show("A brand new version of the application is available, would you like to update?", "That's important", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return result == DialogResult.Yes ?
                DownloadAndStartApplication() :
                StartApplication();
        }
    }
}
