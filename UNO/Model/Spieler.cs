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

        public Spieler(string name, IWebSocketConnection socket)
        {
            Name = name;
            Socket = socket;
        }

        //public event Action ZiehtKarte;

        public void ZiehtKarte(Queue<IKarte> stapel)
        {
            Karten.Add(stapel.Dequeue());
        }

        public IKarte LegtKarte(IKarte karte)
        {
            Karten.Remove(karte);
            return karte;
        }

        public void TeileSpielStand(IKarte gelegteKarte, bool aktiv)
        {

            var obj = new { aktuelleKarte = gelegteKarte, aktiv = aktiv, hand = Karten };
            var json = new JavaScriptSerializer().Serialize(obj);
            Socket.Send(json);
        }

        public bool KannSpielerLegen(IKarte karte)
        {
            bool kannLegen = false;
            foreach (ZahlKarte k in Karten)
            {
                ZahlKarte zk = (ZahlKarte)karte;
                if (k.Farbe == karte.Farbe || zk.Zahl == k.Zahl)
                {
                    kannLegen = true;
                }
                else
                {
                    kannLegen = false;
                }
            }
            foreach (IKarte k in Karten)
            {
                if (k.Farbe == karte.Farbe || k.Typ == karte.Typ)
                {
                    kannLegen = true;
                }
                else
                {
                    kannLegen = false;
                }
            }
            return kannLegen;
        }

        public void OnSend(string message)
        {
            string s = message;
        }
    }
}
