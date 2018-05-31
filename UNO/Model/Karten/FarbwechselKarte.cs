using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model.Karten
{
    class FarbwechselKarte:IKarte
    {
        public KartenTyp Typ => KartenTyp.Farbwechsel;
        public KartenFarbe Farbe { get; set; }
        public int KartenZiehen { get; }
        public int Zahl { get; }
        public FarbwechselKarte()
        {
            Farbe = KartenFarbe.Schwarz;
        }
    }
}
