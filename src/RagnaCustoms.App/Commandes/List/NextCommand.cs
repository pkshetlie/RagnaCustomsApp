using System.Collections.Generic;
using System.Windows.Forms;
using RagnaCustoms.App.Views;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    internal class NextCommand : ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string> { "next" };
        }

        string ICommandes.Help()
        {
            return TwitchBotForm.GetLocalizedText("Command.Next.Help", "Display the next song to play in chat");
        }

        public List<UserType> IllegalUsers()
        {
            return new List<UserType>();
        }

        bool ICommandes.Action(
            JoinedChannel joinedChannel,
            TwitchClient client,
            TwitchBotForm me,
            OnMessageReceivedArgs e
        )
        {
            var song = me.songRequests?.Rows[0]?.Cells["Song"]?.Value?.ToString() ?? null;
            if (song != null)
                client.SendMessage(joinedChannel, string.Format(TwitchBotForm.GetLocalizedText("Command.Next.Song", "Next song: {0}"), song));
            else
                client.SendMessage(joinedChannel, TwitchBotForm.GetLocalizedText("Command.Next.End", "End of the queue"));
            return true;
        }
    }
}
