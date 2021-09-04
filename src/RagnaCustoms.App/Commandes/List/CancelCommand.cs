using RagnaCustoms.App.Views;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class CancelCommand: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "cancel" };
        }
        string ICommandes.Help()
        {
            return Resources.Command_Cancel_Help;
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
            for (var i = me.songRequests.Rows.Count - 2; 0 <= i; i--)
            {
                if (me.songRequests.Rows[i].Cells["Viewer"].Value.ToString() == e.ChatMessage.Username)
                {
                    var sng = me.songRequests.Rows[i].Cells["Song"].Value.ToString();
                    try
                    {
                        me.RemoveAtSongRequestInList(i);
                        client.SendMessage(joinedChannel, $"{Resources.Command_Cancel_Action_Canceled} {sng} ");
                    }
                    catch (Exception)
                    {
                        client.SendMessage(joinedChannel, Resources.Command_Cancel_Action_NoSongToCancel);
                    }
                }
            }
            return true;
        }
    }

}