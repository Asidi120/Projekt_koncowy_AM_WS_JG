using Projekt_koncowy_AM_WS_JG.Model;
using Projekt_koncowy_AM_WS_JG.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CustomMenu = Projekt_koncowy_AM_WS_JG.View.Menu;
using MySql.Data.MySqlClient;
using System.Windows;



namespace Projekt_koncowy_AM_WS_JG.Presenter
{
    public class UserPresenter
    {
        private readonly MainView _view;
        //private readonly Menu _menu;
        //private readonly HomePage _homePage;
        private RejestracjaPage rejestracja;
        private CustomMenu menu;
        private Model.MainModel _model;
        private HomePage homePage;
        public UserPresenter(MainView view, MainModel model)
        {
            _view = view;
            _model = model;
            menu = new CustomMenu();
            menu.Loguj += GdyLoguj;
            menu.Rejestracja += GdyRejestracja;
            _view.LoadView(menu);
            var autorzy = _model.Autorzy_lista();
            string tekstDoWyswietlenia = string.Join("\n", autorzy);
            //System.Windows.MessageBox.Show(tekstDoWyswietlenia, "Lista autorów");

        }
        public void GdyLoguj(object sender, EventArgs e)
        {
            if (_model.CzyEmailIstnieje(menu.EmailLogowanie) && menu.HasloLogowanie == _model.HasloUzytkownika(menu.EmailLogowanie))
            {
                homePage = new HomePage();
                homePage.Wyloguj += GdyWyloguj;
                _view.LoadView(homePage);
            }
            else
            {
                MessageBox.Show("Nieprawidłowy email lub hasło", "Błąd");
            }
        }

        public void GdyWyloguj(object sender, EventArgs e)
        {
            menu = new CustomMenu();
            menu.Loguj += GdyLoguj;
            menu.Rejestracja += GdyRejestracja;
            _view.LoadView(menu);
        }
        public void GdyRejestracja(object sender, EventArgs e)
        {
            rejestracja = new RejestracjaPage();
            _view.LoadView(rejestracja);
            rejestracja.RejestracjaGotowa += GdyRejestracjaGotowa;
        }
        public void GdyRejestracjaGotowa(object sender, EventArgs e)
        {
            if(!_model.CzyUzytkownikIstnieje(rejestracja.NazwaUżytkownika))
            {
                if (!_model.CzyEmailIstnieje(rejestracja.Email))
                {
                    var homePage = new HomePage();
                    homePage.Wyloguj += GdyWyloguj;
                    _view.LoadView(homePage);

                    _model.DodajDoBazyUzytkownika(rejestracja.NazwaUżytkownika, rejestracja.Haslo, rejestracja.Email);
                }
                else
                {
                    MessageBox.Show("Adres email już istnieje", "Nieprawidłowy adres email");
                }
            }
            else
            {
                MessageBox.Show("Nazwa użytkownika już istnieje", "Nieprawidłowa nazwa użytkownika");
            }
            
        }

       
    }
}