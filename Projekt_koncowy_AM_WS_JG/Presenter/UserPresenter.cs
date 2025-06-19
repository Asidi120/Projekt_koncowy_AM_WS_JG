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
        private DodajEdytujPage dodajEdytujPage;
        private DodajEdytujAutorPage dodajEdytujAutorPage;
        private DodajEdytujeWydawnictwoPage dodajEdytujWydawnictwoPage;
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
                    _model.ZaladujBaze();
                    ZaladujWidokHome();
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
        public enum ZakladkaStartowa
        {
            Ksiazki,
            Wydawnictwa,
            Autorzy,
            Opinie,
            Status,
            Uzytkownicy
        }
        private void ZaladujWidokHome(ZakladkaStartowa zakladka = ZakladkaStartowa.Ksiazki)
        {
            homePageRoot = new HomePageRoot();

            homePageRoot.Wyloguj += GdyWyloguj;
            homePageRoot.Edytuj += GdyEdytujNacisniete;
            homePageRoot.Dodaj += GdyDodajNacisniete;
            homePageRoot.Usun += GdyUsunNacisniete;
            homePageRoot.DodajWydawnictwo += GdyDodajWydawnictwoNacisniete;
            homePageRoot.EdytujWydawnictwo += GdyEdytujWydawnictwoNacisniete;
            homePageRoot.UsunWydawnictwo += GdyUsunWydawnictwoNacisniete;
            homePageRoot.DodajAutora += GdyDodajAutoraNacisniete;
            homePageRoot.EdytujAutora += GdyEdytujAutoraNacisniete;
            homePageRoot.UsunAutora += GdyUsunAutoraNacisniete;
            homePageRoot.ZmienRoot += GdyZmienRootNacisniete;
            homePageRoot.UsunUzytkownika += GdyUsunUzytkownikaNacisniete;
            homePageRoot.UsunStatus += GdyUsunStatusNacisniete;
            homePageRoot.UsunOpinie += GdyUsunOpinieNacisniete;


            homePageRoot.UstawKsiazkiwroot(_model.baza);
            switch (zakladka)
            {
                case ZakladkaStartowa.Wydawnictwa:
                    homePageRoot.PrzelaczNaZakladkeWydawnictwa();
                    break;
                case ZakladkaStartowa.Autorzy:
                    homePageRoot.PrzelaczNaZakladkeAutorzy();
                    break;
                case ZakladkaStartowa.Opinie:
                    homePageRoot.PrzelaczNaZakladkeOpinie();
                    break;
                case ZakladkaStartowa.Uzytkownicy:
                    homePageRoot.PrzelaczNaZakladkeUzytkownicy();
                    break;
                case ZakladkaStartowa.Status:
                    homePageRoot.PrzelaczNaZakladkeStatus();
                    break;
                default:
                    homePageRoot.PrzelaczNaZakladkeKsiazki();
                    break;
            }

            _view.LoadView(homePageRoot);
        }

        private void GdyUsunOpinieNacisniete(object? sender, string idOpini)
        {
            _model.UsunOpinieZBazy(idOpini);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Opinie);
        }

        private void GdyUsunStatusNacisniete(int idUzytkownika, int idKsiazki)
        {
            _model.UsunStatusZBazy(idUzytkownika, idKsiazki);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Status);
        }



        public void GdyZmienRootNacisniete(object sender, string idUzytkownika)
        {
            if (homePageRoot != null)
            {
                if (int.TryParse(idUzytkownika, out int id))
                {
                    _model.UstawRootDlaUzytkownika(id);
                    _model.ZaladujBaze();
                    ZaladujWidokHome(ZakladkaStartowa.Uzytkownicy);
                }
                else
                {
                    MessageBox.Show("Niepoprawny format ID użytkownika.");
                }
            }
        }

        public void GdyUsunUzytkownikaNacisniete(object sender, string idUzytkownika)
        {
            if (homePageRoot != null)
            {
                if (int.TryParse(idUzytkownika, out int id))
                {
                    _model.UsunUzytkownikaZBazy(id);
                    _model.ZaladujBaze();
                    ZaladujWidokHome(ZakladkaStartowa.Uzytkownicy);
                }
                else
                {
                    MessageBox.Show("Niepoprawny format ID użytkownika.");

                }
            }
        }

        public void GdyDodajWydawnictwoNacisniete(object sender, Wydawnictwo wydawnictwo)
        {
            if (homePageRoot != null)
            {
                dodajEdytujWydawnictwoPage = new DodajEdytujeWydawnictwoPage(null);  
                dodajEdytujWydawnictwoPage.DodajWydawnictwo += GdyDodajWydawnictwoZatwierdzone; 
                dodajEdytujWydawnictwoPage.WrocNacisniete += GdyWrocNacisniete3;

                _view.LoadView(dodajEdytujWydawnictwoPage);
            }
        }

        private void GdyDodajWydawnictwoZatwierdzone(object sender, Wydawnictwo wydawnictwo)
        {
            _model.DodajWydawnictwoDoBazy(wydawnictwo);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Wydawnictwa);
        }
        public void GdyEdytujWydawnictwoNacisniete(object sender, Wydawnictwo wydawnictwo)
        {
            if (homePageRoot != null)
            {
                dodajEdytujWydawnictwoPage = new DodajEdytujeWydawnictwoPage(wydawnictwo);
                dodajEdytujWydawnictwoPage.DodajWydawnictwo += GdyEdytujWydawnictwoZatwierdzone; 
                dodajEdytujWydawnictwoPage.WrocNacisniete += GdyWrocNacisniete3;                 
                _view.LoadView(dodajEdytujWydawnictwoPage);
            }
        }

        private void GdyEdytujWydawnictwoZatwierdzone(object sender, Wydawnictwo wydawnictwo)
        {
            _model.AktualizujWydawnictwo(wydawnictwo);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Wydawnictwa);
        }

        public void GdyUsunWydawnictwoNacisniete(object sender, string idWydawnictwa)
        {
            if (!int.TryParse(idWydawnictwa, out int id))
            {
                MessageBox.Show("Nieprawidłowy identyfikator wydawnictwa.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool usunieto = _model.UsunWydawnictwoZBazy(id);
            if (!usunieto)
            {
                MessageBox.Show("Nie można usunąć wydawnictwa, ponieważ ma przypisane książki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Wydawnictwa);
        }


        public void GdyDodajAutoraNacisniete(object sender, Autor autor)
        {
            if (homePageRoot != null)
            {
                dodajEdytujAutorPage = new DodajEdytujAutorPage(null); 
                dodajEdytujAutorPage.DodajAutora += GdyDodajAutoraZatwierdzony; 
                dodajEdytujAutorPage.WrocNacisniete += GdyWrocNacisniete3;

                _view.LoadView(dodajEdytujAutorPage);
            }
        }
        private void GdyDodajAutoraZatwierdzony(object sender, Autor autor)
        {
            _model.DodajAutoraDoBazy(autor); 
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Autorzy);
        }



        public void GdyEdytujAutoraNacisniete(object sender, Autor autor)
        {
            if (homePageRoot != null)
            {
                dodajEdytujAutorPage = new DodajEdytujAutorPage(autor);
                dodajEdytujAutorPage.DodajAutora += GdyEdytujAutoraZatwierdzony;
                dodajEdytujAutorPage.WrocNacisniete += GdyWrocNacisniete3;
                _view.LoadView(dodajEdytujAutorPage);
            }
        }
        private void GdyEdytujAutoraZatwierdzony(object sender, Autor autor)
        {
            _model.AktualizujAutora(autor);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Autorzy);
        }


        public void GdyUsunAutoraNacisniete(object sender, string idAutora)
        {
            bool usunieto = _model.UsunAutoraZBazy(idAutora);

            if (!usunieto)
            {
                MessageBox.Show("Nie można usunąć autora, ponieważ ma przypisane książki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Autorzy);
        }


        public void GdyUsunNacisniete(object sender, string idKsiazki)
        {
            _model.UsunKsiazkeZBazy(idKsiazki);
            _model.ZaladujBaze();

            homePageRoot = new HomePageRoot();
            homePageRoot.Wyloguj += GdyWyloguj;
            homePageRoot.Edytuj += GdyEdytujNacisniete;
            homePageRoot.Dodaj += GdyDodajNacisniete;
            homePageRoot.Usun += GdyUsunNacisniete;
            homePageRoot.DodajWydawnictwo += GdyDodajWydawnictwoNacisniete;
            homePageRoot.EdytujWydawnictwo += GdyEdytujWydawnictwoNacisniete;
            homePageRoot.UsunWydawnictwo += GdyUsunWydawnictwoNacisniete;
            homePageRoot.DodajAutora += GdyDodajAutoraNacisniete;
            homePageRoot.EdytujAutora += GdyEdytujAutoraNacisniete;
            homePageRoot.UsunAutora += GdyUsunAutoraNacisniete;

            homePageRoot.UstawKsiazkiwroot(_model.baza);
            _view.LoadView(homePageRoot); 
        }


        public void GdyEdytujNacisniete(object sender, Ksiazka ksiazka)
        {
            if (homePageRoot != null)
            {
                dodajEdytujPage = new DodajEdytujPage(ksiazka);
                dodajEdytujPage.Edytuj += GdyEdytujKsiazkeNacisniete;
                _view.LoadView(dodajEdytujPage);
                dodajEdytujPage.WrocNacisniete += GdyWrocNacisniete3;
            }
        }
        private void GdyEdytujKsiazkeNacisniete(object sender, Ksiazka ksiazka)
        {
            _model.AktualizujKsiazke(ksiazka);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Ksiazki);
        }
        public void GdyDodajNacisniete(object sender, Ksiazka ksiazka)
        {
            if (homePageRoot != null)
            {
                dodajEdytujPage = new DodajEdytujPage(null);
                dodajEdytujPage.Edytuj += GdyDodajKsiazkeNacisniete;
                _view.LoadView(dodajEdytujPage);
                dodajEdytujPage.WrocNacisniete += GdyWrocNacisniete3;

            }
        }
        private void GdyDodajKsiazkeNacisniete(object sender, Ksiazka ksiazka)
        {
            _model.DodajKsiazkeDoBazy(ksiazka);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Ksiazki);
        }
        public void GdyWrocNacisniete3(object? sender, EventArgs e)
        {
            if (homePageRoot != null)
            {
                _model.ZaladujBaze();
                homePageRoot = new HomePageRoot();
                homePageRoot.Wyloguj += GdyWyloguj;
                _view.LoadView(homePageRoot);
                homePageRoot.Edytuj += GdyEdytujNacisniete;
                homePageRoot.Dodaj += GdyDodajNacisniete;
                homePageRoot.Usun += GdyUsunNacisniete;
                homePageRoot.UstawKsiazkiwroot(_model.baza);
                homePageRoot.DodajWydawnictwo += GdyDodajWydawnictwoNacisniete;
                homePageRoot.EdytujWydawnictwo += GdyEdytujWydawnictwoNacisniete;
                homePageRoot.UsunWydawnictwo += GdyUsunWydawnictwoNacisniete;
                homePageRoot.DodajAutora += GdyDodajAutoraNacisniete;
                homePageRoot.EdytujAutora += GdyEdytujAutoraNacisniete;
                homePageRoot.UsunAutora += GdyUsunAutoraNacisniete;
                homePageRoot.UsunUzytkownika += GdyUsunUzytkownikaNacisniete;
                homePageRoot.ZmienRoot += GdyZmienRootNacisniete;
                homePageRoot.UsunStatus += GdyUsunStatusNacisniete;
            }
        }
        //public void GdyZmienRootNacisniete(object sender, string id_root)
        //{
        //    if (homePageRoot != null)
        //    {
        //        //_model.ZmienRoot(id_root);
        //        //trzeba jeszcze zmienic karte 
        //        _model.ZaladujBaze();
        //        homePageRoot = new HomePageRoot();
        //        homePageRoot.Wyloguj += GdyWyloguj;
        //        _view.LoadView(homePageRoot);
        //        homePageRoot.Edytuj += GdyEdytujNacisniete;
        //        homePageRoot.Dodaj += GdyDodajNacisniete;
        //        homePageRoot.Usun += GdyUsunNacisniete;
        //        homePageRoot.UstawKsiazkiwroot(_model.baza);
        //        homePageRoot.DodajWydawnictwo += GdyDodajWydawnictwoNacisniete;
        //        homePageRoot.EdytujWydawnictwo += GdyEdytujWydawnictwoNacisniete;
        //        homePageRoot.UsunWydawnictwo += GdyUsunWydawnictwoNacisniete;
        //        homePageRoot.DodajAutora += GdyDodajAutoraNacisniete;
        //        homePageRoot.EdytujAutora += GdyEdytujAutoraNacisniete;
        //        homePageRoot.UsunAutora += GdyUsunAutoraNacisniete;
        //        homePageRoot.UsunUzytkownika += GdyUsunUzytkownikaNacisniete;
        //        homePageRoot.ZmienRoot += GdyZmienRootNacisniete;
        //    }
        //}
        public void GdyUsunStatus(object sender, (int idUzytkownika, int idKsiazki) args)
        {
            int idUzytkownika = args.idUzytkownika;
            int idKsiazki = args.idKsiazki;
            _model.UsunStatusZBazy(idUzytkownika, idKsiazki);
            _model.ZaladujBaze();
            ZaladujWidokHome(ZakladkaStartowa.Status);
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
            int idautora = _model.autorzy.FindIndex(a => a.IDAutora == idautorazhome);
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
            bookPage.WrocNacisniete += GdyWrocNacisniete2;
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
            int idautoraIndex = _model.autorzy.FindIndex(a => a.IDAutora == idautora);

            var autor = _model.autorzy[idautoraIndex];
            autorpage = new AutorPage(autor);
            _view.LoadView(autorpage);
            autorpage.WrocNacisniete += GdyWrocNacisniete1;
        }
        public void PrzeniesNaWydawnictwo(object sender, String idwydawnictwazbook)
        {
            int idwydawnictwa = _model.wydawnictwa.FindIndex(w => w.IDWydawnictwa == idwydawnictwazbook);
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
        private void GdyWrocNacisniete2(object? sender, EventArgs e)
        {
            int indeks_zakladki = homePage.KtoraZakladka;
            string wyszukiwane = homePage.SearchText;
            homePage=new HomePage();
            homePage.Wyloguj += GdyWyloguj;
            _view.LoadView(homePage);
            ZaladujKsiazki();
            homePage.PrzeniesNaTytul += PrzeniesNaTytul;
            homePage.PrzeniesNaAutora += PrzeniesNaAutorazhome;
            _model.PobierzUzytkownika(menu.EmailLogowanie);
            _model.ZaladujKsiazkiStatusowe(_model.uzytkownik.IDUzytkownika);
            UstawKsiazka();
            homePage.Email = _model.uzytkownik.Email;
            homePage.Nick = _model.uzytkownik.Nick;
            homePage.Plec = _model.uzytkownik.Plec;
            homePage.DataZalozenia = _model.uzytkownik.Data_zalozenia;
            homePage.Wyszukuje += GdyWyszukuje;
            homePage.ZmianaPlci += GdyZmianaPlci;
            homePage.ZmienPlec(_model.uzytkownik.Plec);
            switch (indeks_zakladki)
            {
              case 0:
                    homePage.KtoraZakladka = 0;
                    break;
              case 1:
                    homePage.KtoraZakladka = 1;
                    homePage.SearchTextBox.Text = wyszukiwane;
                    break;
              case 2:
                    homePage.KtoraZakladka = 2;
                    break;
              case 3:
                    homePage.KtoraZakladka = 3;
                    break;

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
                bookPage.WrocNacisniete += GdyWrocNacisniete2;
                bookPage.PrzeniesNaWydawnictwo += PrzeniesNaWydawnictwo;
                bookPage.PrzeniesNaAutora += PrzeniesNaAutorazbook;
                bookPage.DodajDoListy += DodajDoListyNacisniete;
                bookPage.UsunZListy += UsunZListyNacisniete;
                bookPage.UstawStatus(_model.ZwrocStatusWybranejKsiazki(bookPage.IDKsiazkiWybranej, _model.uzytkownik.IDUzytkownika));
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