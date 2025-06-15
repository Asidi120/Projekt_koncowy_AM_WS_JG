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
        public HomePage()
        {
            InitializeComponent();

        }
        private void WylogujKlikniete(object sender, EventArgs e)
        {
            Wyloguj?.Invoke(this, EventArgs.Empty);
        }
        public void UstawKsiazka(IEnumerable<Ksiazka> ksiazki) 
        {
            KsiazkiControl.ItemsSource = ksiazki;
            KsiazkiControl.ItemTemplate = (DataTemplate)this.Resources["KsiazkaTemplate"];
        }

        private void Tytul_Click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
