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
        public int RokWydania { get; set; }
        public int LiczbaStron { get; set; }
        public string Jezyk { get; set; }
        public string Wydawnictwo { get; set; }
        public string Średnia_Ocena { get; set; }
        public string Liczba_Ocen { get; set; }
        public List<float> Lista_ocen { get; set; }
    }
}