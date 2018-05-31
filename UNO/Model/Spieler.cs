using Fleck;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Model.Karten;

namespace UNO.Model
{
    class Spieler : ISpieler
    {
        public IWebSocketConnection Socket { get; }
        public List<IKarte> Karten { get; } = new List<IKarte>();
        public string Name { get; }
        public bool Aussetzen { get; set; }
        public bool Ziehen { get; set; }
        public bool Ki { get; }
        public int? CardIndex { get; set; }

        public bool Spielstarten { get; set; }



        public Spieler(string name, IWebSocketConnection socket)
        {
            Ki = false;
            Name = name;
            Socket = socket;
            Spielstarten = false;
        }

        //public event Action ZiehtKarte;

        public void ZiehtKarte(Queue<IKarte> stapel)
        {
            Karten.Add(stapel.Dequeue());
        }

        public void TeileSpielStand(IKarte gelegteKarte, bool aktiv)
        {

            var obj = new { aktuelleKarte = gelegteKarte, aktiv = aktiv, hand = Karten , name = Name};
            var json = new JavaScriptSerializer().Serialize(obj);
            Socket.Send(json);
        }

        public void OnSend(string message)
        {
            if (message == "START") {
                Spielstarten = true;
            } else if(message != "Ping")
            {
                string s = message;
                s = s.Replace("card-", "");
                try
                {
                    int cardIndex = Int32.Parse(s);
                    CardIndex = cardIndex;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                    CardIndex = null;
                }
            }

        }

        public void HastGewonnen()
        {

            var obj = new { gewonnen = true};
            var json = new JavaScriptSerializer().Serialize(obj);
            Socket.Send(json);
        }
    }
}
