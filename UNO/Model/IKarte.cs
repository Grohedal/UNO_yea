using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{
    interface IKarte
    {
        KartenTyp Typ { get; }
        KartenFarbe Farbe { get; }
    }
}
