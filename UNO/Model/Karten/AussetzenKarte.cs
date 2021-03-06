﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model.Karten
{
    class AussetzenKarte:IKarte
    {
        public KartenTyp Typ => KartenTyp.Aussetzen;

        public KartenFarbe Farbe { get; set; }

        public AussetzenKarte(KartenFarbe farbe)
        {
            Farbe = farbe;
        }
    }
}
