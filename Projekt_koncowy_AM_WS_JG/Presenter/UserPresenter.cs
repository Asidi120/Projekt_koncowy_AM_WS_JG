using MySql.Data.MySqlClient;
using Projekt_koncowy_AM_WS_JG.Model;
using Projekt_koncowy_AM_WS_JG.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CustomMenu = Projekt_koncowy_AM_WS_JG.View.Menu;



namespace Projekt_koncowy_AM_WS_JG.Presenter
{
    public class UserPresenter
    {
        private readonly MainView _view;
        private RejestracjaPage rejestracja;
        private CustomMenu menu;
        private Model.MainModel _model;
        private HomePage homePage;
        private HomePageRoot homePageRoot;
        private BookPage bookPage;
        private RatePage ratePage;
        public UserPresenter(MainView view, MainModel model)
        {
            _view = view;
            _model = model;
            menu = new CustomMenu();
            menu.Loguj += GdyLoguj;
            menu.Rejestracja += GdyRejestracja;
            _view.LoadView(menu);
        }
        public void GdyLoguj(object sender, EventArgs e)
        {
            //string hasloZBazy = _model.HasloUzytkownika(menu.EmailLogowanie);
            //if (_model.CzyEmailIstnieje(menu.EmailLogowanie) && BCrypt.Net.BCrypt.Verify(menu.HasloLogowanie, hasloZBazy))
            //{
            //    if (_model.CzyJestRoot(menu.EmailLogowanie))
            //    {
            //        homePageRoot = new HomePageRoot();
            //        homePageRoot.Wyloguj += GdyWyloguj;
            //        _view.LoadView(homePageRoot);
            //    }
            //    else
            //    {
                    homePage = new HomePage();
                    homePage.Wyloguj += GdyWyloguj;
                    _view.LoadView(homePage);
                    ZaladujKsiazki();
                     homePage.PrzeniesNaTytul += PrzeniesNaTytul;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Nieprawidłowy email lub hasło", "Błąd");
            //}
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
            if (!_model.CzyUzytkownikIstnieje(rejestracja.NazwaUżytkownika))
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
        public void PrzeniesNaTytul(object sender, Ksiazka ksiazka)
        {
            bookPage = new BookPage(ksiazka);
            bookPage.OpiniaNacisnieta += GdyOpiniaNacisnieta;
            _view.LoadView(bookPage);
        }
        private void GdyOpiniaNacisnieta(object? sender, EventArgs e)
        {
            ratePage = new RatePage();
            _view.LoadView(ratePage);
        }
        private void ZaladujKsiazki()
        {
            _model.ZaladujKsiazkiZBazy();
            homePage.UstawKsiazka(_model.ksiazki);
        }
    }
}