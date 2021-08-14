using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views.Commandes
{
    class NextCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "next" };
        }
        string ICommandes.help()
        {
            return "?";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            var song = me.songRequests.Rows[0].Cells["Song"].Value.ToString();
            if (song != null)
            {
                client.SendMessage(joinedChannel, $"Next song : {song} ");
            }
            else
            {
                client.SendMessage(joinedChannel, $"End of the queue");
            }
            return true;
        }
    }

}