using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model.Karten
{
    class RichtungswechselKarte :IKarte
    {
        public KartenTyp Typ => KartenTyp.Richtungswechsel;

        public KartenFarbe Farbe { get; set; }

        public RichtungswechselKarte(KartenFarbe farbe)
        {
            Farbe = farbe;
        }
    }
}
