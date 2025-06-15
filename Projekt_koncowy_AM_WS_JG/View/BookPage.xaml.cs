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
using Projekt_koncowy_AM_WS_JG.Model;

namespace Projekt_koncowy_AM_WS_JG.View
{
    /// <summary>
    /// Logika interakcji dla klasy BookPage.xaml
    /// </summary>
    public partial class BookPage : UserControl
    {
        public event EventHandler OpiniaNacisnieta;
        public event EventHandler WrocNacisniete;
        public BookPage(Ksiazka ksiazka)
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(ksiazka.Okladka))
            {
                OkladkaImage.Source = new BitmapImage(new Uri(ksiazka.Okladka, UriKind.Absolute));
            }
            else
            {
                OkladkaImage.Source = new BitmapImage(new Uri("Images/brak_okladki.png", UriKind.Relative));
            }

            TytulText.Text = ksiazka.Tytul;
            AutorText.Text = $"Autor: {ksiazka.Autor}";
            WydawnictwoText.Text = $"Wydawnictwo: {ksiazka.Wydawnictwo}";
            GatunekText.Text = $"Gatunek: {ksiazka.Gatunek}";
            OpisText.Text = ksiazka.Opis;
            RokWydaniaText.Text = $"Rok wydania: {ksiazka.RokWydania}";
            LiczbaStronText.Text = $"Liczba stron: {ksiazka.LiczbaStron}";
            JezykText.Text = $"Język: {ksiazka.Jezyk}";
            SredniaOcenaText.Text = $"Średnia ocena: {ksiazka.JakieOpinie?.Średnia_Ocena ?? "Brak ocen"} ({ksiazka.JakieOpinie?.Liczba_Ocen ?? "0"} ocen)";

            // TU USTAWIAMY OPINIE
            if (ksiazka?.JakieOpinie?.Lista_Opinii != null)
            {
                OpinieControl.ItemsSource = ksiazka.JakieOpinie.Lista_Opinii;
            }
        }

        public void UstawOpinieZKsiazki(Ksiazka ksiazka)
        {
            if (ksiazka?.JakieOpinie?.Lista_Opinii != null)
            {
                OpinieControl.ItemsSource = ksiazka.JakieOpinie.Lista_Opinii;
            }
        }
        private void NapiszOpinie_Click(object sender, RoutedEventArgs e)
        {
            OpiniaNacisnieta?.Invoke(this, EventArgs.Empty);
        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }
    }
}
