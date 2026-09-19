using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using RagnaCustoms.App.Views;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    internal class VersionCommand : ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string> { "version", "v" };
        }

        string ICommandes.Help()
        {
            return TwitchBotForm.GetLocalizedText("Command.Version.Help", "Display the application version");
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
            client.SendMessage(joinedChannel, string.Format(TwitchBotForm.GetLocalizedText("Command.Version.Message", "I'm on version {0}"), Assembly.GetExecutingAssembly().GetName().Version));
            return true;
        }
    }
}
