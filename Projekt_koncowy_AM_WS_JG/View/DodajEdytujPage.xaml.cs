using Microsoft.Win32;
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
        public event EventHandler<string> DodajOkladke;
        private Ksiazka _ksiazka;
        public event EventHandler<Ksiazka> Edytuj;
        public DodajEdytujPage(Ksiazka ksiazka)
        {
            InitializeComponent();
            if(ksiazka !=null)
            {
                _ksiazka = ksiazka;
                TytulDoDodaj = ksiazka.Tytul;
                GatunekDoDodaj = ksiazka.Gatunek;
                RokWydaniaDoDodaj = ksiazka.RokWydania;
                LiczbaStronDoDodaj = ksiazka.LiczbaStron;
                JezykDoDodaj = ksiazka.Jezyk;
                OpisDoDodaj = ksiazka.Opis;
                IDAutoraDoDodaj = ksiazka.IDAutora;
                IDWydawnictwaDoDodaj = ksiazka.IDWydawnictwa;
            }
            else
            {
                _ksiazka=new Ksiazka();
            }
        }
        public string TytulDoDodaj
        {
            get => TytulBox.Text;
            set => TytulBox.Text = value;
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

        //private void Dodaj_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_ksiazka == null)
        //    {
        //        _ksiazka = new Ksiazka
        //        {
        //            Tytul = TytulDoDodaj,
        //            Gatunek = (GatunekBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
        //            LiczbaStron = LiczbaStronDoDodaj,
        //            RokWydania = RokWydaniaDoDodaj,
        //            Jezyk = JezykDoDodaj,
        //            IDAutora = IDAutoraDoDodaj,
        //            IDWydawnictwa = IDWydawnictwaDoDodaj,
        //            Opis = OpisDoDodaj
        //        };
        //    }
        //    else
        //    {
        //        _ksiazka.Tytul = TytulDoDodaj;
        //        _ksiazka.Gatunek = (GatunekBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        //        _ksiazka.LiczbaStron = LiczbaStronDoDodaj;
        //        _ksiazka.RokWydania = RokWydaniaDoDodaj;
        //        _ksiazka.Jezyk = JezykDoDodaj;
        //        _ksiazka.IDAutora = IDAutoraDoDodaj;
        //        _ksiazka.IDWydawnictwa = IDWydawnictwaDoDodaj;
        //        _ksiazka.Opis = OpisDoDodaj;
        //    }

        //    Edytuj?.Invoke(this, _ksiazka);
        //}
        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TytulDoDodaj) ||string.IsNullOrWhiteSpace(JezykDoDodaj) ||string.IsNullOrWhiteSpace(IDAutoraDoDodaj) || string.IsNullOrWhiteSpace(IDWydawnictwaDoDodaj) ||
                (GatunekBox.SelectedItem == null && string.IsNullOrWhiteSpace(GatunekDoDodaj)) ||
                !int.TryParse(LiczbaStronDoDodaj, out int strony) || strony <= 0)
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione, gatunek musi być wybrany, a liczba stron musi być większa od zera.",
                                "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(RokWydaniaDoDodaj))
            {
                MessageBox.Show("Rok wydania nie może być pusty.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(RokWydaniaDoDodaj, out _))
            {
                MessageBox.Show("Rok wydania musi być liczbą.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_ksiazka == null)
                _ksiazka = new Ksiazka();

            _ksiazka.Tytul = TytulDoDodaj;
            _ksiazka.Gatunek = (GatunekBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? GatunekDoDodaj;
            _ksiazka.LiczbaStron = LiczbaStronDoDodaj;
            _ksiazka.RokWydania = RokWydaniaDoDodaj;
            _ksiazka.Jezyk = JezykDoDodaj;
            _ksiazka.IDAutora = IDAutoraDoDodaj;
            _ksiazka.IDWydawnictwa = IDWydawnictwaDoDodaj;
            _ksiazka.Opis = OpisDoDodaj;

            Edytuj?.Invoke(this, _ksiazka);
        }


        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            Wyloguj?.Invoke(this, EventArgs.Empty);
        }

        private void DodajOkladke_Click(object sender, RoutedEventArgs e)
        {
            string id_ksiazki = _ksiazka?.IDKsiazki ?? string.Empty;
            if (string.IsNullOrEmpty(id_ksiazki))
            {
                MessageBox.Show("Nie można dodać okładki, ponieważ nie stworzono jeszcze tej książki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                DodajOkladke?.Invoke(this, id_ksiazki);
            }
        }
        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }
        public string PobierzSciezkeDoPliku()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Obrazy (*.jpg)|*.jpg";
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        public void PokazKomunikat(string wiadomosc)
        {
            MessageBox.Show(wiadomosc);
        }
    }
}
