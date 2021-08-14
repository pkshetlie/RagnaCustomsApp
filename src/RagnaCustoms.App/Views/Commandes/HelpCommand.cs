using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views.Commandes
{
    class HelpCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "help", "h" };
        }
        string ICommandes.help()
        {
            return "affiche la liste des commandes";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            client.SendMessage(joinedChannel, $"Help 1/2 : !rc help (this command), !rc {}song id} (download the map), !rc cancel (remove last song you request)");
            client.SendMessage(joinedChannel, $"Help 2/2 : !rc open (open queue), !rc close (close queue), !rc shift (remove first song in list), !rc queue (list of songs not played), !rc next (next song to play), !rc version (to know current version)");
            return true;
        }
    }

}