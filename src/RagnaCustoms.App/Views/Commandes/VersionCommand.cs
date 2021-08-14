using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views.Commandes
{
    class VersionCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "version", "v" };
        }
        string ICommandes.help()
        {
            return "Affiche la version de l'application";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            client.SendMessage(joinedChannel, $"I'm version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            return true;
        }
    }

}