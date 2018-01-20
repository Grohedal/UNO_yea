using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model.Karten
{
    class AussetzenKarte:IKarte
    {
        KartenTyp IKarte.Typ => KartenTyp.Aussetzen;

        public KartenFarbe Farbe { get; }

        public AussetzenKarte(KartenFarbe farbe)
        {
            Farbe = farbe;
        }
    }
}
