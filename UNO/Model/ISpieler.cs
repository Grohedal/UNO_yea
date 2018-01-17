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

        event Action ZiehtKarte;

        event Func<IKarte> LegtKarte;
    }
}
