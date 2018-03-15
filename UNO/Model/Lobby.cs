using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UNO.Model
{
    class Lobby
    {
        public List<Spielfeld> Tische { get; }
        public List<ISpieler> AlleSpieler { get; }

        public Lobby()
        {
            Tische = new List<Spielfeld>();
            AlleSpieler = new List<ISpieler>();

        }

        public Lobby(Spielfeld tisch)
        {
            Tische.Add(tisch);
        }

        public void Init(Spielfeld Tisch)
        {
            while(!Tisch.Tischführer.Spielstarten)
            {
                Thread.Sleep(200);
            }

            Tisch.SpielStart();
        }

        public void SpielerHinzufügen(ISpieler Spieler)
        {
            AlleSpieler.Add(Spieler);
        }

        public void UpdateSpieler(List<ISpieler> spieler, Spielfeld Tisch)
        {
            Tisch.AllSpieler = spieler;
        }
    }
}
