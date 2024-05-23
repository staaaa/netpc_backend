using Backend.Models;
using MySql.Data.MySqlClient;

namespace Backend.Services
{
    public class ContactsDbService
    {
        private readonly MySqlConnection _connection;
        public ContactsDbService()
        {
            string mysqlConnString = "server=127.0.0.1;user=root;database=netpc;password=";
            _connection = new MySqlConnection(mysqlConnString);
        }

        public ContactDto GetContact(string email)
        {
            try
            {
                _connection.Open();
                string query = "SELECT * FROM contacts WHERE email = @Email";
                using MySqlCommand cmd = new(query, _connection);
                cmd.Parameters.AddWithValue("@Email", email);

                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new ContactDto
                    {
                        Name = reader.GetString("name"),
                        Surname = reader.GetString("surname"),
                        Email = reader.GetString("email"),
                        Phone = reader.GetString("phone"),
                        Category = reader.GetString("category"),
                        Subcategory = reader.GetString("subcategory"),
                        Password = reader.GetString("password"),
                        DateOfBirth = reader.GetString("birthday")
                    };
                }
                return null;
            }
            catch { return null; }
            finally { _connection.Close(); }
        }

        public List<ContactDto>? GetAllContacts()
        {
            try
            {
                List<ContactDto> contacts = new List<ContactDto>();
                _connection.Open();
                string query = "SELECT * FROM contacts";
                using MySqlCommand cmd = new(query, _connection);

                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ContactDto contact = new ContactDto();
                    contact.Name = reader.GetString("name");
                    contact.Surname = reader.GetString("surname");
                    contact.Email = reader.GetString("email");
                    contact.Phone = reader.GetInt32("phone").ToString();
                    contact.Category = reader.GetString("category");
                    contact.Subcategory = reader.GetString("subcategory");
                    contact.Password = reader.GetString("password");
                    contact.DateOfBirth = reader.GetString("birthday");
                    contacts.Add(contact);
                }
                return contacts;
            }
            catch { return null; }
            finally { _connection.Close(); }
        }

        public bool InsertContact(ContactDto contact)
        {
            try
            {
                _connection.Open();
                string query = "INSERT INTO contacts VALUES (@Name, @Surname, @Email, @Password, @Category, @Subcategory, @Phone, @Birthday)";
                using MySqlCommand cmd = new(query, _connection);
                cmd.Parameters.AddWithValue("@Name", contact.Name);
                cmd.Parameters.AddWithValue("@Surname", contact.Surname);
                cmd.Parameters.AddWithValue("@Email", contact.Email);
                cmd.Parameters.AddWithValue("@Category", contact.Category);
                cmd.Parameters.AddWithValue("@Subcategory", contact.Subcategory);
                cmd.Parameters.AddWithValue("@Phone", contact.Phone);
                cmd.Parameters.AddWithValue("@Birthday", contact.DateOfBirth);
                cmd.Parameters.AddWithValue("@Password", contact.Password);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
            finally { _connection.Close(); }
        }

        public void DropContact(string email)
        {
            try
            {
                _connection.Open();
                string query = "DELETE FROM contacts WHERE email = @Email";
                using MySqlCommand cmd = new(query, _connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }
            catch {}
            finally { _connection.Close(); }
        }

        public bool UpdateContact(ContactDto contact, string email)
        {
            try
            {
                _connection.Open();

                string query = "UPDATE contacts SET Name = @Name, Surname = @Surname, password = @Password , Email = @NewEmail, Phone = @Phone, category = @Category, Subcategory = @Subcategory, birthday = @DateOfBirth WHERE Email = @Email";
                using MySqlCommand cmd = new MySqlCommand(query, _connection);

                cmd.Parameters.AddWithValue("@Name", contact.Name);
                cmd.Parameters.AddWithValue("@Surname", contact.Surname);
                cmd.Parameters.AddWithValue("@NewEmail", contact.Email);
                cmd.Parameters.AddWithValue("@Phone", contact.Phone);
                cmd.Parameters.AddWithValue("@Category", contact.Category);
                cmd.Parameters.AddWithValue("@Subcategory", contact.Subcategory);
                cmd.Parameters.AddWithValue("@DateOfBirth", contact.DateOfBirth);
                cmd.Parameters.AddWithValue("@Password", contact.Password);
                cmd.Parameters.AddWithValue("@Email", email);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch { return false; }
            finally { _connection.Close(); }
        }
    }
}
