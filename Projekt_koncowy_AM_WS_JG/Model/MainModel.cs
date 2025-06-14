using MySql.Data.MySqlClient;

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

    }
}
