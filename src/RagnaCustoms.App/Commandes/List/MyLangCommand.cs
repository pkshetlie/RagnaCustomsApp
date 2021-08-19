using System;
using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class MyLangCommand: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "my-lang" };
        }
        string ICommandes.Help()
        {
            return "définir la langue dans laquel le bot vous répond";
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
            var args = e.ChatMessage.Message.Split(' ');
            if (args.Length < 3) return false;
            var lang = args[2];
            if (lang.Length > 10) return false;
            
            me._configuration.ViewerLang.Add(e.ChatMessage.UserId, lang);
            if (me._configuration.ViewerLang.ContainsKey(e.ChatMessage.UserId))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(me._configuration.ViewerLang[e.ChatMessage.UserId], true);
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
            client.SendMessage(joinedChannel, "Votre langue a bien été changé, si ce message n'est pas afficher dans la bonne langue, c'est que votre langue n'est pas encore disponible.");
            return true;
        }
    }

}