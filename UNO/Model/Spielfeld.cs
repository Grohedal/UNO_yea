using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using UNO.Model.Karten;

namespace UNO.Model
{
    class Spielfeld
    {
        public List<ISpieler> AllSpieler = new List<ISpieler>();
        List<ISpieler> FertigeSpieler = new List<ISpieler>();
        Queue<IKarte> Stapel = new Queue<IKarte>();
        List<IKarte> GelegteKarten = new List<IKarte>();
        ISpieler AktiverSpieler;
        bool NichtGelegt = true;
        bool vierziehenAktiv = false;
        int KartenZiehen;

        public Spielfeld(IEnumerable<ISpieler> spieler)
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
                    if (Stapel.Count() < 7)
                    {
                        InitStapel();
                    }
                    spieler.Karten.Add(Stapel.Dequeue());
                }
            }
        }

        private void LegtKarte(IKarte karte)
        {
            if (AktiverSpieler.Ziehen != true || karte.Typ == KartenTyp.Ziehen || karte.Typ == KartenTyp.VierZiehen)
            {
                AktiverSpieler.Karten.Remove(karte);
                GelegteKarten.Add(karte);
                NichtGelegt = false;
            }
        }

        private void Spielzug()
        {
            if (AllSpieler.Count == 1)
            {
                SpielNeustart();
            }

            AktiverSpieler = AllSpieler.First();

            if (AktiverSpieler.Aussetzen == true)
            {
                if (AktiverSpieler.Ki == true)
                {
                    ((KI)AktiverSpieler).Aussetzen = false;
                }
                else
                {
                    ((Spieler)AktiverSpieler).Aussetzen = false;
                }
                NächsterSpieler();
            }
            foreach (ISpieler temp in AllSpieler)
            {
                if (temp.Ki == false)
                {
                    if (temp == AktiverSpieler)
                    {
                        temp.TeileSpielStand(GelegteKarten.Last(), true, AllSpieler);
                    }
                    else
                    {
                        temp.TeileSpielStand(GelegteKarten.Last(), false, AllSpieler);
                    }
                }
            }

            if (AktiverSpieler.Ki)
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                while (st.ElapsedMilliseconds < 1500)
                {

                }
                st.Stop();
            }
            NichtGelegt = true;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (stopWatch.ElapsedMilliseconds < 20000 && NichtGelegt)
            {
                if (AktiverSpieler.Ki == true)
                {
                    KIMachtZug();
                    break;
                }
                else if (AktiverSpieler.CardIndex != null)
                {
                    IKarte gelegteKarteSpieler = AktiverSpieler.Karten[(int)AktiverSpieler.CardIndex];
                    if (VersuchtKarteLegen(gelegteKarteSpieler))
                    {
                        if (gelegteKarteSpieler.Farbe == KartenFarbe.Schwarz || gelegteKarteSpieler.Typ == KartenTyp.VierZiehen)
                        {
                            int blauCounter = 0;
                            int rotCounter = 0;
                            int gelbCounter = 0;
                            int grünCounter = 0;
                            foreach (IKarte zk in AktiverSpieler.Karten)
                            {
                                switch (zk.Farbe)
                                {
                                    case KartenFarbe.Gelb:
                                        gelbCounter++;
                                        break;
                                    case KartenFarbe.Rot:
                                        rotCounter++;
                                        break;
                                    case KartenFarbe.Blau:
                                        blauCounter++;
                                        break;
                                    case KartenFarbe.Gruen:
                                        grünCounter++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (blauCounter > rotCounter && blauCounter > grünCounter && blauCounter > gelbCounter)
                            {
                                gelegteKarteSpieler.Farbe = KartenFarbe.Blau;
                            }
                            else if (rotCounter > blauCounter && rotCounter > grünCounter && rotCounter > gelbCounter)
                            {
                                gelegteKarteSpieler.Farbe = KartenFarbe.Rot;
                            }
                            else if (grünCounter > rotCounter && grünCounter > blauCounter && grünCounter > gelbCounter)
                            {
                                gelegteKarteSpieler.Farbe = KartenFarbe.Gruen;
                            }
                            else
                            {
                                gelegteKarteSpieler.Farbe = KartenFarbe.Gelb;
                            }
                            if (gelegteKarteSpieler.Typ == KartenTyp.VierZiehen)
                            {
                                vierziehenAktiv = true;
                                KartenZiehen += 4;
                            }
                            LegtKarte(gelegteKarteSpieler);
                        }
                        else
                        {
                            LegtKarte(gelegteKarteSpieler);
                        }
                        break;
                    }
                    else
                    {
                        if (vierziehenAktiv)
                        {
                            for (int i = 0; i < KartenZiehen; i++)
                            {
                                GenugKartenImStapel();
                                AktiverSpieler.ZiehtKarte(Stapel);
                            }
                            KartenZiehen = 0;
                            vierziehenAktiv = false;
                            ((Spieler)AktiverSpieler).Ziehen = false;
                            ((Spieler)AktiverSpieler).CardIndex = null;
                        }
                        else
                        {
                            if (AktiverSpieler.Ziehen == true)
                            {
                                break;
                            }
                            GenugKartenImStapel();
                            AktiverSpieler.ZiehtKarte(Stapel);
                            ((Spieler)AktiverSpieler).CardIndex = null;
                        }
                        NächsterSpieler();
                    }
                }
            }
            if (GelegteKarten.Last().Typ == KartenTyp.Ziehen && !NichtGelegt)
            {
                if (AllSpieler[1].Ki)
                {
                    ((KI)AllSpieler[1]).Ziehen = true;
                }
                else
                {
                    ((Spieler)AllSpieler[1]).Ziehen = true;
                }
                if (AktiverSpieler.Ki)
                {
                    ((KI)AktiverSpieler).Ziehen = false;
                }
                else
                {
                    ((Spieler)AktiverSpieler).Ziehen = false;
                }
                KartenZiehen += 2;
            }
            else if (KartenZiehen != 0)
            {
                if (NichtGelegt)
                {
                    for (int i = 0; i < KartenZiehen; i++)
                    {
                        GenugKartenImStapel();
                        AktiverSpieler.ZiehtKarte(Stapel);
                    }
                    vierziehenAktiv = false;
                    KartenZiehen = 0;
                    if (AktiverSpieler.Ki)
                    {
                        ((KI)AktiverSpieler).Ziehen = false;
                    }
                    else
                    {
                        ((Spieler)AktiverSpieler).Ziehen = false;
                    }
                }
            }
            else if (KartenZiehen == 0 && NichtGelegt)
            {
                GenugKartenImStapel();
                AktiverSpieler.ZiehtKarte(Stapel);
            }
            stopWatch.Stop();
            if (AktiverSpieler.Karten.Count == 0)
            {
                SpielerGewinnt();
                AktiverSpieler.HastGewonnen();
                Spielzug();
            }
            if (AllSpieler.Count > 1)
            {
                if (!AktiverSpieler.Ki)
                {
                    ((Spieler)AktiverSpieler).CardIndex = null;
                }
                NächsterSpieler();
            }
        }

        private void KIMachtZug()
        {
            if (vierziehenAktiv)
            {
                ((KI)AktiverSpieler).ZiehtKarte(Stapel);
                ((KI)AktiverSpieler).ZiehtKarte(Stapel);
                ((KI)AktiverSpieler).ZiehtKarte(Stapel);
                ((KI)AktiverSpieler).ZiehtKarte(Stapel);
                KartenZiehen = 0;
                NichtGelegt = false;
                vierziehenAktiv = false;
            }
            else
            {
                ((KI)AktiverSpieler).ÜberprüftKarten(GelegteKarten.Last());
                IKarte gelegteKarteSpieler = ((KI)AktiverSpieler).LegtKarte();
                if (VersuchtKarteLegen(gelegteKarteSpieler))
                {
                    if (gelegteKarteSpieler.Typ == KartenTyp.VierZiehen)
                    {
                        vierziehenAktiv = true;
                        KartenZiehen += 4;
                        gelegteKarteSpieler.Farbe = KartenFarbe.Blau;
                    }
                    else if (gelegteKarteSpieler.Typ == KartenTyp.Farbwechsel)
                    {
                        gelegteKarteSpieler.Farbe = KartenFarbe.Blau;
                    }
                    LegtKarte(gelegteKarteSpieler);
                }
                else
                {
                    if (((KI)AktiverSpieler).Ziehen != true)
                    {
                        GenugKartenImStapel();
                        ((KI)AktiverSpieler).ZiehtKarte(Stapel);
                        ((KI)AktiverSpieler).CardIndex = null;
                        NächsterSpieler();
                    }
                }
            }
        }

        private void SpielerGewinnt()
        {
            AllSpieler.Remove(AktiverSpieler);
            FertigeSpieler.Add(AktiverSpieler);
            SpielEnde();
        }

        private void GenugKartenImStapel()
        {
            if (Stapel.Count == 0)
            {
                if (GelegteKarten.Count > 4)
                {
                    Stapel = new Queue<IKarte>(GelegteKarten.GetRange(0, GelegteKarten.Count - 1));
                    GelegteKarten.RemoveRange(0, GelegteKarten.Count - 1);
                }
                else
                {
                    InitStapel();
                }
            }
        }


        public bool VersuchtKarteLegen(IKarte karte)
        {
            IKarte obersteKarte = GelegteKarten.Last();
            //Schwarze Karten noch nicht da
            if (vierziehenAktiv)
            {
                return false;
            }
            if (karte.Farbe == obersteKarte.Farbe)
            {
                if (karte.Typ == KartenTyp.Richtungswechsel)
                {
                    Richtungswechsel();
                }
                else if (karte.Typ == KartenTyp.Aussetzen)
                {
                    if (AllSpieler[1].Ki)
                    {
                        ((KI)AllSpieler[1]).Aussetzen = true;
                    }
                    else
                    {
                        ((Spieler)AllSpieler[1]).Aussetzen = true;
                    }
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
                if (AllSpieler[1].Ki)
                {
                    ((KI)AllSpieler[1]).Aussetzen = true;
                }
                else
                {
                    ((Spieler)AllSpieler[1]).Aussetzen = true;
                }
                return true;
            }
            else if (karte.Farbe == KartenFarbe.Schwarz)
            {
                return true;
            }
            return false;
        }

        private void Richtungswechsel()
        {
            AllSpieler.Reverse();
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
                    Stapel.Enqueue(new FarbwechselKarte());
                    Stapel.Enqueue(new FarbwechselKarte());
                    Stapel.Enqueue(new FarbwechselKarte());
                    Stapel.Enqueue(new FarbwechselKarte());
                    Stapel.Enqueue(new VierZiehenKarte());
                    Stapel.Enqueue(new VierZiehenKarte());
                    Stapel.Enqueue(new VierZiehenKarte());
                    Stapel.Enqueue(new VierZiehenKarte());
                    continue;
                }
                Stapel.Enqueue(new ZahlKarte(0, farbe));
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        Stapel.Enqueue(new ZahlKarte(i, farbe));
                    }
                    Stapel.Enqueue(new ZweiZiehenKarte(farbe));
                    Stapel.Enqueue(new RichtungswechselKarte(farbe));
                    Stapel.Enqueue(new AussetzenKarte(farbe));
                }
            }

            Stapel = Stapel.Mischen();
        }

        public void SpielStart()
        {
            int count = 0;
            while (AllSpieler.Count < 4)
            {
                AllSpieler.Add(new KI("Knud" + count, null));
                count++;
            }
            Austeilen();
            GelegteKarten.Add(Stapel.Dequeue());
            if (GelegteKarten[0].Typ == KartenTyp.Ziehen)
            {
                ((Spieler)AllSpieler[0]).Ziehen = true;
                KartenZiehen = 2;
            }
            else if (GelegteKarten[0].Typ == KartenTyp.VierZiehen || GelegteKarten[0].Typ == KartenTyp.Farbwechsel)
            {
                GelegteKarten.Add(Stapel.Dequeue());
                for (int i = 1; i < 99; i++)
                {
                    if (GelegteKarten[i].Typ == KartenTyp.VierZiehen || GelegteKarten[i].Typ == KartenTyp.Farbwechsel)
                    {
                        GelegteKarten.Add(Stapel.Dequeue());
                    }
                    else
                    {
                        i = 100;
                    }
                }
            }
            Spielzug();
        }

        public void SpielEnde()
        {
            List<ISpieler> AlleSpielerImSpiel = AllSpieler.Concat(FertigeSpieler).ToList();
            foreach (ISpieler sp in AlleSpielerImSpiel)
            {
                if (sp.Karten.Count > 0)
                {
                    sp.Karten.RemoveAll(x => (true));

                }
            }

            AllSpieler.RemoveRange(0, AllSpieler.Count - 1);
            FertigeSpieler.RemoveRange(0, FertigeSpieler.Count - 1);
            Stapel.Clear();
            GelegteKarten.RemoveRange(0, GelegteKarten.Count - 1);
            AllSpieler = AlleSpielerImSpiel;

            for (int i = 0; i < AllSpieler.Count; i++)
            {
                object ki = AllSpieler.Where(x => x.Ki == true).FirstOrDefault();
                if (ki != null)
                {
                    AllSpieler.Remove(AllSpieler.Where(x => x.Ki == true).FirstOrDefault());
                    i--;
                }
            }
            foreach (ISpieler qwe in AllSpieler)
            {
                qwe.Spielstarten = false;
                var obj = new { spielEnde = true };
                var json = new JavaScriptSerializer().Serialize(obj);
                qwe.Socket.Send(json);
            }

            if(AllSpieler.Count > 1)
            {
                ISpieler ersterSpieler = AllSpieler.First();
                while(ersterSpieler.Spielstarten != true)
                {
                    Thread.Sleep(200);
                }
                SpielStart();
            }
        }

        private void SpielNeustart()
        {
            List<ISpieler> AlleSpielerImSpiel = AllSpieler.Concat(FertigeSpieler).ToList();
            foreach (ISpieler sp in AlleSpielerImSpiel)
            {
                if (sp.Karten.Count > 0)
                {
                    sp.Karten.RemoveRange(0, sp.Karten.Count - 1);
                }
            }
            AllSpieler.RemoveRange(0, AllSpieler.Count - 1);
            FertigeSpieler.RemoveRange(0, FertigeSpieler.Count - 1);
            Stapel.Clear();
            GelegteKarten.RemoveRange(0, GelegteKarten.Count - 1);
            AllSpieler = AlleSpielerImSpiel;
            InitStapel();
            SpielStart();
        }
    }
}
