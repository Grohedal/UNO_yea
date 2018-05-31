using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model.Karten
{
    class VierZiehenKarte : IKarte
    {
        public KartenTyp Typ => KartenTyp.VierZiehen;
        public KartenFarbe Farbe { get; set; }
        public int KartenZiehen { get; }
        public int Zahl { get; }
        public VierZiehenKarte()
        {
            Farbe = KartenFarbe.Schwarz;
        }
    }
}
