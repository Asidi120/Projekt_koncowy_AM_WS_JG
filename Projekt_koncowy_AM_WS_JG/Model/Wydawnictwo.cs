using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Wydawnictwo
    {
        public string IDWydawnictwa { get; set; }
        public string Nazwa { get; set; }
        public string Kraj_zalozenia { get; set; }
        public string Rok_zalozenia { get; set; }
        public List<Ksiazka> Ksiazki { get; set; }
        //public Wydawnictwo(string nazwa, string kraj_zalozenia, string rok_zalozenia)
        //{
        //    Nazwa = nazwa;
        //    Kraj_zalozenia = kraj_zalozenia;
        //    Rok_zalozenia= rok_zalozenia;
        //    Ksiazki = new List<Ksiazka>();
        //}

        public Wydawnictwo(string id_wydaw,string nazwa, string kraj_zalozenia, string rok_zalozenia)
        {
            IDWydawnictwa = id_wydaw;
            Nazwa = nazwa;
            Kraj_zalozenia = kraj_zalozenia;
            Rok_zalozenia = rok_zalozenia;
            Ksiazki = new List<Ksiazka>();
        }
    }
}
