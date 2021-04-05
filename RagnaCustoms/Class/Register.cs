using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagnaCustoms
{
    class Register
    {

        public static bool RegisteryKeyExists()
        {
            string Protocol = "ragnac";

            RegistryKey ProtocolKey = Registry.ClassesRoot.OpenSubKey(Protocol, true);
            return (ProtocolKey != null);
        }
        public static void AddRegistryKeys()
        {
            string Protocol = "ragnac";
            RegistryKey ProtocolKey = Registry.ClassesRoot.OpenSubKey(Protocol, true);
            if (ProtocolKey == null)
                ProtocolKey = Registry.ClassesRoot.CreateSubKey(Protocol, true);

            RegistryKey CommandKey = ProtocolKey.CreateSubKey(@"shell\open\command", true);
            if (CommandKey == null)
                CommandKey = Registry.ClassesRoot.CreateSubKey(@"shell\open\command", true);

            if (ProtocolKey.GetValue("OneClickRS-Provider", "").ToString() != "RagnaCustoms")
            {
                ProtocolKey.SetValue("URL Protocol", "", RegistryValueKind.String);
                ProtocolKey.SetValue("OneClickRS-Provider", "RagnaCustoms", RegistryValueKind.String);
                CommandKey.SetValue("", $"\"{Process.GetCurrentProcess().MainModule.FileName}\" \"--install\" \"%1\"");
            }
        }

        public static void RemoveRegistryKeys()
        {
            string Protocol = "ragnac";
            Registry.ClassesRoot.DeleteSubKeyTree(Protocol, true);
           
        }
    }
}
