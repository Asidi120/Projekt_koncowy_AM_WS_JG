using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Uzytkownik
    {
        private string nick;
        private string email;
        private string plec;
        private string data_zalozenia;
        private string id_uzytkowika;
        private string root;

        public List<Ksiazka> KsiazkiChcePrzeczytac;
        public List<Ksiazka> KsiazkiPrzeczytane;
        public List<Ksiazka> KsiazkiWTrakcie;
        public List<Ksiazka> KsiazkiPorzucone;

        public Uzytkownik()
        {
            KsiazkiChcePrzeczytac = new List<Ksiazka>();
            KsiazkiPrzeczytane = new List<Ksiazka>();
            KsiazkiWTrakcie = new List<Ksiazka>();
            KsiazkiPorzucone = new List<Ksiazka>();
        }
        public Uzytkownik(string nick, string email, string plec, string data_zalozenia, string id_uzytkowika, string root)
        {
            Nick = nick;
            Email = email;
            Plec = plec;
            Data_zalozenia = data_zalozenia;
            IDUzytkownika = id_uzytkowika;
            Root = root;
        }
        public string Root
        {
            get { return root; }
            set { root = value; }
        }
        public string IDUzytkownika
        {
            get { return id_uzytkowika; }
            set {  id_uzytkowika = value;}
        }
        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Plec
        {
            get { return plec; }
            set { plec = value; }
        }
        public string Data_zalozenia
        {
            get { return data_zalozenia; }
            set { data_zalozenia = value; }
        }
    }
}
