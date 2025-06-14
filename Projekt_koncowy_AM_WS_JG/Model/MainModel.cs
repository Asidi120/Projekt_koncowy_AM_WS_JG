using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace Projekt_koncowy_AM_WS_JG.Model
{
    public class MainModel
    {
        private string connStr = "server=localhost;user=root;password=123;database=ksiazki;";
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
    }
}
