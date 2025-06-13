using Projekt_koncowy_AM_WS_JG.Presenter;
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
using System.Windows.Shapes;

namespace Projekt_koncowy_AM_WS_JG.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public event EventHandler Loguj;
        public MainView()
        {
            InitializeComponent();
        }
        public void LoadView(UserControl view)
        {
            MainContent.Content = view;
        }

        private void LogowanieKlikniete(object sender, EventArgs e)
        {
            Loguj?.Invoke(this, EventArgs.Empty);
        }
    }
}
