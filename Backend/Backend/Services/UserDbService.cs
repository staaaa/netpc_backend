using Backend.Models;
using MySql.Data.MySqlClient;

namespace Backend.Services
{
    public class UserDbService
    {
        private readonly MySqlConnection _connection;
        public UserDbService()
        {
            string mysqlConnString = "server=127.0.0.1;user=root;database=netpc;password=";
            _connection = new MySqlConnection(mysqlConnString);
        }
        public User? GetUser(string username)
        {
            try
            {
                _connection.Open();
                string query = "SELECT * FROM users WHERE username = @Username";
                using MySqlCommand cmd = new(query, _connection);
                cmd.Parameters.AddWithValue("@Username", username);

                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        Username = reader.GetString("username"),
                        PasswordHash = reader.GetString("password")
                    };
                }
                return null;
            }
            catch { return null; }
            finally { _connection.Close(); }
        }

        public bool InsertUser(User user)
        {
            try
            {
                _connection.Open();
                string query = "INSERT INTO users VALUES (@Username, @Password)";
                using MySqlCommand cmd = new(query, _connection);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.PasswordHash);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
            finally { _connection.Close(); }
        }
    }
}
