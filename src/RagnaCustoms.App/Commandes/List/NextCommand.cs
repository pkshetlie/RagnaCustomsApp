using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class NextCommand: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "next" };
        }
        string ICommandes.Help()
        {
            return "?";
        }
        public List<UserType> IllegalUsers()
        {
            return new List<UserType>();
        }
        bool ICommandes.Action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            var song = me.songRequests?.Rows[0]?.Cells["Song"]?.Value?.ToString() ?? null;
            if (song != null)
            {
                client.SendMessage(joinedChannel, $"Next song : {song} ");
            }
            else
            {
                client.SendMessage(joinedChannel, "End of the queue");
            }
            return true;
        }
    }

}