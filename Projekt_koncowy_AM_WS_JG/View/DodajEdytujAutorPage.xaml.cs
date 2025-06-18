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
    /// Logika interakcji dla klasy DodajEdytujAutorPage.xaml
    /// </summary>
    public partial class DodajEdytujAutorPage : UserControl
    {

        public event EventHandler WrocNacisniete;
        public event EventHandler<Autor> DodajAutora;
        private Autor _autor;
        public string ImieDoDodaj
        {
            get => ImieBox.Text;
            set => ImieBox.Text = value;
        }
        public string NazwiskoDoDodaj
        {
            get => NazwiskoBox.Text;
            set => NazwiskoBox.Text = value;
        }
        public string DataUrodzeniaDoDodaj
        {
            get => RokBox.Text;
            set => RokBox.Text = value;
        }
        public string NarodowoscDoDodaj
        {
            get => KrajBox.Text;
            set => KrajBox.Text = value;
        }
        public DodajEdytujAutorPage(Autor autor)
        {
            InitializeComponent();
            ImieBox.Text = autor.Imie;
            NazwiskoBox.Text = autor.Nazwisko;
            RokBox.Text = autor.DataUrodzenia;
            KrajBox.Text = autor.Narodowosc;
        }

        private void DodajAutora_Click(object sender, RoutedEventArgs e)
        {
            _autor = new Autor
            {
                Imie = ImieDoDodaj,
                Nazwisko = NazwiskoDoDodaj,
                DataUrodzenia = DataUrodzeniaDoDodaj,
                Narodowosc = NarodowoscDoDodaj
            };
            DodajAutora?.Invoke(this, _autor);
        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }
    }
}
