using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Views.Commandes
{
    class HelpCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "help", "h" };
        }
        string ICommandes.help()
        {
            return "affiche la liste des commandes";
        }
        bool ICommandes.action(
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
                if (me.commandes.ContainsKey(searshedCmd))
                {
                    var commande = me.commandes[searshedCmd];
                    client.SendMessage(joinedChannel, $"Commande {searshedCmd} ---> {commande.help()}");
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
                foreach (KeyValuePair<string,ICommandes> cmd in me.commandes)
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