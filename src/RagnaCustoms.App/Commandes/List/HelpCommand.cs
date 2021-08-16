using System;
using RagnaCustoms.App.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class HelpCommand: ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string>() { "help", "h" };
        }
        string ICommandes.Help()
        {
            return "display list of commands";
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
            if (args.Length >= 3)
            {
                var searshedCmd = args[2];
                if (me.Commandes.ContainsKey(searshedCmd))
                {
                    var commande = me.Commandes[searshedCmd];
                    client.SendMessage(joinedChannel, $"Commande {searshedCmd} ---> {commande.Help()}");
                }
                else
                {
                    client.SendMessage(joinedChannel, "Cette commande n'existe pas, vérifiez le nom de la commande.");
                }
            }
            else
            {
                var cmdList = new List<ICommandes>();
                var stringCommandList = new List<String>();
                var index = 0;
                stringCommandList.Add("");
                foreach (KeyValuePair<string,ICommandes> cmd in me.Commandes)
                {
                    if (!cmdList.Contains(cmd.Value))
                    {
                        cmdList.Add(cmd.Value);
                        if (stringCommandList[index].Length <= 450)
                        {
                            if (stringCommandList[index].Length == 0) stringCommandList[index] = $"!rc {cmd.Key}";
                            else stringCommandList[index] = $"{stringCommandList[index]}, !rc {cmd.Key}";
                        }
                        else
                        {
                            index++;
                            stringCommandList.Add("");
                            stringCommandList[index] = $"!rc {cmd.Key}";
                        }
                    }
                }
                foreach (string s in stringCommandList)
                {
                    client.SendMessage(joinedChannel, $"Liste des commandes ---> {s}");
                }
                client.SendMessage(joinedChannel, "Pour voir la description complete d'une commande utiliser \"!rc help [nom_de_la_commande]\"");
            }
            return true;
        }
    }

}