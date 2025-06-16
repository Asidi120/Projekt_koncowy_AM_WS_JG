using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Windows;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class MainModel
    {
        public List<Ksiazka> ksiazki;
        public List<Ksiazka> najpopularniejsze_ksiazki;
        public List<Ksiazka> najnowsze_ksiazki;
        public List<Autor> autorzy;
        public List<Wydawnictwo> wydawnictwa;
        private string connStr = "server=localhost;user=root;password=123;database=ksiazki;";
        public Uzytkownik uzytkownik;
        public MainModel()
        {
            ksiazki = new List<Ksiazka>();
            najpopularniejsze_ksiazki = new List<Ksiazka>();
            najnowsze_ksiazki = new List<Ksiazka>();
            autorzy = new List<Autor>();
            wydawnictwa = new List<Wydawnictwo>();
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
                using (var cmd = new MySqlCommand("SELECT nazwa, kraj_zalozenia, rok_zalozenia FROM wydawnictwo", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nazwa = $"{reader["nazwa"]}";
                        string kraj_zalozenia = $"{reader["kraj_zalozenia"]}";
                        string rok_zalozenia = $"{reader["rok_zalozenia"]}";
                        Wydawnictwo wydawnictwo = new Wydawnictwo(nazwa, kraj_zalozenia, rok_zalozenia);
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
                using (var cmd = new MySqlCommand("SELECT concat(imie, ' ', nazwisko) imie_nazwisko, rok_urodzenia, kraj_pochodzenia FROM autor", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string imie_nazwisko = $"{reader["imie_nazwisko"]}";
                        string rok_urodzenia = $"{reader["rok_urodzenia"]}"; 
                        string kraj_pochodzenia = $"{reader["kraj_pochodzenia"]}";
                        Autor autor = new Autor(imie_nazwisko, rok_urodzenia, kraj_pochodzenia);
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
                using (var cmd = new MySqlCommand($"select count(*) from uzytkownicy where nick = @nick",conn))
                {
                    cmd.Parameters.AddWithValue("@nick", nazwauzytkownika);

                    var liczbawystepowaniauzytkownika = Convert.ToInt32(cmd.ExecuteScalar());
                    if(liczbawystepowaniauzytkownika > 0)
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
                            string emaill = $"{reader["email"]}";
                            string plec = $"{reader["plec"]}";
                            string id_uzytkownk = $"{reader["id_uzytkownik"]}";
                            uzytkownik = new Uzytkownik
                            {
                                Nick = nick,
                                Email = emaill,
                                Plec = plec,
                                Data_zalozenia = data,
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
            PobierzAutorow();
            PobierzWydawnictwa();
            using (var conn1 = GetConnection())
            {
                conn1.Open();

                using (var cmd = new MySqlCommand("select k.id_ksiazka, a.id_autor, w.id_wydaw, tytul, gatunek, opis, jezyk_oryginalu, rok_wydania, liczba_stron, concat(imie, ' ', nazwisko) jakiautor, nazwa, avg(ocena) srednia_ocena, count(ocena) liczba_ocen from ksiazka k left join autor a on k.id_autor = a.id_autor left join wydawnictwo w on k.id_wydaw = w.id_wydaw left join opinia o on o.id_ksiazka = k.id_ksiazka group by k.id_ksiazka;", conn1))
                using (var reader = cmd.ExecuteReader())
                {

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
                                        string użytkownik = $"{reader2["nick"]}";
                                        Opinia opinia = new Opinia(ocena, recenzja, użytkownik, data_wystawienia);
                                        opinie.Lista_Opinii.Add(opinia);

                                    }

                                }
                            }
                        }
                        ksiazki.Add(new Ksiazka(id_ksiazka,id_autor,id_wydaw,tytul, autor, opis, gatunek, rok_wydania, liczba_stron, jezyk_oryginalu, wydawnictwo, opinie));
                        int indeks_autora = int.Parse(id_autor) - 1;
                        autorzy[indeks_autora].Ksiazki = ksiazki
                        .Where(k => k.IDAutora == id_autor)
                        .ToList();

                        int indeks_wydawnictwa = int.Parse(id_wydaw) - 1;
                        wydawnictwa[indeks_wydawnictwa].Ksiazki = ksiazki
                        .Where(k => k.IDWydawnictwa == id_wydaw)
                        .ToList();

                    }
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
            .Take(3)
            .ToList();

           
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
    }
}
