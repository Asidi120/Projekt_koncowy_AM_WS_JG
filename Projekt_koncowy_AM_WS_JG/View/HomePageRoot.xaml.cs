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
        public HomePageRoot()
        {
            InitializeComponent();

        }
        public void UstawKsiazkiwroot(List<Ksiazka> ksiazki)
        {
            KsiazkiGrid.ItemsSource = ksiazki;
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
    }
}
