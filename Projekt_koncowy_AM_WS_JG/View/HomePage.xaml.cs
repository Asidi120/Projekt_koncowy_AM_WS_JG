﻿using System;
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
    /// Logika interakcji dla klasy HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public event EventHandler Wyloguj;
        public event EventHandler<Ksiazka> PrzeniesNaTytul;
        public event EventHandler Wyszukuje;
        public event EventHandler<string> PrzeniesNaAutora;
        public event EventHandler<string> ZmianaPlci;
        private string searchText;
        private string wybranyAutorIndex = "";
        public bool byloUstawianiePlci = false;
        public HomePage()
        {
            InitializeComponent();
            byloUstawianiePlci = false;
        }
        private void WylogujKlikniete(object sender, EventArgs e)
        {
            Wyloguj?.Invoke(this, EventArgs.Empty);
        }
        public void UstawKsiazkaNajpopularniejsze(List<Ksiazka> ksiazki)
        {
            KsiazkiControl.ItemsSource = ksiazki;
            KsiazkiControl.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
        }

        public void UstawKsiazkaNajnowsze(List<Ksiazka> ksiazki)
        {
            KsiazkiControl2.ItemsSource = ksiazki;
            KsiazkiControl2.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
        }

        public void UstawKsiazkaChcePrzeczytac(List<Ksiazka> ksiazki)
        {
            ChcePrzeczytacControl.ItemsSource = ksiazki;
            ChcePrzeczytacControl.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
        }

        public void UstawKsiazkaPrzeczytane(List<Ksiazka> ksiazki)
        {
            PrzeczytaneControl.ItemsSource = ksiazki;
            PrzeczytaneControl.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
        }

        public void UstawKsiazkaWTrakcie(List<Ksiazka> ksiazki)
        {
            WTrakcieControl.ItemsSource = ksiazki;
            WTrakcieControl.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
        }
        public void UstawKsiazkaPorzucone(List<Ksiazka> ksiazki)
        {
            PorzuconeControl.ItemsSource = ksiazki;
            PorzuconeControl.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
        }
        private void Tytul_Click(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var ksiazka = textBlock?.DataContext as Ksiazka;

            if (ksiazka != null)
            {
                PrzeniesNaTytul?.Invoke(this, ksiazka);
            }
        }
        private void ScrollLeft_Click(object sender, RoutedEventArgs e)
        {
            double offset = BookScrollViewer.HorizontalOffset - 150;
            if (offset < 0) offset = 0;
            BookScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollRight_Click(object sender, RoutedEventArgs e)
        {
            double offset = BookScrollViewer.HorizontalOffset + 150;
            BookScrollViewer.ScrollToHorizontalOffset(offset);
        }
        public void PokazKsiazki(List<Ksiazka> ksiazki)
        {
            SearchResultsControl.ItemsSource = ksiazki;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchText = SearchTextBox.Text.ToLowerInvariant().Trim();
            Wyszukuje?.Invoke(this, EventArgs.Empty);
        }
        public string SearchText
        {
            get => searchText;
            set => searchText = value;
        }
        public string Nick
        {
            get => NickText.Text;
            set => NickText.Text = value;
        }
        public string Email
        {
            get => EmailText.Text;
            set => EmailText.Text = value;
        }
        public string Plec
        {
            get => PlecText.Text;
            set => PlecText.Text = value;
        }
        public string DataZalozenia
        {
            get => DataDodText.Text;
            set => DataDodText.Text = value;
        }
        public string WybranyAutor
        {
            get => wybranyAutorIndex;
            set
            {
                wybranyAutorIndex = value;
            }
        }
        private void ScrollLeft_ChcePrzeczytac(object sender, RoutedEventArgs e)
        {
            double offset = ChcePrzeczytacScrollViewer.HorizontalOffset - 150;
            if (offset < 0) offset = 0;
            ChcePrzeczytacScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollRight_ChcePrzeczytac(object sender, RoutedEventArgs e)
        {
            double offset = ChcePrzeczytacScrollViewer.HorizontalOffset + 150;
            ChcePrzeczytacScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollLeft_Przeczytane(object sender, RoutedEventArgs e)
        {
            double offset = PrzeczytaneScrollViewer.HorizontalOffset - 150;
            if (offset < 0) offset = 0;
            PrzeczytaneScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollRight_Przeczytane(object sender, RoutedEventArgs e)
        {
            double offset = PrzeczytaneScrollViewer.HorizontalOffset + 150;
            PrzeczytaneScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollLeft_WTrakcie(object sender, RoutedEventArgs e)
        {
            double offset = WTrakcieScrollViewer.HorizontalOffset - 150;
            if (offset < 0) offset = 0;
            WTrakcieScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollRight_WTrakcie(object sender, RoutedEventArgs e)
        {
            double offset = WTrakcieScrollViewer.HorizontalOffset + 150;
            WTrakcieScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollLeft_Porzucone(object sender, RoutedEventArgs e)
        {
            double offset = PorzuconeScrollViewer.HorizontalOffset - 150;
            if (offset < 0) offset = 0;
            PorzuconeScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void ScrollRight_Porzucone(object sender, RoutedEventArgs e)
        {
            double offset = PorzuconeScrollViewer.HorizontalOffset + 150;
            PorzuconeScrollViewer.ScrollToHorizontalOffset(offset);
        }

        private void HorizontalScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Przekieruj zdarzenie do nadrzędnego ScrollViewer-a (np. ten który zawija cały TabItem)
            e.Handled = true;

            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = sender
            };

            // Znajdź główny pionowy ScrollViewer – zamień na swój, np. "MainScrollViewer"
            MainScrollViewer.RaiseEvent(eventArg);
        }

        private void Autor_Click(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var ksiazka = textBlock?.DataContext as Ksiazka;
            WybranyAutor = ksiazka?.IDAutora ?? string.Empty;
            if (ksiazka != null && ksiazka.Autor != null)
            {
                PrzeniesNaAutora?.Invoke(this, WybranyAutor);
            }
        }

        private void ScrollRight1_Click(object sender, RoutedEventArgs e)
        {
            double offset = BookScrollViewer2.HorizontalOffset + 150;
            BookScrollViewer2.ScrollToHorizontalOffset(offset);
        }

        private void ScrollLeft1_Click(object sender, RoutedEventArgs e)
        {
            double offset = BookScrollViewer2.HorizontalOffset - 150;
            if (offset < 0) offset = 0;
            BookScrollViewer2.ScrollToHorizontalOffset(offset);
        }
        public void ZmienPlec(string nowaplec)
        {
            bool znaleziono = false;

            foreach (ComboBoxItem item in PlecComboBox.Items)
            {
                if (item.Content.ToString().ToLower() == nowaplec.ToLower())
                {
                    PlecComboBox.SelectedItem = item;
                    PlecText.Text = nowaplec.Substring(0,1).ToUpper()+nowaplec.Substring(1,nowaplec.Length-1).ToLower();
                    znaleziono = true;
                    break;
                }
            }

            if (!znaleziono)
            {
                PlecComboBox.SelectedIndex = 2;
            }
            byloUstawianiePlci = true;
        }
        private void ProceduraZmiantPlci_Click(object sender, SelectionChangedEventArgs e)
        {
            if (byloUstawianiePlci)
            {
                var selectedItem = PlecComboBox.SelectedItem as ComboBoxItem;
                string wynik = selectedItem?.Content.ToString() ?? string.Empty;
                ZmianaPlci?.Invoke(this, wynik);
            }
            else
            {
                return;
            }
        }

        public int KtoraZakladka
        {
            get => Zakladki.SelectedIndex;
            set => Zakladki.SelectedIndex = value;
        }

    }
}
