﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Autor
    {
        public Autor() { }
        public string IDAutora { get; set; }
        public string ImieNazwisko { get; set; }
        public string Imie {  get; set; }
        public string Nazwisko { get; set; }
        public string DataUrodzenia { get; set; }
        public string Narodowosc { get; set; }
        public List<Ksiazka> Ksiazki { get; set; }
        public Autor(string id_autora, string imie_nazwisko, string data_urodzenia, string narodowosc)
        {
            IDAutora = id_autora;
            ImieNazwisko = imie_nazwisko;
            DataUrodzenia = data_urodzenia;
            Narodowosc = narodowosc;
            Ksiazki = new List<Ksiazka>();
        }

        public Autor(string id_autora, string imie, string nazwisko, string data_urodzenia, string narodowosc)
        {
            IDAutora = id_autora;
            Imie = imie;
            Nazwisko = nazwisko;
            DataUrodzenia= data_urodzenia;
            Narodowosc = narodowosc;
        }
    }
}
