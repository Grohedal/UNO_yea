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
        public List<ISpieler> Spieler = new List<ISpieler>();
        List<ISpieler> FertigeSpieler = new List<ISpieler>();
        Queue<IKarte> Stapel = new Queue<IKarte>();
        List<IKarte> GelegteKarten = new List<IKarte>();
        ISpieler AktiverSpieler;
        bool NichtGelegt = true;
        int KartenZiehen;

        public Spielfeld(IEnumerable<ISpieler> spieler)
        {
            KartenZiehen = 0;
            Spieler = spieler.ToList();
            InitStapel();
        }

        private void Austeilen()
        {
            for (int i = 0; i < 7; i++)
            {
                foreach (ISpieler spieler in Spieler)
                {
                    spieler.Karten.Add(Stapel.Dequeue());
                }
            }
        }

        private void Spielzug()
        {
            AktiverSpieler = Spieler.First();
            
            if (AktiverSpieler.Aussetzen == true)
            {
                NächsterSpieler();
            }

            AktiverSpieler.ZiehtKarte(Stapel);

            foreach (ISpieler temp in Spieler)
            {
                temp.TeileSpielStand(GelegteKarten.Last(), true);
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //NichtGelegt = AktiverSpieler.KannSpielerLegen(GelegteKarten.Last());
            while (stopWatch.ElapsedMilliseconds < 20000 && NichtGelegt)
            {

            }
            if (GelegteKarten.Last().Typ == KartenTyp.Ziehen && !NichtGelegt)
            {
                KartenZiehen += 2;
            }
            else
            {
                if (KartenZiehen == 0)
                {
                    AktiverSpieler.ZiehtKarte(Stapel);
                }
                for (int i = 0; i < KartenZiehen; i++)
                {
                    AktiverSpieler.ZiehtKarte(Stapel);
                }
                KartenZiehen = 0;
            }
            stopWatch.Stop();
            if (Spieler.Count > 1)
            {
                NächsterSpieler();
            }
        }

        private void NächsterSpieler()
        {
            Spieler.Remove(AktiverSpieler);
            Spieler.Add(AktiverSpieler);
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
