using Newtonsoft.Json;
using RagnaCustoms.Class;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace RagnaCustoms
{
    static class Program
    {
        public static AppSettings Settings;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //if (!File.Exists("Config.json"))
            //{
                Settings = new AppSettings();
            //    Settings.Save();
            //}
            //var json = File.ReadAllText("Config.json");
            //Settings = JsonConvert.DeserializeObject<AppSettings>(json);



            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Culture);

            if (args.Length == 0)
            {

#if !DEBUG
                if (!IsAdministrator())
                {
                    enableRunAs();
                }
                else
                {   
#endif
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
#if !DEBUG
            }
#endif
            }
            else
            {
                if (args[0] == "--install")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form2(args));
                }
            }
        }

        public static void enableRunAs()
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            //proc.Arguments = string.Join(" ", args);

            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = Application.ExecutablePath;
            proc.Verb = "runas";
            try
            {
                Process.Start(proc);
            }
            catch
            {
                // The user refused the elevation.
                // Do nothing and return directly ...
                return;
            }
            finally
            {
                Application.Exit();
            }
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
