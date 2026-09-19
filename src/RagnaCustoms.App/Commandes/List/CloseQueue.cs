using System.Collections.Generic;
using System.Windows.Forms;
using RagnaCustoms.App.Views;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    internal class CloseQueue : ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string> { "close" };
        }

        string ICommandes.Help()
        {
            return TwitchBotForm.GetLocalizedText("Command.Close.Help", "Close the queue (moderator only)");
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
            if (me.QueueIsOpen)
            {
                me.QueueIsOpen = false;
                client.SendMessage(joinedChannel, TwitchBotForm.GetLocalizedText("Command.Close.Closed", "Queue is now closed"));
            }
            else
            {
                client.SendMessage(joinedChannel, TwitchBotForm.GetLocalizedText("Command.Close.AlreadyClosed", "Queue is already closed"));
            }

            return true;
        }
    }
}
