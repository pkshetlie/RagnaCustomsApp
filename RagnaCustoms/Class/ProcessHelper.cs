using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RagnaCustoms.Class
{
    
        public class ProcessHelper
        {
            public static void SetFocusToExternalApp(string strProcessName)
            {
                Process[] arrProcesses = Process.GetProcessesByName(strProcessName);
                if (arrProcesses.Length > 0)
                {

                    IntPtr ipHwnd = arrProcesses[0].MainWindowHandle;
                    Thread.Sleep(100);
                    SetForegroundWindow(ipHwnd);

                }
            }

            //API-declaration
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

        
    }
}
