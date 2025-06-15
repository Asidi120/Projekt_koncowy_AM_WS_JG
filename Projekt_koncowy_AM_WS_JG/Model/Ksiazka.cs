using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Ksiazka
    {
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public string Opis { get; set; }
        public string Gatunek { get; set; }
        public string Okladka { get; set; }
        public string RokWydania { get; set; }
        public string LiczbaStron { get; set; }
        public string Jezyk { get; set; }
        public string Wydawnictwo { get; set; }
        public string Średnia_Ocena { get; set; }
        public string Liczba_Ocen { get; set; }
        public List<float> Lista_ocen { get; set; }

        public Ksiazka (string tytul, string autor, string opis, string gatunek, string rokWydania, string liczbaStron, string jezyk, string wydawnictwo)
        {
            Tytul = tytul;
            Autor = autor;
            Opis = opis;
            Gatunek = gatunek;
            //Okladka = okladka;
            RokWydania = rokWydania;
            LiczbaStron = liczbaStron;
            Jezyk = jezyk;
            Wydawnictwo = wydawnictwo;
            //Średnia_Ocena = średnia_Ocena;
            //Liczba_Ocen = liczba_Ocen;
            //Lista_ocen = lista_ocen;
        }
    }
}