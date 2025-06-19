using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Windows;
using System.IO;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class MainModel
    {
        public List<Ksiazka> ksiazki;
        public List<Ksiazka> najpopularniejsze_ksiazki;
        public List<Ksiazka> najnowsze_ksiazki;
        public List<Autor> autorzy;
        public List<Wydawnictwo> wydawnictwa;
        private string connStr = "server=localhost;user=root;password=!Kuba!12;database=ksiazki;";
        public Uzytkownik uzytkownik;
        public InformacjeZBazy baza;
        public MainModel()
        {
            ksiazki = new List<Ksiazka>();
            najpopularniejsze_ksiazki = new List<Ksiazka>();
            najnowsze_ksiazki = new List<Ksiazka>();
            autorzy = new List<Autor>();
            wydawnictwa = new List<Wydawnictwo>();
            baza = new InformacjeZBazy();
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }

        public void PobierzWydawnictwa()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT id_wydaw, nazwa, kraj_zalozenia, rok_zalozenia FROM wydawnictwo", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id_wydaw = $"{reader["id_wydaw"]}";
                        string nazwa = $"{reader["nazwa"]}";
                        string kraj_zalozenia = $"{reader["kraj_zalozenia"]}";
                        string rok_zalozenia = $"{reader["rok_zalozenia"]}";
                        Wydawnictwo wydawnictwo = new Wydawnictwo(id_wydaw, nazwa, kraj_zalozenia, rok_zalozenia);
                        wydawnictwa.Add(wydawnictwo);
                    }
                }
            }
        }
        public void PobierzAutorow()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT id_autor,concat(imie, ' ', nazwisko) imie_nazwisko, rok_urodzenia, kraj_pochodzenia FROM autor", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id_autor = $"{reader["id_autor"]}";
                        string imie_nazwisko = $"{reader["imie_nazwisko"]}";
                        string rok_urodzenia = $"{reader["rok_urodzenia"]}";
                        string kraj_pochodzenia = $"{reader["kraj_pochodzenia"]}";
                        Autor autor = new Autor(id_autor,imie_nazwisko, rok_urodzenia, kraj_pochodzenia);
                        autorzy.Add(autor);
                    }
                }
            }
        }

        public bool CzyUzytkownikIstnieje(string nazwauzytkownika)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select count(*) from uzytkownicy where nick = @nick", conn))
                {
                    cmd.Parameters.AddWithValue("@nick", nazwauzytkownika);

                    var liczbawystepowaniauzytkownika = Convert.ToInt32(cmd.ExecuteScalar());
                    if (liczbawystepowaniauzytkownika > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool CzyEmailIstnieje(string email)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select count(*) from uzytkownicy where email = @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    var liczbawystepowaniaemaila = Convert.ToInt32(cmd.ExecuteScalar());
                    if (liczbawystepowaniaemaila > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void PobierzUzytkownika(string email)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select id_uzytkownik,nick,data_zalozenia,email,plec from uzytkownicy where email = @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nick = $"{reader["nick"]}";
                            string data = $"{reader["data_zalozenia"]}";
                            string[] data_zalozenia = data.Split();
                            string emaill = $"{reader["email"]}";
                            string plec = $"{reader["plec"]}";
                            string id_uzytkownk = $"{reader["id_uzytkownik"]}";
                            uzytkownik = new Uzytkownik
                            {
                                Nick = nick,
                                Email = emaill,
                                Plec = plec,
                                Data_zalozenia = data_zalozenia[0],
                                IDUzytkownika = id_uzytkownk
                            };
                        }
                    }
                }
            }
        }

        public void DodajDoBazyUzytkownika(string nazwauzytkownika, string haslo, string email)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(haslo);
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"INSERT INTO uzytkownicy (nick, haslo, email) VALUES (@nick, @hashedPassword, @email)", conn))
                {
                    cmd.Parameters.AddWithValue("@nick", nazwauzytkownika);
                    cmd.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DodajDoBazyOpinie(string id_uzytkownik, string id_ksiazka, string recenzja, string ocena)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"INSERT INTO opinia (id_uzytkownik, id_ksiazka, recenzja, ocena, data_wystawienia) VALUES (@id_uzytkownik, @id_ksiazka, @recenzja, @ocena, now())", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    cmd.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);
                    cmd.Parameters.AddWithValue("@recenzja", recenzja);
                    cmd.Parameters.AddWithValue("@ocena", ocena);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string HasloUzytkownika(string email)
        {
            string haslouzytkownika = "";
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select haslo from uzytkownicy where email = @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            haslouzytkownika = $"{reader["haslo"]}";
                        }

                    }
                }

            }
            return haslouzytkownika;
        }

        public bool CzyJestRoot(string email)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select root from uzytkownicy where email = @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    var czyroot = Convert.ToBoolean(cmd.ExecuteScalar());
                    if (czyroot)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void ZaladujKsiazkiZBazy()
        {
            ksiazki = new List<Ksiazka>();
            najnowsze_ksiazki = new List<Ksiazka>();
            najpopularniejsze_ksiazki = new List<Ksiazka>();
            PobierzAutorow();
            PobierzWydawnictwa();
            using (var conn1 = GetConnection())
            {
                conn1.Open();

                using (var cmd = new MySqlCommand("select k.id_ksiazka, a.id_autor, w.id_wydaw, tytul, gatunek, opis, jezyk_oryginalu, rok_wydania, liczba_stron, concat(imie, ' ', nazwisko) jakiautor, nazwa, round(avg(ocena),2) srednia_ocena, count(ocena) liczba_ocen from ksiazka k left join autor a on k.id_autor = a.id_autor left join wydawnictwo w on k.id_wydaw = w.id_wydaw left join opinia o on o.id_ksiazka = k.id_ksiazka group by k.id_ksiazka;", conn1))
                using (var reader = cmd.ExecuteReader())
                {
                    //int licznik = 0;

                    while (reader.Read())
                    {

                        string id_ksiazka = $"{reader["id_ksiazka"]}";
                        string id_autor = $"{reader["id_autor"]}";
                        string id_wydaw = $"{reader["id_wydaw"]}";
                        string tytul = $"{reader["tytul"]}";
                        string gatunek = $"{reader["gatunek"]}";
                        string opis = $"{reader["opis"]}";
                        string jezyk_oryginalu = $"{reader["jezyk_oryginalu"]}";
                        string rok_wydania = $"{reader["rok_wydania"]}";
                        string liczba_stron = $"{reader["liczba_stron"]}";
                        string autor = $"{reader["jakiautor"]}";
                        string wydawnictwo = $"{reader["nazwa"]}";
                        string srednia_ocena = $"{reader["srednia_ocena"]}";
                        string liczba_ocen = $"{reader["liczba_ocen"]}";
                        string rootFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
                        string okladkiFolder = System.IO.Path.Combine(rootFolder, "Okładki");
                        string okladka = System.IO.Path.Combine(okladkiFolder, $"{id_ksiazka}.jpg");
                        Opinie opinie = new Opinie(srednia_ocena, liczba_ocen);

                        using (var conn2 = GetConnection())
                        {
                            conn2.Open();

                            using (var cmd2 = new MySqlCommand($"select o.ocena, recenzja, data_wystawienia, nick from opinia o, uzytkownicy u where o.id_uzytkownik = u.id_uzytkownik and o.id_ksiazka = @id_ksiazka", conn2))
                            {
                                cmd2.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);

                                using (var reader2 = cmd2.ExecuteReader())
                                {



                                    while (reader2.Read())
                                    {
                                        string ocena = $"{reader2["ocena"]}";
                                        string recenzja = $"{reader2["recenzja"]}";
                                        string data_wystawienia = $"{reader2["data_wystawienia"]}";
                                        string[] data = data_wystawienia.Split(" ");
                                        string użytkownik = $"{reader2["nick"]}";
                                        Opinia opinia = new Opinia(ocena, recenzja, użytkownik, data[0]);
                                        opinie.Lista_Opinii.Add(opinia);

                                    }

                                }
                            }
                        }
                        ksiazki.Add(new Ksiazka(id_ksiazka, id_autor, id_wydaw, tytul, autor, opis, gatunek, rok_wydania, liczba_stron, jezyk_oryginalu, wydawnictwo, opinie,okladka));
                        ////int indeks_autora = int.Parse(id_autor) - 1;
                        //autorzy[licznik].Ksiazki = ksiazki
                        //.Where(k => k.IDAutora == id_autor)
                        //.ToList();


                        //int indeks_wydawnictwa = int.Parse(id_wydaw) - 1;
                        //wydawnictwa[licznik].Ksiazki = ksiazki
                        //.Where(k => k.IDWydawnictwa == id_wydaw)
                        //.ToList();

                        ///*licznik*/++;

                    }
                    PobierzKsiazkiAutorowIWydawnictw();
                }
            }
            najpopularniejsze_ksiazki = ksiazki
             .Where(k => k.JakieOpinie != null && int.TryParse(k.JakieOpinie.Liczba_Ocen, out _))
             .OrderByDescending(k => int.Parse(k.JakieOpinie.Liczba_Ocen))
             .Take(10)
             .ToList();

            najnowsze_ksiazki = ksiazki
            .Where(k => k.RokWydania != null && int.TryParse(k.RokWydania, out _))
            .OrderByDescending(k => int.Parse(k.RokWydania))
            .Take(10)
            .ToList();


        }

        public void PobierzKsiazkiAutorowIWydawnictw()
        {
            for (int i = 0; i < autorzy.Count; i++) 
            {
                autorzy[i].Ksiazki = ksiazki
                        .Where(k => k.IDAutora == autorzy[i].IDAutora)
                        .ToList();
            }

            for (int i = 0; i < wydawnictwa.Count; i++)
            {
                wydawnictwa[i].Ksiazki = ksiazki
                        .Where(k => k.IDWydawnictwa == wydawnictwa[i].IDWydawnictwa)
                        .ToList();
            }
        }
        public bool CzyUzytkownikWyslalOpinie(string id_uzytkownik, string id_ksiazka)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select count(*) ile_opinii from opinia where id_uzytkownik = @id_uzytkownik and id_ksiazka = @id_ksiazka", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    cmd.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);
                    var ile = Convert.ToInt32(cmd.ExecuteScalar());
                    if (ile >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool CzyKsiazkaWLiscie(string id_ksiazka, string id_uzytkownik)
        {

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select count(*) ile_statusow from status where id_uzytkownik = @id_uzytkownik and id_ksiazka = @id_ksiazka", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    cmd.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);
                    var ile = Convert.ToInt32(cmd.ExecuteScalar());
                    if (ile >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void DodajStatusDoBazy(string id_ksiazka, string id_uzytkownik, string status)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"INSERT INTO status (id_uzytkownik, id_ksiazka, status) VALUES (@id_uzytkownik, @id_ksiazka, @status)", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    cmd.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);
                    cmd.Parameters.AddWithValue("@status", status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UsunStatusZBazy(string id_ksiazka, string id_uzytkownik)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"delete from status where id_uzytkownik = @id_uzytkownik and id_ksiazka = @id_ksiazka;", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    cmd.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ZamienStatusWBazie(string id_ksiazka, string id_uzytkownik, string status)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"update status set status = @status where id_uzytkownik = @id_uzytkownik and id_ksiazka = @id_ksiazka;", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    cmd.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);
                    cmd.Parameters.AddWithValue("@status", status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string ZwrocStatusWybranejKsiazki(string id_ksiazka, string id_uzytkownik)
        {
            string statuswybrany = "";
            using (var conn = GetConnection())

            {
                conn.Open();
                using (var cmd = new MySqlCommand($"select status from status where id_ksiazka = @id_ksiazka and id_uzytkownik = @id_uzytkownik;", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    cmd.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            statuswybrany = $"{reader["status"]}";
                        }
                    }
                }
            }
            return statuswybrany;
        }
        public void ZmienPlecUzytkownika(string id_uzytkownika, string plec)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"update uzytkownicy set plec = @plec where id_uzytkownik = @id_uzytkownika;", conn))
                {
                    cmd.Parameters.AddWithValue("@id_uzytkownika", id_uzytkownika);
                    cmd.Parameters.AddWithValue("@plec", plec);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ZaladujKsiazkiStatusowe(string id_uzytkownik)
        {
            string status = "";
            string id_ksiazki_string = "";
            int id_ksiazki;

            //Chcę przeczytać
            using (var conn1 = GetConnection())

            {
                conn1.Open();
                using (var cmd1 = new MySqlCommand($"select id_ksiazka, status from status where id_uzytkownik = @id_uzytkownik and status = 'Chcę przeczytać';", conn1))
                {
                    cmd1.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    using (var reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = $"{reader["status"]}";
                            id_ksiazki_string = $"{reader["id_ksiazka"]}";
                           
                            id_ksiazki = ksiazki.FindIndex(k => k.IDKsiazki == id_ksiazki_string);

                            uzytkownik.KsiazkiChcePrzeczytac.Add(ksiazki[id_ksiazki]);
                        }
                    }
                }
            }
            //Przeczytane
            using (var conn2 = GetConnection())

            {
                conn2.Open();
                using (var cmd2 = new MySqlCommand($"select id_ksiazka, status from status where id_uzytkownik = @id_uzytkownik and status = 'Przeczytane';", conn2))
                {
                    cmd2.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    using (var reader = cmd2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = $"{reader["status"]}";
                            id_ksiazki_string = $"{reader["id_ksiazka"]}";
                            id_ksiazki = ksiazki.FindIndex(k => k.IDKsiazki == id_ksiazki_string);

                            uzytkownik.KsiazkiPrzeczytane.Add(ksiazki[id_ksiazki]);
                        }
                    }
                }
            }

            //W trakcie
            using (var conn3 = GetConnection())

            {
                conn3.Open();
                using (var cmd3 = new MySqlCommand($"select id_ksiazka, status from status where id_uzytkownik = @id_uzytkownik and status = 'W trakcie';", conn3))
                {
                    cmd3.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    using (var reader = cmd3.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = $"{reader["status"]}";
                            id_ksiazki_string = $"{reader["id_ksiazka"]}";
                            id_ksiazki = ksiazki.FindIndex(k => k.IDKsiazki == id_ksiazki_string);

                            uzytkownik.KsiazkiWTrakcie.Add(ksiazki[id_ksiazki]);
                        }
                    }
                }
            }
            //Porzucone
            using (var conn4 = GetConnection())

            {
                conn4.Open();
                using (var cmd4 = new MySqlCommand($"select id_ksiazka, status from status where id_uzytkownik = @id_uzytkownik and status = 'Porzucone';", conn4))
                {
                    cmd4.Parameters.AddWithValue("@id_uzytkownik", id_uzytkownik);
                    using (var reader = cmd4.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = $"{reader["status"]}";
                            id_ksiazki_string = $"{reader["id_ksiazka"]}";
                            id_ksiazki = ksiazki.FindIndex(k => k.IDKsiazki == id_ksiazki_string);

                            uzytkownik.KsiazkiPorzucone.Add(ksiazki[id_ksiazki]);
                        }
                    }
                }
            }
        }

        public void ZaladujBaze()
        {
            baza = new InformacjeZBazy();
            // Ładowanie książek
            
            using (var conn1 = GetConnection())
            {
                conn1.Open();
                using (var cmd1 = new MySqlCommand("SELECT k.id_ksiazka, k.tytul, k.gatunek, k.opis, k.jezyk_oryginalu, k.rok_wydania, k.liczba_stron, k.id_autor, k.id_wydaw, w.nazwa AS wydawnictwo, a.imie AS imie_autora, a.nazwisko AS nazwisko_autora FROM ksiazka k, wydawnictwo w, autor a WHERE k.id_wydaw = w.id_wydaw AND k.id_autor = a.id_autor;\r\n", conn1))
                using (var reader = cmd1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id_ksiazka = $"{reader["id_ksiazka"]}";
                        string tytul = $"{reader["tytul"]}";
                        string gatunek = $"{reader["gatunek"]}";
                        string opis = $"{reader["opis"]}";
                        string jezyk_oryginalu = $"{reader["jezyk_oryginalu"]}";
                        string rok_wydania = $"{reader["rok_wydania"]}";
                        string liczba_stron = $"{reader["liczba_stron"]}";
                        string id_autor = $"{reader["id_autor"]}";
                        string id_wydaw = $"{reader["id_wydaw"]}";
                        string autor = $"{reader["imie_autora"]}"+" "+$"{reader["nazwisko_autora"]}";
                        string wydawnictwo = $"{reader["wydawnictwo"]}";
                        Ksiazka ksiazka = new Ksiazka(id_ksiazka, tytul, gatunek, opis, jezyk_oryginalu, rok_wydania, liczba_stron, id_autor, id_wydaw,autor,wydawnictwo);
                        baza.Ksiazki.Add(ksiazka);
                    }
                }
            }

            // Ładowanie autorów
            using (var conn2 = GetConnection())
            {
                conn2.Open();
                using (var cmd2 = new MySqlCommand("SELECT * FROM autor;", conn2))
                using (var reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = $"{reader["id_autor"]}";
                        string imie = $"{reader["imie"]}";
                        string nazwisko = $"{reader["nazwisko"]}";
                        string rok = $"{reader["rok_urodzenia"]}";
                        string kraj = $"{reader["kraj_pochodzenia"]}";

                        Autor autor = new Autor(id,imie, nazwisko, rok, kraj);
                        baza.Autorzy.Add(autor);
                    }
                }
            }

            // Ładowanie wydawnictw
            using (var conn3 = GetConnection())
            {
                conn3.Open();
                using (var cmd3 = new MySqlCommand("SELECT * FROM wydawnictwo;", conn3))
                using (var reader = cmd3.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = $"{reader["id_wydaw"]}";
                        string nazwa = $"{reader["nazwa"]}";
                        string kraj = $"{reader["kraj_zalozenia"]}";
                        string rok = $"{reader["rok_zalozenia"]}";

                        Wydawnictwo wyd = new Wydawnictwo(id, nazwa, kraj, rok);
                        baza.Wydawnictwa.Add(wyd);
                    }
                }
            }

            // Ładowanie użytkowników
            using (var conn4 = GetConnection())
            {
                conn4.Open();
                using (var cmd4 = new MySqlCommand("SELECT * FROM uzytkownicy;", conn4))
                using (var reader = cmd4.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = $"{reader["id_uzytkownik"]}";
                        string nick = $"{reader["nick"]}";
                        string email = $"{reader["email"]}";
                        string plec = $"{reader["plec"]}";
                        string data = $"{reader["data_zalozenia"]}";
                        string root = $"{reader["root"]}";

                        Uzytkownik uz = new Uzytkownik(nick, email, plec, data, id, root);
                        baza.Uzytkownicy.Add(uz);
                    }
                }
            }

            // Ładowanie opinii
            using (var conn5 = GetConnection())
            {
                conn5.Open();
                using (var cmd5 = new MySqlCommand("SELECT * FROM opinia;", conn5))
                using (var reader = cmd5.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        string id_opinia = $"{reader["id_opinia"]}";
                        string id_ksiazka = $"{reader["id_ksiazka"]}";
                        string ocena = $"{reader["ocena"]}";
                        string recenzja = $"{reader["recenzja"]}";
                        string uzytkownik = $"{reader["id_uzytkownik"]}";
                        string data = $"{reader["data_wystawienia"]}";

                        Opinia op = new Opinia(id_opinia,id_ksiazka,ocena, recenzja, uzytkownik, data);
                        baza.Opinie.Add(op);
                    }
                }
            }

            // Ładowanie statusów
            using (var conn6 = GetConnection())
            {
                conn6.Open();
                using (var cmd6 = new MySqlCommand("SELECT * FROM status;", conn6))
                using (var reader = cmd6.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idUzytkownik = $"{reader["id_uzytkownik"]}";
                        string idKsiazka = $"{reader["id_ksiazka"]}";
                        string status = $"{reader["status"]}";

                        Status stat = new Status(idUzytkownik, idKsiazka, status);
                        baza.Statusy.Add(stat);
                    }
                }
            }
        }
        public void DodajKsiazkeDoBazy(Ksiazka ksiazka)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO ksiazka 
                        (tytul, gatunek, opis, liczba_stron, rok_wydania, jezyk_oryginalu, id_autor, id_wydaw)
                        VALUES (@tytul, @gatunek, @opis, @liczba_stron, @rok_wydania, @jezyk_oryginalu, @id_autor, @id_wydaw)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tytul", ksiazka.Tytul);
                    cmd.Parameters.AddWithValue("@gatunek", ksiazka.Gatunek);
                    cmd.Parameters.AddWithValue("@opis", ksiazka.Opis);
                    cmd.Parameters.AddWithValue("@liczba_stron", ksiazka.LiczbaStron);
                    cmd.Parameters.AddWithValue("@rok_wydania", ksiazka.RokWydania);
                    cmd.Parameters.AddWithValue("@jezyk_oryginalu", ksiazka.Jezyk);
                    cmd.Parameters.AddWithValue("@id_autor", ksiazka.IDAutora);
                    cmd.Parameters.AddWithValue("@id_wydaw", ksiazka.IDWydawnictwa);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AktualizujKsiazke(Ksiazka ksiazka)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string query = @"UPDATE ksiazka 
                        SET tytul = @tytul,gatunek = @gatunek,opis = @opis,liczba_stron = @liczba_stron,rok_wydania = @rok_wydania,jezyk_oryginalu = @jezyk_oryginalu, id_autor = @id_autor,id_wydaw = @id_wydaw WHERE id_ksiazka = @id_ksiazka";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tytul", ksiazka.Tytul);
                    cmd.Parameters.AddWithValue("@gatunek", ksiazka.Gatunek);
                    cmd.Parameters.AddWithValue("@opis", ksiazka.Opis);
                    cmd.Parameters.AddWithValue("@liczba_stron", ksiazka.LiczbaStron);
                    cmd.Parameters.AddWithValue("@rok_wydania", ksiazka.RokWydania);
                    cmd.Parameters.AddWithValue("@jezyk_oryginalu", ksiazka.Jezyk);
                    cmd.Parameters.AddWithValue("@id_autor", ksiazka.IDAutora);
                    cmd.Parameters.AddWithValue("@id_wydaw", ksiazka.IDWydawnictwa);
                    cmd.Parameters.AddWithValue("@id_ksiazka", ksiazka.IDKsiazki);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UsunKsiazkeZBazy(string idKsiazki)
        {
            if (string.IsNullOrEmpty(idKsiazki))
                return;

            using (var conn = GetConnection())
            {
                conn.Open();

                string usunPowiazaniaQuery = @"
            DELETE FROM opinia WHERE id_ksiazka = @id_ksiazka;
            DELETE FROM status WHERE id_ksiazka = @id_ksiazka;
        ";

                using (var cmd = new MySqlCommand(usunPowiazaniaQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id_ksiazka", idKsiazki);
                    cmd.ExecuteNonQuery();
                }

                string usunKsiazkeQuery = "DELETE FROM ksiazka WHERE id_ksiazka = @id_ksiazka";

                using (var cmd = new MySqlCommand(usunKsiazkeQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id_ksiazka", idKsiazki);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DodajAutoraDoBazy(Autor autor)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string insertQuery = @"INSERT INTO autor (imie, nazwisko, rok_urodzenia, kraj_pochodzenia) 
                               VALUES (@imie, @nazwisko, @rok_urodzenia, @kraj_pochodzenia)";

                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@imie", autor.Imie);
                    cmd.Parameters.AddWithValue("@nazwisko", autor.Nazwisko);
                    cmd.Parameters.AddWithValue("@rok_urodzenia", autor.DataUrodzenia);
                    cmd.Parameters.AddWithValue("@kraj_pochodzenia", autor.Narodowosc);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AktualizujAutora(Autor autor)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string updateQuery = @"UPDATE autor 
                               SET imie = @imie,
                                   nazwisko = @nazwisko,
                                   rok_urodzenia = @rok_urodzenia,
                                   kraj_pochodzenia = @kraj_pochodzenia
                               WHERE id_autor = @id_autor";

                using (var cmd = new MySqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@imie", autor.Imie);
                    cmd.Parameters.AddWithValue("@nazwisko", autor.Nazwisko);
                    cmd.Parameters.AddWithValue("@rok_urodzenia", autor.DataUrodzenia);
                    cmd.Parameters.AddWithValue("@kraj_pochodzenia", autor.Narodowosc);
                    cmd.Parameters.AddWithValue("@id_autor", autor.IDAutora);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool UsunAutoraZBazy(string idAutora)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM ksiazka WHERE id_autor = @id_autor";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id_autor", idAutora);
                    int liczbaKsiazek = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (liczbaKsiazek > 0)
                    {
                        return false;
                    }
                }
                string deleteQuery = "DELETE FROM autor WHERE id_autor = @id_autor";
                using (var deleteCmd = new MySqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@id_autor", idAutora);
                    deleteCmd.ExecuteNonQuery();
                }

                return true;
            }
        }
        public void DodajWydawnictwoDoBazy(Wydawnictwo wydawnictwo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string insertQuery = @"INSERT INTO wydawnictwo (nazwa, kraj_zalozenia, rok_zalozenia) 
                               VALUES (@nazwa, @kraj_zalozenia, @rok_zalozenia)";

                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@nazwa", wydawnictwo.Nazwa);
                    cmd.Parameters.AddWithValue("@kraj_zalozenia", wydawnictwo.Kraj_zalozenia ?? "Nieznane");
                    int rok = 0;
                    if (!int.TryParse(wydawnictwo.Rok_zalozenia?.ToString(), out rok))
                    {
                        rok = 0;
                    }
                    cmd.Parameters.AddWithValue("@rok_zalozenia", rok);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AktualizujWydawnictwo(Wydawnictwo wydawnictwo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string updateQuery = @"UPDATE wydawnictwo
                               SET nazwa = @nazwa,
                                   kraj_zalozenia = @kraj_zalozenia,
                                   rok_zalozenia = @rok_zalozenia
                               WHERE id_wydaw = @id_wydaw";

                using (var cmd = new MySqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@nazwa", wydawnictwo.Nazwa);
                    cmd.Parameters.AddWithValue("@kraj_zalozenia", wydawnictwo.Kraj_zalozenia ?? "Nieznane");
                    cmd.Parameters.AddWithValue("@rok_zalozenia", wydawnictwo.Rok_zalozenia);
                    cmd.Parameters.AddWithValue("@id_wydaw", wydawnictwo.IDWydawnictwa);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool UsunWydawnictwoZBazy(int idWydawnictwa)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string checkQuery = @"SELECT COUNT(*) FROM ksiazka WHERE id_wydaw = @id_wydaw";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id_wydaw", idWydawnictwa);
                    int liczbaKsiazek = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (liczbaKsiazek > 0)
                    {
                        return false;
                    }
                }
                string deleteQuery = @"DELETE FROM wydawnictwo WHERE id_wydaw = @id_wydaw";
                using (var deleteCmd = new MySqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@id_wydaw", idWydawnictwa);
                    deleteCmd.ExecuteNonQuery();
                }
                return true;
            }
        }



    }
}

