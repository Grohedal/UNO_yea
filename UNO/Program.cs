using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UNO.Model;

namespace UNO
{
    class Program
    {
        const int HttpPort = 1337;
        const int WebSocketPort = 666;
        static string Ip = GetLocalIPAddress();
        static List<ISpieler> AllSpieler = new List<ISpieler>();
        static Spielfeld DasSpielfeld;
        static Lobby MeineLobby;

        static void Main(string[] args)
        {
            new SimpleHTTPServer("Web", HttpPort);
            WebSocketServer wss = new WebSocketServer($"ws://{Ip}:{WebSocketPort}");
            wss.Start(socket => {
                socket.OnOpen = () => socket.Send("Halloasdasdasd");
                socket.OnOpen = () => LobbyÜbersicht(socket);
            });

#if DEBUG
            Process.Start($"http://{Ip}:{HttpPort}");
#endif
        }

        private static void LobbyÜbersicht(IWebSocketConnection socket)
        {
            if(MeineLobby == null)
            {
                MeineLobby = new Lobby();
            }
            Spieler CurrentSpieler = new Spieler("asdasd", socket, MeineLobby);
            CurrentSpieler.Socket.OnMessage = (string message) => CurrentSpieler.OnSend(message);
            MeineLobby.SpielerHinzufügen(CurrentSpieler);

        }

        //private static void NewSpieler(IWebSocketConnection socket)
        //{
        //    Spieler NewSpieler = new Spieler("asdasd", socket);
        //    NewSpieler.Socket.OnMessage = (string message) => NewSpieler.OnSend(message);
        //    AllSpieler.Add(NewSpieler);

         
        //    if(AllSpieler.Count == 1)
        //    {
        //        DasSpielfeld = new Spielfeld(AllSpieler);
        //        MeineLobby = new Lobby(DasSpielfeld, NewSpieler);
        //        MeineLobby.Init();
        //    } else
        //    {
        //        MeineLobby.UpdateSpieler(AllSpieler);
        //    }

        //}

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
