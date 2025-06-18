using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class InformacjeZBazy
    {
        public List<Ksiazka> Ksiazki { get; set; }
        public List<Autor> Autorzy { get; set; }
        public List<Wydawnictwo> Wydawnictwa { get; set; }
        public List<Opinia> Opinie { get; set; }
        public List<Uzytkownik> Uzytkownicy { get; set; }
        public List<Status> Statusy { get; set; }
        public InformacjeZBazy() {
            Ksiazki = new List<Ksiazka>();
            Autorzy = new List<Autor>();
            Wydawnictwa = new List<Wydawnictwo>();
            Opinie = new List<Opinia>();
            Uzytkownicy = new List<Uzytkownik>();
            Statusy = new List<Status>();
        }
    }
}
