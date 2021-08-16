using RagnaCustoms.App.Views;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class ShiftCommand: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "shift", "done" };
        }
        string ICommandes.Help()
        {
            return "remove the first song in the queue and display the next one";
        }
        public List<UserType> IllegalUsers()
        {
            return new List<UserType>() { UserType.Viewer };
        }
        bool ICommandes.Action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Viewer)
            {
                client.SendMessage(joinedChannel, $"Sorry only moderator can do that.");
            }
            else
            {
                try
                {
                    me.RemoveAtSongRequestInList(0);
                    client.SendMessage(joinedChannel, "Song removed");

                    var sog = me.songRequests?.Rows[0]?.Cells["Song"]?.Value?.ToString() ?? null;
                    if (sog != null)
                    {
                        client.SendMessage(joinedChannel, $"Next song : {sog} ");
                    }
                    else
                    {
                        client.SendMessage(joinedChannel, "End of the queue");
                    }
                }
                catch (Exception oO)
                {
                    client.SendMessage(joinedChannel, "No More song to remove");
                }
            }
            return true;
        }
    }

}