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
        private AutorPage autorpage;
        private WydawnictwoPage wydawnictwoPage;
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
                    homePage.PrzeniesNaTytul += PrzeniesNaTytul;
                    homePage.PrzeniesNaAutora += PrzeniesNaAutorazhome;
                    _model.PobierzUzytkownika(menu.EmailLogowanie);
                    _model.ZaladujKsiazkiStatusowe(_model.uzytkownik.IDUzytkownika);
                    UstawKsiazka();
                    homePage.Email=_model.uzytkownik.Email;
                    homePage.Nick = _model.uzytkownik.Nick;
                    homePage.Plec = _model.uzytkownik.Plec;
                    homePage.DataZalozenia = _model.uzytkownik.Data_zalozenia;
                    homePage.Wyszukuje += GdyWyszukuje;
                    homePage.ZmianaPlci += GdyZmianaPlci;
                    homePage.ZmienPlec(_model.uzytkownik.Plec);


                }
            }
            else
            {
                MessageBox.Show("Nieprawidłowy email lub hasło", "Błąd");
            }
        }
        public void GdyZmianaPlci(object sender, string plec)
        {
            if (homePage != null)
            {
                _model.uzytkownik.Plec=plec;
                _model.ZmienPlecUzytkownika(_model.uzytkownik.IDUzytkownika, plec);
                homePage.ZmienPlec(plec);

            }
        }
        public void PrzeniesNaAutorazhome(object sender, String idautorazhome)
        {
            int idautora = int.Parse(idautorazhome)-1;
            var autor = _model.autorzy[idautora];
            autorpage = new AutorPage(autor);
            _view.LoadView(autorpage);
            autorpage.WrocNacisniete += GdyWrocNacisnieteAutor;
        }
        private void GdyWrocNacisnieteAutor(object? sender, EventArgs e)
        {
            if (homePage != null)
            {
                _view.LoadView(homePage);
            }
        }
        public void GdyWyszukuje(object sender, EventArgs e)
        {
            string searchText = homePage.SearchText;
            var filteredBooks = _model.ksiazki.Where(k => k.Tytul.ToLower().Contains(searchText) || k.Autor.ToLower().Contains(searchText)).ToList();
            homePage.PokazKsiazki(filteredBooks);
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
                    menu = new CustomMenu();
                    menu.Loguj += GdyLoguj;
                    menu.Rejestracja += GdyRejestracja;
                    _view.LoadView(menu);
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
            bookPage.WrocNacisniete += GdyWrocNacisniete;
            bookPage.PrzeniesNaWydawnictwo += PrzeniesNaWydawnictwo;
            bookPage.PrzeniesNaAutora += PrzeniesNaAutorazbook;
            bookPage.DodajDoListy += DodajDoListyNacisniete;
            bookPage.UsunZListy += UsunZListyNacisniete;
            bookPage.UstawStatus(_model.ZwrocStatusWybranejKsiazki(bookPage.IDKsiazkiWybranej, _model.uzytkownik.IDUzytkownika));
        }

        public void UsunZListyNacisniete(object sender, String id_ksiazki)
        {
            if (_model.CzyKsiazkaWLiscie(id_ksiazki, _model.uzytkownik.IDUzytkownika))
            {
                _model.UsunStatusZBazy(id_ksiazki, _model.uzytkownik.IDUzytkownika);
            }
        }
        public void DodajDoListyNacisniete(object sender, String wynikzbook)
        {
            string idksiazki = wynikzbook.Split(";")[0];
            string status = wynikzbook.Split(";")[1];
            if (_model.CzyKsiazkaWLiscie(idksiazki, _model.uzytkownik.IDUzytkownika))
            {
                _model.ZamienStatusWBazie(idksiazki, _model.uzytkownik.IDUzytkownika, status);
            }
            else
            {
                _model.DodajStatusDoBazy(idksiazki, _model.uzytkownik.IDUzytkownika, status);
            }
        }

        public void PrzeniesNaAutorazbook(object sender, String idautora)
        {
            int idautoraIndex = int.Parse(idautora)-1;
            var autor = _model.autorzy[idautoraIndex];
            autorpage = new AutorPage(autor);
            _view.LoadView(autorpage);
            autorpage.WrocNacisniete += GdyWrocNacisniete1;
        }
        public void PrzeniesNaWydawnictwo(object sender, String idwydawnictwazbook)
        {
            int idwydawnictwa = int.Parse(idwydawnictwazbook)-1;
            var wydawnictwo = _model.wydawnictwa[idwydawnictwa];
            wydawnictwoPage = new WydawnictwoPage(wydawnictwo);
            _view.LoadView(wydawnictwoPage);
            wydawnictwoPage.WrocNacisniete += GdyWrocNacisniete1;
        }
        private void GdyWrocNacisniete(object? sender, EventArgs e)
        {
            if (homePage != null)
            {
                _view.LoadView(homePage);
            }
        }
        private void GdyOpiniaNacisnieta(object? sender, EventArgs e)
        {
            if (!_model.CzyUzytkownikWyslalOpinie(_model.uzytkownik.IDUzytkownika, bookPage.IDKsiazkiWybranej))
            {
                ratePage = new RatePage();
                _view.LoadView(ratePage);
                ratePage.Wroc += GdyWrocNacisniete1;
                ratePage.WyslijOpinie += GdyWyslijOpinieNacisniete;
            }
            else
            {
                MessageBox.Show("Nie możesz ponownie wystawić opinii");
            }
        }
        private void GdyWyslijOpinieNacisniete(object? sender, EventArgs e)
        {
            _model.DodajDoBazyOpinie(_model.uzytkownik.IDUzytkownika, bookPage.IDKsiazkiWybranej, ratePage.RecenzjaWystawiona, ratePage.OcenaWystawiona);
            _model.ZaladujKsiazkiZBazy();
            var ksiazka = _model.ksiazki.FirstOrDefault(k => k.IDKsiazki == bookPage.IDKsiazkiWybranej);

            if (ksiazka != null)
            {
                bookPage = new BookPage(ksiazka);
                bookPage.OpiniaNacisnieta += GdyOpiniaNacisnieta;
                _view.LoadView(bookPage);
                bookPage.WrocNacisniete += GdyWrocNacisniete;
            }
        }
        public void GdyWrocNacisniete1(object? sender, EventArgs e)
        {
            if (bookPage != null)
            {
                _view.LoadView(bookPage);
            }
        }
        private void ZaladujKsiazki()
        {
            _model.ZaladujKsiazkiZBazy();
        }

        private void UstawKsiazka()
        {
            homePage.UstawKsiazkaNajpopularniejsze(_model.najpopularniejsze_ksiazki);
            homePage.UstawKsiazkaNajnowsze(_model.najnowsze_ksiazki);
            homePage.UstawKsiazkaChcePrzeczytac(_model.uzytkownik.KsiazkiChcePrzeczytac);
            homePage.UstawKsiazkaPrzeczytane(_model.uzytkownik.KsiazkiPrzeczytane);
            homePage.UstawKsiazkaWTrakcie(_model.uzytkownik.KsiazkiWTrakcie);
            homePage.UstawKsiazkaPorzucone(_model.uzytkownik.KsiazkiPorzucone);
        }

    }
}