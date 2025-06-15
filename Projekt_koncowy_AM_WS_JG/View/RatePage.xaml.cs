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
    /// Logika interakcji dla klasy RatePage.xaml
    /// </summary>
    public partial class RatePage : UserControl
    {
        public event EventHandler Wroc;
        public event EventHandler WyslijOpinie;
        public RatePage()
        {
            InitializeComponent();
        }

        private void WyslijOpinie_Click(object sender, RoutedEventArgs e)
        {
            if (OcenaComboBox.SelectedIndex != -1)
            {
                WyslijOpinie?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Uzupełnij ocenę", "Nieprawidłowa opinia");
            }
        }
   

        private void WrocNacisniete(object sender, RoutedEventArgs e)
        {
            Wroc?.Invoke(this, EventArgs.Empty);
        }

        public string RecenzjaWystawiona
        {
            get => OpiniaTextBox.Text;
            set => OpiniaTextBox.Text = value;
        }

        public string OcenaWystawiona
        {
            get => OcenaComboBox.Text;
            set => OcenaComboBox.Text = value;
        }
    }
}
