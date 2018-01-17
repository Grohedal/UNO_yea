using Fleck;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public event Action ZiehtKarte;

        public event Func<IKarte> LegtKarte;

        public void TeileSpielStand(IKarte gelegteKarte, bool aktiv)
        {
            
            var obj = new { aktuelleKarte = gelegteKarte, aktiv = aktiv, hand = Karten };
            var json = new JavaScriptSerializer().Serialize(obj);
            Socket.Send(json);
        }
    }
}
