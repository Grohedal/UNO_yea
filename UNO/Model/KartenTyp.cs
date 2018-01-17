using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{
    [Flags]
    enum KartenTyp
    {
        Zahl = 1,
        Ziehen = 2,
        Aussetzen = 4,
        Richtungswechsel = 8,
        Farbwechsel = 16
    }
}
