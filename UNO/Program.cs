﻿using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Model;

namespace UNO
{
    class Program
    {
        const int HttpPort = 1337;
        const int WebSocketPort = 666;
        static List<ISpieler> AllSpieler = new List<ISpieler>();
        static Spielfeld DasSpielfeld;
        static Lobby MeineLobby;

        static void Main(string[] args)
        {
            new SimpleHTTPServer("Web", HttpPort);
            WebSocketServer wss = new WebSocketServer($"ws://0.0.0.0:{WebSocketPort}");
            wss.Start(socket => {
                socket.OnOpen = () => socket.Send("Halloasdasdasd");
                socket.OnOpen = () => NewSpieler(socket);
            });

#if DEBUG
            Process.Start($"http://localhost:{HttpPort}");
#endif
        }

        private static void NewSpieler(IWebSocketConnection socket)
        {
            Spieler NewSpieler = new Spieler("asdasd", socket);
            NewSpieler.Socket.OnMessage = (string message) => NewSpieler.OnSend(message);
            AllSpieler.Add(NewSpieler);

         
            if(AllSpieler.Count == 1)
            {
                DasSpielfeld = new Spielfeld(AllSpieler);
                MeineLobby = new Lobby(DasSpielfeld, NewSpieler);
                MeineLobby.Init();
                //DasSpielfeld.AllSpieler = AllSpieler;
                //DasSpielfeld.SpielStart();
            } else
            {
                MeineLobby.UpdateSpieler(AllSpieler);
            }
            //if(AllSpieler.Count < 3)
            //{
            //    DasSpielfeld = new Spielfeld(AllSpieler);
            //}
            //else
            //{
            //    DasSpielfeld.AllSpieler = AllSpieler;
            //    DasSpielfeld.SpielStart();
            //}
        }
    }
}
