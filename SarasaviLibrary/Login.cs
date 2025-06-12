using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace SarasaviLibrary
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent(); 
        }
        private void button1_Click(object sender, EventArgs e)  //Login Button here
        {
            string connectionString = "Data Source=DESKTOP-133\\SQLEXPRESS;Initial Catalog=DatabaseLibrary;Integrated Security=True;"; // Update as needed

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT PasswordHash, PasswordSalt FROM Admins WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", Username.Text.Trim());

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] storedHash = (byte[])reader["PasswordHash"];
                                byte[] storedSalt = (byte[])reader["PasswordSalt"];

                                if (VerifyPassword(Password.Text, storedHash, storedSalt))
                                {
                                    // Login successful
                                    Loan_Return dashboard = new Loan_Return();
                                    dashboard.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


      

        private static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(32); // 256-bit hash
            }
        }

        private static bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            var hash = HashPassword(enteredPassword, storedSalt);
            return hash.SequenceEqual(storedHash);
        }

        private void AdminRegister_Click(object sender, EventArgs e)
        {
            AdminReg AdminReg1 = new AdminReg();
            AdminReg1.Show();
            this.Hide();
        }
    }
}


