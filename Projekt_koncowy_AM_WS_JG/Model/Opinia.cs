using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Opinia
    {
        public string Ocena {  get; set; }
        public string Recenzja { get; set; }
        public string Użytkownik { get; set; }
        public string Data_Wystawienia { get; set; }

        public Opinia(string ocena, string recenzja, string użytkownik, string data_wystawienia)
        {
            Ocena = ocena;
            Recenzja = recenzja;
            Użytkownik = użytkownik;
            Data_Wystawienia = data_wystawienia;
        }

    }
}
