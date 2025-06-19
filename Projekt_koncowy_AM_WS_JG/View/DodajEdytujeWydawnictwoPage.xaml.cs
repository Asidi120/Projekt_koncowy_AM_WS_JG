using Projekt_koncowy_AM_WS_JG.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Logika interakcji dla klasy DodajEdytujeWydawnictwoPage.xaml
    /// </summary>
    public partial class DodajEdytujeWydawnictwoPage : UserControl
    {

        public event EventHandler WrocNacisniete;
        public event EventHandler<Wydawnictwo> DodajWydawnictwo;
        private Wydawnictwo _wydawnictwo;
        public string NazwaWydawnictwa
        {
            get => NazwaBox.Text;
            set => NazwaBox.Text = value;
        }
        public string KrajZalozeniaWydawnictwa
        {
            get => KrajBox.Text;
            set => KrajBox.Text = value;
        }
        public string RokZalozeniaWydawnictwa
        {
            get => RokBox.Text;
            set => RokBox.Text = value;
        }
        public DodajEdytujeWydawnictwoPage(Wydawnictwo wydawnictwo)
        {
            InitializeComponent();
            if (wydawnictwo != null)
            {
                _wydawnictwo = wydawnictwo;
                NazwaWydawnictwa= wydawnictwo.Nazwa;
                KrajZalozeniaWydawnictwa = wydawnictwo.Kraj_zalozenia;
                RokZalozeniaWydawnictwa = wydawnictwo.Rok_zalozenia;
            }
            else
            {
                _wydawnictwo= new Wydawnictwo();
            }
        }

        private void DodajWydawnictwo_Click(object sender, RoutedEventArgs e)
        {
            if(_wydawnictwo == null) 
            {
                _wydawnictwo = new Wydawnictwo
                {
                    Nazwa = NazwaWydawnictwa,
                    Kraj_zalozenia = KrajZalozeniaWydawnictwa,
                    Rok_zalozenia = RokZalozeniaWydawnictwa
                };
            }
            else
            {
                _wydawnictwo.Nazwa = NazwaWydawnictwa;
                _wydawnictwo.Kraj_zalozenia = KrajZalozeniaWydawnictwa;
                _wydawnictwo.Rok_zalozenia = RokZalozeniaWydawnictwa;

            }
            DodajWydawnictwo?.Invoke(this, _wydawnictwo);
        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }
    }
}
