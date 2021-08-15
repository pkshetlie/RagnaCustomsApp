using RagnaCustoms.App.Views;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class CancelCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "cancel" };
        }
        string ICommandes.help()
        {
            return "retirer une de vos requests";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            for (var i = me.songRequests.Rows.Count - 2; 0 <= i; i--)
            {
                if (me.songRequests.Rows[i].Cells["Viewer"].Value.ToString() == e.ChatMessage.Username)
                {
                    var sng = me.songRequests.Rows[i].Cells["Song"].Value.ToString();
                    try
                    {
                        me.RemoveAtSongRequestInList(i);
                        client.SendMessage(joinedChannel, $"Song Cancelled: {sng} ");
                    }
                    catch (Exception o_O)
                    {
                        client.SendMessage(joinedChannel, $"No More song to remove");
                    }
                }
            }
            return true;
        }
    }

}