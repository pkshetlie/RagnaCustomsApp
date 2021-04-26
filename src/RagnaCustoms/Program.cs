using Newtonsoft.Json;
using RagnaCustoms.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace RagnaCustoms
{
    static class Program
    {
        public static AppSettings Settings;
        public static string Version = "2.0.4";
        public static string newVersion = Version;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Settings = new AppSettings();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Culture);


            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
               
#if !DEBUG
                if (!IsAdministrator())
                {
                    enableRunAs();
                }
                else
                {
#endif
                    newVersion = CheckNewVersion();
                    if (newVersion != Version)
                    {

                        Application.Run(new Updater());
                    }
                    Application.Run(new Main());
                //StartWebServer();
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
                    Application.Run(new Downloader(args));
                }
            }
        }


        //private static void StartWebServer()
        //{

        //    var httpListener = new HttpListener();
        //    var simpleServer = new SimpleServer(httpListener, "http://127.0.0.1:1234/score/", ProcessYourResponse);
        //    simpleServer.Start();
        //}
        //public static byte[] ProcessYourResponse(string test)
        //{
        //    Console.WriteLine(test);
        //    return new byte[0]; // TODO when you want return some response
        //}
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

        private static string CheckNewVersion()
        {
            var result = Version;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://ragnacustoms.com/application/version");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

    }
}
public static class ExtensionMethods
{
    public static String PregReplace(this String input, string pattern, string replacements)
    {

        input = Regex.Replace(input, pattern, replacements);
        return input;
    }
}