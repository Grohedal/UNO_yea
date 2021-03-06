﻿using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{
    interface ISpieler
    {
        IWebSocketConnection Socket { get; }
        string Name { get; }
        List<IKarte> Karten { get; }
        bool Aussetzen { get; }
        bool Ki { get; }
        bool Ziehen { get; }
        int? CardIndex { get;  }

        bool Spielstarten { get; set; }

        //event Action ZiehtKarte;
        void ZiehtKarte(Queue<IKarte> stapel);
        
        void TeileSpielStand(IKarte gelegteKarte, bool aktiv, List<ISpieler> mitspieler);

        void HastGewonnen();

    }
}
