using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views.Commandes
{
    class HowToCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "how-to", "ht" };
        }
        string ICommandes.help()
        {
            return "affiche l'aide pour request une map";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            client.SendMessage(joinedChannel, "Go to https://ragnacustoms.com, click on the twitch button to copy !rc [songid] and paste it here");
            return true;
        }
    }

}