using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model.Karten
{
    class ZahlKarte : IKarte
    {
        KartenTyp IKarte.Typ => KartenTyp.Zahl;

        public KartenFarbe Farbe { get; }

        public int Zahl { get; }

        public ZahlKarte(int zahl, KartenFarbe farbe)
        {
            Zahl = zahl;
            Farbe = farbe;
        }
    }
}
