using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class HowToCommand: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "how-to", "ht" };
        }
        string ICommandes.Help()
        {
            return "affiche l'aide pour request une map";
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
            client.SendMessage(joinedChannel, "Go to https://ragnacustoms.com, click on the twitch button to copy !rc [songid] and paste it here");
            return true;
        }
    }

}