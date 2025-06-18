using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Status
    {
        public string IDUzytkownika{ get; set; }
        public string IDKsiazki { get; set; }
        public string StatusKsiazki { get; set; } 
        public Status(string iD_uzytkownika, string iD_ksiazki, string status_ksiazki)
        {
            IDUzytkownika = iD_uzytkownika;
            IDKsiazki = iD_ksiazki;
            StatusKsiazki = status_ksiazki;
        }
    }
}
