using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using UNO.Model.Karten;
using UNO.Model;

namespace UNO.Model
{
    class KI :ISpieler
    {
        public IWebSocketConnection Socket { get; }
        public List<IKarte> Karten { get; } = new List<IKarte>();
        public List<IKarte> LegbareKarten { get; set; }
        public string Name { get; }
        public bool Aussetzen { get; set; }
        public bool Ziehen { get; set; }
        public bool Ki { get; }
        public int? CardIndex { get; set; }

        public KI(string name, IWebSocketConnection socket)
        {
            Ki = true;
            Socket = null;
            Name = name;
        }
        
        public void ÜberprüftKarten(IKarte karte)
        {
            LegbareKarten = new List<IKarte>();
            foreach (IKarte k in Karten)
            {
                if (k.Farbe == karte.Farbe)
                {
                    LegbareKarten.Add(k);
                }
                else if(k.Typ == karte.Typ)
                {
                    if (k.Typ == KartenTyp.Zahl)
                    {
                        if (((ZahlKarte)k).Zahl == ((ZahlKarte)karte).Zahl)
                        {
                            LegbareKarten.Add(k);
                        }
                    }
                    else
                    {
                        LegbareKarten.Add(k);
                    }
                }
            }
        }

        public IKarte LegtKarte()
        {
            if (LegbareKarten.Count > 0)
            {
                IKarte k = LegbareKarten[0];
                Karten.Remove(k);
                return k;
            }
            else
            {
                return Karten[0];
            }
        }

        public void ZiehtKarte(Queue<IKarte> stapel)
        {
            Karten.Add(stapel.Dequeue());
        }

        public void TeileSpielStand(IKarte gelegteKarte, bool aktiv)
        {
        }
    }
}
