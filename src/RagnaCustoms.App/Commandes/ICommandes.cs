using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App
{
    public interface ICommandes
    {
        List<string> Names();
        string Help();
        List<UserType> IllegalUsers();
        bool Action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
            );
    }
}