using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class MainModel
    {
        public List<Ksiazka> ksiazki;
        private string connStr = "server=localhost;user=root;password=123;database=ksiazki;";

        public MainModel()
        {
            ksiazki = new List<Ksiazka>();
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }
        public List<string> Autorzy_lista()
        {
            var autorzy = new List<string>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM autor", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string autor = $"{reader["imie"]} {reader["nazwisko"]} ({reader["rok_urodzenia"]}) - {reader["kraj_pochodzenia"]}";
                        autorzy.Add(autor);
                    }
                }
            }
            return autorzy;
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
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("select k.id_ksiazka, tytul, gatunek, opis, jezyk_oryginalu, rok_wydania, liczba_stron, concat(imie, ' ', nazwisko) jakiautor, nazwa, avg(ocena) srednia_ocena, count(ocena) liczba_ocen from ksiazka k left join autor a on k.id_autor = a.id_autor left join wydawnictwo w on k.id_wydaw = w.id_wydaw left join opinia o on o.id_ksiazka = k.id_ksiazka group by k.id_ksiazka;", conn))
                using (var reader = cmd.ExecuteReader())
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
                        string autor = $"{reader["jakiautor"]}";
                        string wydawnictwo = $"{reader["nazwa"]}";
                        string srednia_ocena = $"{reader["srednia_ocena"]}";
                        string liczba_ocen = $"{reader["liczba_ocen"]}";

                        Opinie opinie = new Opinie(srednia_ocena, liczba_ocen);
                        }

                        using (var cmd2 = new MySqlCommand($"select o.ocena, recenzja, data_wystawienia, nick from opinia o, uzytkownicy u where o.id_uzytkownik = u.id_uzytkownik and o.id_ksiazka = @id_ksiazka", conn))
                        using (var reader2 = cmd2.ExecuteReader())
                        {
                            {
                                cmd2.Parameters.AddWithValue("@id_ksiazka", id_ksiazka);

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
                        ksiazki.Add(new Ksiazka(tytul, autor, opis, gatunek, rok_wydania, liczba_stron, jezyk_oryginalu, wydawnictwo, opinie));
                    }
                }
            }
        }
    }
}
