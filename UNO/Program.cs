using Fleck;
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
        static List<Spieler> AllSpieler = new List<Spieler>();

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
            AllSpieler.Add(new Spieler("asdasd", socket));
            if(AllSpieler.Count >= 2)
            {
                Spielfeld spielfeld = new Spielfeld(AllSpieler);
            }
        }
    }
}
