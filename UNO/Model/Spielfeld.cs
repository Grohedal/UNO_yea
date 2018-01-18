using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Model.Karten;

namespace UNO.Model
{
    class Spielfeld
    {
        public List<Spieler> AllSpieler = new List<Spieler>();
        List<Spieler> FertigeSpieler = new List<Spieler>();
        Queue<IKarte> Stapel = new Queue<IKarte>();
        List<IKarte> GelegteKarten = new List<IKarte>();
        Spieler AktiverSpieler;
        bool NichtGelegt = true;
        int KartenZiehen;

        public Spielfeld(IEnumerable<Spieler> spieler)
        {
            KartenZiehen = 0;
            AllSpieler = spieler.ToList();
            InitStapel();
        }

        private void Austeilen()
        {
            for (int i = 0; i < 7; i++)
            {
                foreach (ISpieler spieler in AllSpieler)
                {
                    spieler.Karten.Add(Stapel.Dequeue());
                }
            }
        }

        private void Spielzug()
        {
            AktiverSpieler = AllSpieler.First();
            
            if (AktiverSpieler.Aussetzen == true)
            {
                NächsterSpieler();
            }

            AktiverSpieler.ZiehtKarte(Stapel);

            foreach (ISpieler temp in AllSpieler)
            {
                if(temp == AktiverSpieler)
                {
                    temp.TeileSpielStand(GelegteKarten.Last(), true);
                } else
                {
                    temp.TeileSpielStand(GelegteKarten.Last(), false);
                }
                
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //NichtGelegt = AktiverSpieler.KannSpielerLegen(GelegteKarten.Last());
            while (stopWatch.ElapsedMilliseconds < 2000 && NichtGelegt)
            {
                if(AktiverSpieler.CardIndex != null)
                {
                    IKarte gelegteKarteSpieler = AktiverSpieler.Karten[(int) AktiverSpieler.CardIndex];
                }
            }
            if (GelegteKarten.Last().Typ == KartenTyp.Ziehen && !NichtGelegt)
            {
                KartenZiehen += 2;
            }
            else if (KartenZiehen != 0)
            {
                for (int i = 0; i < KartenZiehen; i++)
                {
                    AktiverSpieler.ZiehtKarte(Stapel);
                }
                KartenZiehen = 0;
            }
            else
            {
                AktiverSpieler.ZiehtKarte(Stapel);
            }
            stopWatch.Stop();
            if (AllSpieler.Count > 1)
            {
                NächsterSpieler();
            }
        }

        public bool VersuchtKarteLegen(IKarte karte)
        {
            IKarte obersteKarte = GelegteKarten.Last();
            //Schwarze Karten noch nicht da
            if (karte.Farbe == obersteKarte.Farbe)
            {
                if (karte.Typ == KartenTyp.Richtungswechsel)
                {
                    Richtungswechsel();
                }
                else if (karte.Typ == KartenTyp.Aussetzen)
                {
                    AllSpieler[1].Aussetzen = true;
                }
                return true;
            }
            else if (karte.Typ == KartenTyp.Zahl && obersteKarte.Typ == KartenTyp.Zahl)
            {
                ZahlKarte zk = (ZahlKarte)karte;
                ZahlKarte zk2 = (ZahlKarte)obersteKarte;
                if (zk.Zahl == zk2.Zahl)
                {
                    return true;
                }
            }
            else if (karte.Typ == KartenTyp.Ziehen && obersteKarte.Typ == KartenTyp.Ziehen)
            {
                return true;
            }
            else if (karte.Typ == KartenTyp.Richtungswechsel && obersteKarte.Typ == KartenTyp.Richtungswechsel)
            {
                Richtungswechsel();
                return true;
            }
            else if (karte.Typ == KartenTyp.Aussetzen && obersteKarte.Typ == KartenTyp.Aussetzen)
            {
                AllSpieler[1].Aussetzen = true;
                return true;
            }
            return false;
        }

        private void Richtungswechsel()
        {
            Stapel.Reverse();
        }

        private void NächsterSpieler()
        {
            AllSpieler.Remove(AktiverSpieler);
            AllSpieler.Add(AktiverSpieler);
            Spielzug();
        }

        private void InitStapel()
        {
            foreach (KartenFarbe farbe in Enum.GetValues(typeof(KartenFarbe)))
            {
                if (farbe == KartenFarbe.Schwarz)
                {
                    continue;
                }

                for (int i = 0; i <= 9; i++)
                {
                    Stapel.Enqueue(new ZahlKarte(i, farbe));
                }

                Stapel.Enqueue(new ZweiZiehenKarte(farbe));
            }

            Stapel = Stapel.Mischen();
        }

        public void SpielStart()
        {
            Austeilen();
            GelegteKarten.Add(Stapel.Dequeue());
            Spielzug();
        }
    }
}
