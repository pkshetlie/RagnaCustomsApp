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
    class CloseQueue: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "close" };
        }
        string ICommandes.Help()
        {
            return "Vous permet de clore la queue (moderateur uniquement)";
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
            if (me.QueueIsOpen)
            {
                me.QueueIsOpen = false;
                client.SendMessage(joinedChannel, "Queue is now closed");
            }
            else
            {
                client.SendMessage(joinedChannel, "Queue is already closes");
            }            
            return true;
        }
    }

}