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
        private Model.MainModel _model;
        public UserPresenter(MainView view, MainModel model)
        {
            _view = view;
            _model = model;
            _view.Loguj+= GdyLoguj;
        }
        public void GdyLoguj(object sender, EventArgs e)
        {
            _view.LoadView(new HomePage());
        }
    }    
}