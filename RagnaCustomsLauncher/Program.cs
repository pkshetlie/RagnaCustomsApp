using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RagnaCustomsLauncher
{
    static class Program
    {
        public static string Version = "2.0.5";
        public static string newVersion = Version;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            newVersion = CheckNewVersion();
            if (newVersion != Version)
            {

                Application.Run(new Updater());
            }
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
