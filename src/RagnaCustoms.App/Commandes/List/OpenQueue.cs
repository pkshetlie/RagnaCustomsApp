using System.Collections.Generic;
using System.Windows.Forms;
using RagnaCustoms.App.Views;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    internal class OpenQueue : ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string> { "open" };
        }

        string ICommandes.Help()
        {
            return "open the queue (moderator only)";
        }

        public List<UserType> IllegalUsers()
        {
            return new List<UserType> { UserType.Viewer };
        }

        bool ICommandes.Action(
            JoinedChannel joinedChannel,
            TwitchClient client,
            TwitchBotForm me,
            OnMessageReceivedArgs e
        )
        {
            if (!me.QueueIsOpen)
            {
                me.QueueIsOpen = true;
                client.SendMessage(joinedChannel, "Queue is now open");
            }
            else
            {
                client.SendMessage(joinedChannel, "Queue is already open");
            }

            return true;
        }
    }
}