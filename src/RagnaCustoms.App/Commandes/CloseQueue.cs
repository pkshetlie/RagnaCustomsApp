using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class CloseQueue: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "close" };
        }
        string ICommandes.help()
        {
            return "Vous permet de clore la queue (moderateur uniquement)";
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
                client.SendMessage(joinedChannel, $"Sorry only moderator can do that");
            }
            if (me.QueueIsOpen)
            {
                me.QueueIsOpen = false;
                client.SendMessage(joinedChannel, $"Queue is now closed");
            }
            else
            {
                client.SendMessage(joinedChannel, $"Queue is already closes");
            }            
            return true;
        }
    }

}