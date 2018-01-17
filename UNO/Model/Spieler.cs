using Fleck;
using System;
using System.Collections.Generic;
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
    }
}
