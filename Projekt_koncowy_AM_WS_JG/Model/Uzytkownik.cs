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
