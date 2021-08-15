using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class OpenQueue: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "open" };
        }
        string ICommandes.Help()
        {
            return "Vous permet d'ouvrir la queue (moderateur uniquement)";
        }
        public List<UserType> IllegalUsers()
        {
            return new List<UserType>() { UserType.Viewer };
        }
        bool ICommandes.Action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            if (!me.QueueIsOpen)
            {
                me.QueueIsOpen = true;
                client.SendMessage(joinedChannel, "Queue is now open");
            }
            else
            {
                client.SendMessage(joinedChannel, "Queue is already open");
            }            
            return true;
        }
    }

}