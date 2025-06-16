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
    /// Logika interakcji dla klasy HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public event EventHandler Wyloguj;
        public event EventHandler<Ksiazka> PrzeniesNaTytul;
        public event EventHandler Wyszukuje;
        public event EventHandler<string> PrzeniesNaAutora;
        private string searchText;
        private string wybranyAutorIndex = "";

        public HomePage()
        {
            InitializeComponent();

        }
        private void WylogujKlikniete(object sender, EventArgs e)
        {
            Wyloguj?.Invoke(this, EventArgs.Empty);
        }
        public void UstawKsiazka(List<Ksiazka> ksiazki)
        {
            KsiazkiControl.ItemsSource = ksiazki;
            ChcePrzeczytacControl.ItemsSource = ksiazki;
            PrzeczytaneControl.ItemsSource = ksiazki;
            WTrakcieControl.ItemsSource = ksiazki;
            PorzuconeControl.ItemsSource = ksiazki;
            KsiazkiControl.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
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

    }
}
