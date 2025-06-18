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
        public DodajEdytujPage(Ksiazka ksiazka)
        {
            InitializeComponent();
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

        }

        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DodajOkladke_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DodajAutora_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DodajWydawnictwo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }
    }
}
