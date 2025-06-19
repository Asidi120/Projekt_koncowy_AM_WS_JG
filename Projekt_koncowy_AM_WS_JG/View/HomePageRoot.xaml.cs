using MySql.Data.MySqlClient;
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
using Projekt_koncowy_AM_WS_JG.Model;

namespace Projekt_koncowy_AM_WS_JG.View
{
    /// <summary>
    /// Logika interakcji dla klasy HomePageRoot.xaml
    /// </summary>
    public partial class HomePageRoot : UserControl
    {
        public event EventHandler Wyloguj;
        public event EventHandler<Ksiazka> Edytuj;
        public event EventHandler<Ksiazka> Dodaj;
        public event EventHandler<string> Usun;
        public event EventHandler<Wydawnictwo> DodajWydawnictwo;
        public event EventHandler<Wydawnictwo> EdytujWydawnictwo;
        public event EventHandler<string> UsunWydawnictwo;
        public event EventHandler<Autor> DodajAutora;
        public event EventHandler<Autor> EdytujAutora;
        public event EventHandler<string> UsunAutora;
        public event EventHandler<string> UsunUzytkownika;
        public event EventHandler<string> ZmienRoot;
        public event Action<int, int> UsunStatus;

        private InformacjeZBazy _baza;

        public HomePageRoot()
        {
            InitializeComponent();

        }
        public void UstawKsiazkiwroot(InformacjeZBazy baza)
        {
            KsiazkiGrid.ItemsSource = baza.Ksiazki;
            AutorzyGrid.ItemsSource = baza.Autorzy;
            WydawnictwaGrid.ItemsSource = baza.Wydawnictwa;
            StatusGrid.ItemsSource = baza.Statusy;
            OpinieGrid.ItemsSource = baza.Opinie;
            UzytkownicyGrid.ItemsSource = baza.Uzytkownicy;
            _baza = baza;
        }

        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            Wyloguj?.Invoke(this, EventArgs.Empty);
        }
        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            Dodaj?.Invoke(this, null);
        }
        private void Edytuj_Click(object sender, RoutedEventArgs e)
        {
            if (KsiazkiGrid.SelectedItem is Ksiazka wybrana)
            {
                Edytuj?.Invoke(this, wybrana);
            }
            else
            {
                MessageBox.Show("Wybierz książkę do edycji.");
            }
        }
        public void PrzelaczNaZakladkeWydawnictwa()
        {
            MainTabControl.SelectedItem = WydawnictwaTab;
        }
        public void PrzelaczNaZakladkeAutorzy()
        {
            MainTabControl.SelectedItem = AutorzyTab;
        }
        public void PrzelaczNaZakladkeKsiazki()
        {
            MainTabControl.SelectedItem = KsiazkiTab;
        }
        public void PrzelaczNaZakladkeUzytkownicy()
        {
            MainTabControl.SelectedItem = UzytkownicyTab;
        }
        public void PrzelaczNaZakladkeOpinie()
        {
            MainTabControl.SelectedItem = OpinieTab;
        }
        public void PrzelaczNaZakladkeStatus()
        {
            MainTabControl.SelectedItem = StatusTab;
        }

        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            if (KsiazkiGrid.SelectedItem is Ksiazka wybrana)
            {
                Usun?.Invoke(this, wybrana.IDKsiazki);
            }
            else
            {
                MessageBox.Show("Wybierz książkę do usunięcia.");
            }
        }

        private void Dodaj_Wydawnictwo_Click(object sender, RoutedEventArgs e)
        {
            DodajWydawnictwo?.Invoke(this, null);
        }

        private void Edytuj_Wydawnictwo_Click(object sender, RoutedEventArgs e)
        {
            if (WydawnictwaGrid.SelectedItem is Wydawnictwo wydawnictwo)
            {
                EdytujWydawnictwo?.Invoke(this, wydawnictwo);
            }
            else
            {
                MessageBox.Show("Wybierz wydawnictwo do edycji.");
            }
        }

        private void Usun_Wydawnictwo_Click(object sender, RoutedEventArgs e)
        {
            if (WydawnictwaGrid.SelectedItem is Wydawnictwo wydawnictwo)
            {
                UsunWydawnictwo?.Invoke(this, wydawnictwo.IDWydawnictwa);
            }
            else
            {
                MessageBox.Show("Wybierz wydawnictwo do usunięcia.");
            }
        }

        private void Dodaj_Autora_Click(object sender, RoutedEventArgs e)
        {
            DodajAutora?.Invoke(this, null);
        }

        private void Edytuj_Autora_Click(object sender, RoutedEventArgs e)
        {
            if (AutorzyGrid.SelectedItem is Autor autor)
            {
                EdytujAutora?.Invoke(this, autor);
            }
            else
            {
                MessageBox.Show("Wybierz autora do edycji.");
            }
        }

        private void Usun_Autora_Click(object sender, RoutedEventArgs e)
        {
            if (AutorzyGrid.SelectedItem is Autor autor)
            {
                UsunAutora?.Invoke(this, autor.IDAutora);
            }
            else
            {
                MessageBox.Show("Wybierz autora do usunięcia.");
            }
        }

        private void Zmien_root_Click(object sender, RoutedEventArgs e)
        {
            if (UzytkownicyGrid.SelectedItem is Uzytkownik uzytkownik)
            {
                ZmienRoot?.Invoke(this, uzytkownik.IDUzytkownika);
            }
            else
            {
                MessageBox.Show("Wybierz użytkownika do zmiany pola root.");
            }
        }


        private void Usun_uzytkownika_Click(object sender, RoutedEventArgs e)
        {
            if (UzytkownicyGrid.SelectedItem is Uzytkownik uzytkownik)
            {
                UsunUzytkownika?.Invoke(this, uzytkownik.IDUzytkownika.ToString());
            }
            else
            {
                MessageBox.Show("Wybierz użytkownika do usunięcia.");
            }
        }

        private void Usun_Status_Click(object sender, RoutedEventArgs e)
        {
            if (StatusGrid.SelectedItem is Status status)
            {
                if (int.TryParse(status.IDUzytkownika, out int idUzytkownika) &&
                    int.TryParse(status.IDKsiazki, out int idKsiazki))
                {
                    UsunStatus?.Invoke(idUzytkownika, idKsiazki);
                }
                else
                {
                    MessageBox.Show("Niepoprawne ID użytkownika lub książki.");
                }
            }
            else
            {
                MessageBox.Show("Wybierz status do usunięcia.");
            }
        }



    }
}
