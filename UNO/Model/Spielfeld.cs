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
        Dictionary<IWebSocketConnection ,ISpieler> Spieler = new Dictionary<IWebSocketConnection, ISpieler>();
        List<ISpieler> FertigeSpieler = new List<ISpieler>();
        Queue<IKarte> Stapel = new Queue<IKarte>();
        List<IKarte> GelegteKarten = new List<IKarte>();
        ISpieler AktiverSpieler;

        public Spielfeld(IEnumerable<ISpieler> spieler)
        {
            Spieler = spieler.ToDictionary(s => s.Socket, s => s);
            InitStapel();
        }

        private void Austeilen()
        {
            for (int i = 0; i < 7; i++)
            {
                foreach (ISpieler spieler in Spieler.Values)
                {
                    spieler.Karten.Add(Stapel.Dequeue());
                }
            }
        }

        private void Spielzug()
        {
            IWebSocketConnection key = Spieler.Keys.First();
            AktiverSpieler = Spieler.Values.First();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (stopWatch.ElapsedMilliseconds < 20000)
            {
                
            }
            stopWatch.Stop();
            Spieler.Remove(key);
            Spieler.Add(key, AktiverSpieler);
            if(Spieler.Count > 1)
            {
                Spielzug();
            }
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

            Stapel.Mischen();
        }

        public void SpielStart()
        {
            Austeilen();
            GelegteKarten.Add(Stapel.Dequeue());
            Spielzug();
        }
    }
}
