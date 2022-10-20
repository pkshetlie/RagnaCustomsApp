using System.Collections.Generic;
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using RagnaCustoms.App.Views;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    internal class HelpCommand : ICommandes
    {
        List<string> ICommandes.Names()
        {
            return new List<string> { "help", "h" };
        }

        string ICommandes.Help()
        {
            return Resources.Command_Help_Help;
        }

        public List<UserType> IllegalUsers()
        {
            return new List<UserType>();
        }

        bool ICommandes.Action(
            JoinedChannel joinedChannel,
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
                    client.SendMessage(joinedChannel,
                        $"{Resources.Command_Help_Action_Cmd} {searshedCmd} ---> {commande.Help()}");
                }
                else
                {
                    client.SendMessage(joinedChannel, Resources.Command_Help_Action_UnknowCommand);
                }
            }
            else
            {
                var cmdList = new List<ICommandes>();
                var stringCommandList = new List<string>();
                var index = 0;
                stringCommandList.Add("");
                foreach (var cmd in me.Commandes)
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

                foreach (var s in stringCommandList)
                    client.SendMessage(joinedChannel, $"{Resources.Command_Help_Action_CommandList} ---> {s}");
                client.SendMessage(joinedChannel, Resources.Command_Help_Action_OthersCommand);
            }

            return true;
        }
    }
}