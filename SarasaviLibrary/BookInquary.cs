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

namespace SarasaviLibrary
{
    public partial class BookInquary : Form
    {
        public BookInquary()
        {
            InitializeComponent();
        }
        // Connection string to the SQL Server database
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-133\SQLEXPRESS;Initial Catalog=C153091DB;Integrated Security=True;");

        // Event handler for the search button click event
        private void button1_Click(object sender, EventArgs e)
        {
            string searchKeyword = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(searchKeyword))
            {
                MessageBox.Show("Please enter a search keyword.");
                return;
            }

            SearchBooks(searchKeyword);
        }


        private void SearchBooks(string keyword)
        {
            try
            {
                connection.Open();
                string query = @"SELECT b.BookId, b.Title, b.Publisher, b.Year, 
                                    b.Borrowable, b.Reference, 
                                    (SELECT COUNT(*) FROM BookCopies WHERE BookId = b.BookId AND Available = 1) AS AvailableCopies
                                 FROM Books b
                                 WHERE b.BookId LIKE @keyword OR b.Title LIKE @keyword OR b.Publisher LIKE @keyword";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                    dataGridViewBooks.DataSource = dt;
                else
                    MessageBox.Show("No books found matching your search.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


     

        // Event handler for the dashboard button click event
        private void button2_Click(object sender, EventArgs e)
        {
            Loan_Return dashBoardForm = new Loan_Return();
            dashBoardForm.Show();
            this.Hide();
        }
    }
}
