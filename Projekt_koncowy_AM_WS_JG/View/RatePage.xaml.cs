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

        public int OcenaWystawiona { get; private set; }
        public string RecenzjaWystawiona => OpiniaTextBox.Text;
        public RatePage()
        {
            InitializeComponent();
        }
        private void Star_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button starButton && int.TryParse(starButton.Tag.ToString(), out int rating))
            {
                OcenaWystawiona = rating;
                UpdateStars(rating);
                RatingText.Text = $"{rating}/10";
            }
        }

        private void UpdateStars(int selectedRating)
        {
            for (int i = 1; i <= 10; i++)
            {
                if (FindName($"Star{i}") is Button star)
                {
                    star.Foreground = i <= selectedRating ? Brushes.Gold : Brushes.LightGray;
                }
            }
        }
        private void WyslijOpinie_Click(object sender, RoutedEventArgs e)
        {
            if (OcenaWystawiona== 0)
            {
                MessageBox.Show("Proszę wybrać ocenę od 1 do 10 gwiazdek", "Brak oceny");
                return;
            }
            else
            {
                WyslijOpinie?.Invoke(this, e);
            }
        }
   

        private void WrocNacisniete(object sender, RoutedEventArgs e)
        {
            Wroc?.Invoke(this, EventArgs.Empty);
        }
    }
}
