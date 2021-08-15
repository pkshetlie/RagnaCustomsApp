using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
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
            client.SendMessage(joinedChannel, $"You can request custom maps to " + joinedChannel.Channel + 
                " with the command !rc {map_id}, to find it go to https://ragnacustoms.com and click on the twitch button" +
                " (the purple one) to copy !rc {map_id} command and come back here to paste the command.");
            return true;
        }
    }

}