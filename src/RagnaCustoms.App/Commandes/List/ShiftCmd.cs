using RagnaCustoms.App.Views;
using RagnaCustoms.Models;
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
                if (me.songRequests.Rows.Count <= 1)
                {
                    client.SendMessage(joinedChannel, "No More song to remove");
                    return true;
                }
                try
                {
                    me.RemoveLastPlayerSong();
                    client.SendMessage(joinedChannel, "Song removed");
                    var sog = me.songRequests?.Rows[0]?.Cells["Song"]?.Value?.ToString() ?? null;
                    client.SendMessage(joinedChannel, sog != null ? $"Next song : {sog} " : "End of the queue");
                }
                catch(Exception O_o)
                {
                    client.SendMessage(joinedChannel, $"I can't!");
                }
            }
            return true;
        }
    }

}