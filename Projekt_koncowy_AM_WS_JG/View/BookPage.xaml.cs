﻿using Projekt_koncowy_AM_WS_JG.Model;
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
using System.IO;


namespace Projekt_koncowy_AM_WS_JG.View
{
    /// <summary>
    /// Logika interakcji dla klasy BookPage.xaml
    /// </summary>
    public partial class BookPage : UserControl
    {
        public event EventHandler OpiniaNacisnieta;
        public event EventHandler WrocNacisniete;
        public event EventHandler<string> DodajDoListy;
        public event EventHandler<string> UsunZListy;
        string id_ksiazki_wybranej;
        public event EventHandler<string> PrzeniesNaWydawnictwo;
        public event EventHandler<string> PrzeniesNaAutora;
        private string wybraneWydawnictwo = "";
        private string wybranyAutorIndex = "";
        private string status_wybranej_ksiazki = "";
        private bool byloUstawianieStatusu;


        public string WybranyAutor
        {
            get => wybranyAutorIndex;
            set
            {
                wybranyAutorIndex = value;
            }
        }

        public string StatusWybranejKsiazki
        {
            get => status_wybranej_ksiazki;
            set
            {
                status_wybranej_ksiazki = value;
            }
        }
        public string WybraneWydawnictwo
        {
            get => wybraneWydawnictwo;
            set
            {
                wybraneWydawnictwo = value;
            }
        }
        public void UstawStatus(string nowystatus)
        {
            status_wybranej_ksiazki = nowystatus;
            byloUstawianieStatusu = true;
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == status_wybranej_ksiazki)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
                else
                {
                    StatusComboBox.SelectedIndex = 0;
                }
            }
        }
        public BookPage(Ksiazka ksiazka)
        {
            InitializeComponent();
            string rootFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            string okladkiFolder = System.IO.Path.Combine(rootFolder, "Okładki");
            string sciezkaDoOkladki = System.IO.Path.Combine(okladkiFolder, $"{ksiazka.IDKsiazki}.jpg");
            if (File.Exists(sciezkaDoOkladki))
            {
                OkladkaImage.Source = new BitmapImage(new Uri(sciezkaDoOkladki));
            }
            else
            {
                string sciezkaDomyslna = System.IO.Path.Combine(okladkiFolder, "1.jpg");
                OkladkaImage.Source = new BitmapImage(new Uri(sciezkaDomyslna));
            }

            TytulText.Text = ksiazka.Tytul;
            AutorText.Text = $"Autor: {ksiazka.Autor}";
            WydawnictwoText.Text = $"Wydawnictwo: {ksiazka.Wydawnictwo}";
            GatunekText.Text = $"Gatunek: {ksiazka.Gatunek}";
            OpisText.Text = ksiazka.Opis;
            RokWydaniaText.Text = $"Rok wydania: {ksiazka.RokWydania}";
            LiczbaStronText.Text = $"Liczba stron: {ksiazka.LiczbaStron}";
            JezykText.Text = $"Język: {ksiazka.Jezyk}";
            SredniaOcenaText.Text = $"Średnia ocena: {ksiazka.JakieOpinie?.Średnia_Ocena ?? "Brak ocen"} ({ksiazka.JakieOpinie?.Liczba_Ocen ?? "0"})";
            id_ksiazki_wybranej = ksiazka.IDKsiazki;
            wybraneWydawnictwo= ksiazka.IDWydawnictwa;
            wybranyAutorIndex= ksiazka.IDAutora;
            byloUstawianieStatusu = false;
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
        public string IDKsiazkiWybranej
        {
            get => id_ksiazki_wybranej;
            set => id_ksiazki_wybranej = value;
        }
        private void Autor_Click(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var ksiazka = textBlock?.DataContext as Ksiazka;
            wybranyAutorIndex = WybranyAutor ?? string.Empty;
            if (wybranyAutorIndex!=null)
            {
                PrzeniesNaAutora?.Invoke(this, wybranyAutorIndex);
            }
        }
        private void Wydawnictwo_Click(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var ksiazka = textBlock?.DataContext as Ksiazka;
            wybraneWydawnictwo = WybraneWydawnictwo?? string.Empty;
            if (wybraneWydawnictwo!= null)
            {
                PrzeniesNaWydawnictwo?.Invoke(this, wybraneWydawnictwo);
            }
        }

        private void DodajDoListy_Click(object sender, SelectionChangedEventArgs e)
        {
            if (byloUstawianieStatusu)
            {
                var selectedItem = StatusComboBox.SelectedItem as ComboBoxItem;
                string status = selectedItem?.Content.ToString() ?? string.Empty;
                string id_ksiazki = id_ksiazki_wybranej;
                string wynik = id_ksiazki + ";" + status;
                if (status == "Niedodane")
                {
                    UsunZListy?.Invoke(this, id_ksiazki);
                }
                else
                {
                    DodajDoListy?.Invoke(this, wynik);
                }
                return;
            }
            else
            {
                return;
            }
        }


    }
}
