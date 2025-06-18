using Projekt_koncowy_AM_WS_JG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_koncowy_AM_WS_JG.View
{
    /// <summary>
    /// Logika interakcji dla klasy DodajEdytujPage.xaml
    /// </summary>
    public partial class DodajEdytujPage : UserControl
    {
        public event EventHandler Wyloguj;
        public event EventHandler WrocNacisniete;
        public event EventHandler Dodaj;
        public event EventHandler DodajOkladke;
        private Ksiazka _ksiazka;
        public event EventHandler<Ksiazka> Edytuj;
        public DodajEdytujPage(Ksiazka ksiazka)
        {
            InitializeComponent();
            if(ksiazka !=null)
            {
                TytulDoDodaj = ksiazka.Tytul;
                AutorDoDodaj = ksiazka.Autor;
                WydawnictwoDoDodaj = ksiazka.Wydawnictwo;
                GatunekDoDodaj = ksiazka.Gatunek;
                RokWydaniaDoDodaj = ksiazka.RokWydania;
                LiczbaStronDoDodaj = ksiazka.LiczbaStron;
                JezykDoDodaj = ksiazka.Jezyk;
                OpisDoDodaj = ksiazka.Opis;
                IDAutoraDoDodaj = ksiazka.IDAutora;
                IDWydawnictwaDoDodaj = ksiazka.IDWydawnictwa;
            }
        }
        public string TytulDoDodaj
        {
            get => TytulBox.Text;
            set => TytulBox.Text = value;
        }
        public string AutorDoDodaj
        {
            get => AutorBox.Text;
            set => AutorBox.Text = value;
        }
        public string WydawnictwoDoDodaj
        {
            get => WydawnictwoBox.Text;
            set => WydawnictwoBox.Text = value;
        }
        public string GatunekDoDodaj
        {
            get => GatunekBox.Text;
            set => GatunekBox.Text = value;
        }
        public string RokWydaniaDoDodaj
        {
            get => RokBox.Text;
            set => RokBox.Text = value;
        }
        public string LiczbaStronDoDodaj
        {
            get => StronyBox.Text;
            set => StronyBox.Text = value;
        }
        public string JezykDoDodaj
        {
            get => JezykBox.Text;
            set => JezykBox.Text = value;
        }
        public string OpisDoDodaj
        {
            get => OpisBox.Text;
            set => OpisBox.Text = value;

        }
        public string IDAutoraDoDodaj
        {
            get => IDAutoraBox.Text;
            set => IDAutoraBox.Text = value;
        }
        public string IDWydawnictwaDoDodaj
        {
            get => IDWydawnictwaBox.Text;
            set => IDWydawnictwaBox.Text = value;
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            _ksiazka = new Ksiazka()
            {
                Tytul = TytulDoDodaj,
                Autor = AutorDoDodaj,
                Wydawnictwo = WydawnictwoDoDodaj,
                Gatunek = GatunekDoDodaj,
                RokWydania = RokWydaniaDoDodaj,
                LiczbaStron = LiczbaStronDoDodaj,
                Jezyk = JezykDoDodaj,
                Opis = OpisDoDodaj,
                IDAutora = IDAutoraDoDodaj,
                IDWydawnictwa = IDWydawnictwaDoDodaj
            };
            Edytuj?.Invoke(this, _ksiazka);
        }

        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            Wyloguj?.Invoke(this, EventArgs.Empty);
        }

        private void DodajOkladke_Click(object sender, RoutedEventArgs e)
        {
            DodajOkladke?.Invoke(this, EventArgs.Empty);
        }
        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }
    }
}
