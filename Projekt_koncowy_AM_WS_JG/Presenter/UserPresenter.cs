using Projekt_koncowy_AM_WS_JG.Model;
using Projekt_koncowy_AM_WS_JG.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_koncowy_AM_WS_JG.Presenter
{
    public class UserPresenter
    {
        private readonly MainView _view;
        //private readonly Menu _menu;
        //private readonly HomePage _homePage;

        private Model.MainModel _model;
        public UserPresenter(MainView view, MainModel model)
        {
            _view = view;
            _model = model;

            //_menu = new Menu();
            //_homePage = new HomePage();

            //_view.LoadView(_menu);
            //_menu.Loguj += GdyLoguj;
            //_homePage.Wyloguj += GdyWyloguj;

            var menu = new Menu();
            menu.Loguj += GdyLoguj;

            _view.LoadView(menu);

        }
        public void GdyLoguj(object sender, EventArgs e)
        {
            var homePage = new HomePage(); 
            homePage.Wyloguj += GdyWyloguj;
            _view.LoadView(homePage);
        }

        public void GdyWyloguj(object sender, EventArgs e)
        {
            var menu = new Menu();              
            menu.Loguj += GdyLoguj;
            _view.LoadView(menu);
        }
    }    
}