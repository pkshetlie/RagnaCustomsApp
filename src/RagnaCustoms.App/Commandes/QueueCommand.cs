using RagnaCustoms.App.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace RagnaCustoms.App.Commandes
{
    class QueueCommand: ICommandes
    {
        List<string> ICommandes.names()
        {
            return new List<string>() { "queue" };
        }
        string ICommandes.help()
        {
            return "affiche la queue";
        }
        bool ICommandes.action(
            JoinedChannel joinedChannel, 
            TextBox prefixe, 
            TwitchClient client, 
            TwitchBotForm me, 
            OnMessageReceivedArgs e
        )
        {
            var songs = new List<string>();
            var counter = 0;
            var concatStr = "";
            for (var i = 0; me.songRequests.Rows.Count - 1 > i; i++)
            {
                var songStr = me.songRequests.Rows[i].Cells["Song"].Value.ToString();
                if (concatStr.Length + songStr.Length > 500)
                {
                    songs.Add(concatStr);
                    concatStr = songStr;
                    counter = songStr.Length;
                }
                else
                {
                    if (!String.IsNullOrEmpty(concatStr))
                    {
                        concatStr += " // ";
                    }
                    concatStr += songStr;
                }
            }
            songs.Add(concatStr);

            client.SendMessage(joinedChannel, $"Next songs are: ");
            Thread.Sleep(400);
            foreach (var songMessage in songs)
            {
                client.SendMessage(joinedChannel, $"{songMessage}");
            }
            return true;
        }
    }

}