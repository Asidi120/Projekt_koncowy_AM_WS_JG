using Projekt_koncowy_AM_WS_JG.Presenter;
using System.Configuration;
using System.Data;
using System.Windows;
using Projekt_koncowy_AM_WS_JG.Presenter;
using Projekt_koncowy_AM_WS_JG.Model;
using Projekt_koncowy_AM_WS_JG.View;

namespace Projekt_koncowy_AM_WS_JG
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var view = new MainView();
            var model = new Model.MainModel();
            var presenter = new UserPresenter(view, model);
            view.Show();
        }
    }

}
