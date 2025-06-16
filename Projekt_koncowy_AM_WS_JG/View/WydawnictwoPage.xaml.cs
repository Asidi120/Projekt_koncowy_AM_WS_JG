using System;
using System.Windows;
using System.Windows.Controls;
using Projekt_koncowy_AM_WS_JG.Model;

namespace Projekt_koncowy_AM_WS_JG.View
{
    /// <summary>
    /// Logika interakcji dla klasy WydawnictwoPage.xaml
    /// </summary>
    public partial class WydawnictwoPage : UserControl
    {
        public event EventHandler WrocNacisniete;

        public WydawnictwoPage(Wydawnictwo wydawnictwo)
        {
            InitializeComponent();
            UstawWydawnictwo(wydawnictwo);
        }

        public void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }

        public void UstawWydawnictwo(Wydawnictwo wydawnictwo)
        {
            NazwaText.Text = $"{wydawnictwo.Nazwa}";
            KrajText.Text = $"Kraj założenia: {wydawnictwo.Kraj_zalozenia}";
            RokText.Text = $"Rok założenia: {wydawnictwo.Rok_zalozenia}";

            if (wydawnictwo.Ksiazki != null && wydawnictwo.Ksiazki.Count > 0)
            {
                KsiazkiWydawnictwaControl.ItemsSource = wydawnictwo.Ksiazki;
            }
            else
            {
                KsiazkiWydawnictwaControl.ItemsSource = null;
            }
        }
    }
}
