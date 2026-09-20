using System.Collections.Generic;
using RagnaCustoms.App.Views;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    internal class ClearQueueCommand : ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string> { "clearqueue", "emptyqueue" };
        }

        string ICommandes.Help()
        {
            return TwitchBotForm.GetLocalizedText("Command.ClearQueue.Help", "Clear the request queue (moderator only)");
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
            var cleared = me.ClearQueue();
            client.SendMessage(joinedChannel, cleared
                ? TwitchBotForm.GetLocalizedText("Command.ClearQueue.Cleared", "The request queue has been cleared.")
                : TwitchBotForm.GetLocalizedText("Command.ClearQueue.Empty", "The request queue is already empty."));
            return true;
        }
    }
}
