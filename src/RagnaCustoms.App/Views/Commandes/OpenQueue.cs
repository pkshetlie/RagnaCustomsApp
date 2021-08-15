using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views.Commandes
{
    class OpenQueue: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "open" };
        }
        string ICommandes.help()
        {
            return "Vous permet d'ouvrir la queue (moderateur uniquement)";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Viewer)
            {
                client.SendMessage(joinedChannel, "Sorry only moderator can do that");
            }
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