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
            string hasloZBazy = _model.HasloUzytkownika(menu.EmailLogowanie);
            if (_model.CzyEmailIstnieje(menu.EmailLogowanie) && BCrypt.Net.BCrypt.Verify(menu.HasloLogowanie, hasloZBazy))
            {
                if (_model.CzyJestRoot(menu.EmailLogowanie))
                {
                    homePageRoot = new HomePageRoot();
                    homePageRoot.Wyloguj += GdyWyloguj;
                    _view.LoadView(homePageRoot);
                }
                else
                {
                    homePage = new HomePage();
                    homePage.Wyloguj += GdyWyloguj;
                    _view.LoadView(homePage);
                    ZaladujKsiazki();
                }
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
        private void ZaladujKsiazki()
        {
            var ksiazki = new List<Ksiazka>
{
    new Ksiazka { Tytul = "Metro 2033", Autor = "Dmitry Glukhovsky", Okladka = "Images/metro.jpg", Opis = "Postapokaliptyczna powieść o życiu w moskiewskim metrze." },
    new Ksiazka { Tytul = "Wiedźmin", Autor = "Andrzej Sapkowski", Okladka = "Images/wiedzmin.jpg", Opis = "Saga o białowłosym łowcy potworów – Geralcie z Rivii." },
    new Ksiazka { Tytul = "Hobbit", Autor = "J.R.R. Tolkien", Okladka = "Images/hobbit.jpg", Opis = "Przygoda Bilba Bagginsa w świecie Śródziemia." },
    new Ksiazka { Tytul = "1984", Autor = "George Orwell", Okladka = "Images/1984.jpg", Opis = "Wizja totalitarnego społeczeństwa i Wielkiego Brata." },
    new Ksiazka { Tytul = "Zbrodnia i kara", Autor = "Fiodor Dostojewski", Okladka = "Images/zbrodnia.jpg", Opis = "Psychologiczna opowieść o winie, karze i odkupieniu." },
    new Ksiazka { Tytul = "Gra o tron", Autor = "George R.R. Martin", Okladka = "Images/graotron.jpg", Opis = "Pierwszy tom sagi Pieśń lodu i ognia – walka o Żelazny Tron." },
    new Ksiazka { Tytul = "Duma i uprzedzenie", Autor = "Jane Austen", Okladka = "Images/duma.jpg", Opis = "Miłosna opowieść osadzona w realiach XVIII-wiecznej Anglii." },
    new Ksiazka { Tytul = "Harry Potter i Kamień Filozoficzny", Autor = "J.K. Rowling", Okladka = "Images/hp1.jpg", Opis = "Pierwsza część przygód młodego czarodzieja." },
    new Ksiazka { Tytul = "Lśnienie", Autor = "Stephen King", Okladka = "Images/lsnienie.jpg", Opis = "Mrożąca krew w żyłach historia w nawiedzonym hotelu." },
    new Ksiazka { Tytul = "Solaris", Autor = "Stanisław Lem", Okladka = "Images/solaris.jpg", Opis = "Klasyka polskiej science fiction o niezrozumiałym oceanie." }
};

            homePage.UstawKsiazka(ksiazki);
        }
    }
}