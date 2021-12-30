using RagnaCustoms.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RagnaCustoms.App.Extensions;

namespace RagnaCustoms.Models
{
    public class EasyStreamRequest
    {
        public static void EnableEasyStreamRequest(Configuration configuration)
        {
            configuration.EasyStreamRequest = true;
        }
        
        public static void DisableEasyStreamRequest(bool state, Configuration configuration) // true pour gardé le système actif et ne supprimé que les dossiers
        {
            configuration.EasyStreamRequest = state;
            RemoveBackupDirectory();
        }

        public static void CreateBackupDirectory()
        {
            DirProvider.getCustomDirectory().MoveTo(DirProvider.RagnarockBackupSongDirectoryPath);
        }
        public static void RemoveBackupDirectory()
        {
            DirProvider.getCustomDirectory().Delete(true);
            DirProvider.getCustomBackupDirectory().MoveTo(DirProvider.RagnarockSongDirectoryPath);
        }
    }
}
