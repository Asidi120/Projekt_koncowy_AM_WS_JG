using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class Opinie
    {
        public string Średnia_Ocena { get; set; }
        public string Liczba_Ocen { get; set; }
        public List<Opinia> Lista_Opinii { get; set; }
 

        public Opinie(string średnia_ocena, string liczba_ocen)
        {
            Średnia_Ocena = średnia_ocena;
            Liczba_Ocen = liczba_ocen;
            Lista_Opinii = new List<Opinia>();
        }
       

    }
}
