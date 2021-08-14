using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views.Commandes
{
    class BaseCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "" };
        }
        string ICommandes.help()
        {
            return "Affiche aux viewers comment proposer des maps au streameur";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            client.SendMessage(joinedChannel, $"You can propose custom maps to " + joinedChannel.Channel + " thanks to the command !rc {map_id} simply by clicking on the corresponding button on the site https://ragnacustoms.com/");
            return true;
        }
    }

}