using Fleck;
using System;
using System.Collections.Generic;
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
            Austeilen();
            GelegteKarten.Add(Stapel.Dequeue());
            AktiverSpieler = Spieler.Values.First();
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
            throw new NotImplementedException();
        }
    }
}
