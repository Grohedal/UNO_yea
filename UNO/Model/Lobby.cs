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
        public Spielfeld Tisch;

        public Spieler Tischführer;

        public Lobby(Spielfeld tisch, Spieler führer)
        {
            Tisch = tisch;
            Tischführer = führer;
        }

        public void Init()
        {
            while (!Tischführer.Spielstarten)
            {
                Thread.Sleep(200);
            }

            Tisch.SpielStart();
        }

        public void UpdateSpieler(List<ISpieler> spieler)
        {
            Tisch.AllSpieler = spieler;
        }
    }
}
