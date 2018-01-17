using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model.Karten
{
    class ZweiZiehenKarte : IKarte
    {
        public KartenTyp Typ => KartenTyp.Ziehen;

        public KartenFarbe Farbe { get;}

        public int Anzahl { get; } = 2;

        public ZweiZiehenKarte(KartenFarbe farbe)
        {
            Farbe = farbe;
        }
    }
}
