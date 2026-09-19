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
            return TwitchBotForm.GetLocalizedText("Command.Open.Help", "Open the queue (moderator only)");
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
                client.SendMessage(joinedChannel, TwitchBotForm.GetLocalizedText("Command.Open.Opened", "Queue is now open"));
            }
            else
            {
                client.SendMessage(joinedChannel, TwitchBotForm.GetLocalizedText("Command.Open.AlreadyOpen", "Queue is already open"));
            }

            return true;
        }
    }
}
