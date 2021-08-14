using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views
{
    public interface ICommandes
    {
        List<string> names();
        string help();
        bool action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
            );
    }
}