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
    /// Logika interakcji dla klasy AutorPage.xaml
    /// </summary>
    public partial class AutorPage : UserControl
    {
        public event EventHandler WrocNacisniete;
        public AutorPage(Autor autor)
        {
            InitializeComponent();
            UstawAutora(autor);
        }
        public void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }
        public void UstawAutora(Autor autor)
        {
            ImieNazwiskoText.Text = $"{autor.ImieNazwisko}";
            DataUrodzeniaText.Text = $"Data urodzenia: {autor.DataUrodzenia}";
            NarodowoscText.Text = $"Narodowość: {autor.Narodowosc}";
            if (autor.Ksiazki != null && autor.Ksiazki.Count > 0)
            {
                KsiazkiAutoraControl.ItemsSource = autor.Ksiazki;
            }
            else
            {
                KsiazkiAutoraControl.ItemsSource = null;
            }
        }
    }
}
