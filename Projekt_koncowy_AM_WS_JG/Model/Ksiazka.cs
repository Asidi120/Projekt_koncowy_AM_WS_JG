using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Ksiazka
    {
        public string IDKsiazki {  get; set; }
        public string IDAutora {  get; set; }
        public string IDWydawnictwa { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public string Opis { get; set; }
        public string Gatunek { get; set; }
        public string RokWydania { get; set; }
        public string LiczbaStron { get; set; }
        public string Jezyk { get; set; }
        public string Wydawnictwo { get; set; }
        public Opinie JakieOpinie { get; set; }
        public string Okladka { get; set; }

        public Ksiazka (string id_ksiazki, string id_autora, string id_wydawnictwa, string tytul, string autor, string opis, string gatunek, string rokWydania, string liczbaStron, string jezyk, string wydawnictwo, Opinie jakieopinie, string okladka)
        {
            IDKsiazki = id_ksiazki;
            IDAutora = id_autora;
            IDWydawnictwa = id_wydawnictwa;
            Tytul = tytul;
            Autor = autor;
            Opis = opis;
            Gatunek = gatunek;
            RokWydania = rokWydania;
            LiczbaStron = liczbaStron;
            Jezyk = jezyk;
            Wydawnictwo = wydawnictwo;
            JakieOpinie = jakieopinie;
            Okladka = okladka;
        }
        public Ksiazka(string id_ksiazki, string tytul,string gatunek,string opis,string jezyk,string rokWydania,string liczbaStron,string id_autora,string id_wydawnictwa)
        {
            IDKsiazki = id_ksiazki;
            Tytul = tytul;
            Gatunek = gatunek;
            Opis = opis;
            Jezyk = jezyk;
            RokWydania = rokWydania;
            LiczbaStron = liczbaStron;
            IDAutora = id_autora;
            IDWydawnictwa = id_wydawnictwa;
        }
    }
}