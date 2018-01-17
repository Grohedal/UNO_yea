using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{
    interface ISpieler
    {
        IWebSocketConnection Socket { get; }
        string Name { get; }
        List<IKarte> Karten { get; }
        bool Aussetzen { get; }

        //event Action ZiehtKarte;
        void ZiehtKarte(Queue<IKarte> stapel);

        bool KannSpielerLegen(IKarte karte);

        event Func<IKarte> LegtKarte;

        void TeileSpielStand(IKarte gelegteKarte, bool aktiv);
    }
}
