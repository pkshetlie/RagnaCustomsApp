using System.Collections.Generic;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using RagnaCustoms.App.Views;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    internal class BaseCommand : ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string> { "" };
        }

        string ICommandes.Help()
        {
            return Resources.Command_Base_Help;
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
            client.SendMessage(joinedChannel,
                string.Format(Resources.Command_Base_Action_Message, joinedChannel.Channel));
            return true;
        }
    }
}