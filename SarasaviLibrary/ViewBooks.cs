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
    public partial class ViewBooks : Form
    {
        private SqlConnection connection;
        public ViewBooks()
        {
            InitializeComponent();
            
            // Initialize the SQL connection with the given connection string
            connection = new SqlConnection(@"Data Source=DESKTOP-133\SQLEXPRESS;Initial Catalog=C153091DB;Integrated Security=True;");
            LoadBooks();
        }

        private void LoadBooks()
        {
            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        b.BookId, 
                        b.Title, 
                        b.Publisher, 
                        b.Year, 
                        c.Classification, 
                        b.Reference, 
                        b.Borrowable, 
                        COUNT(bc.CopyId) AS NumberOfCopies
                    FROM Books b
                    INNER JOIN ClassificationTable c ON b.ClassificationId = c.ClassificationId
                    LEFT JOIN BookCopies bc ON b.BookId = bc.BookId
                    GROUP BY b.BookId, b.Title, b.Publisher, b.Year, c.Classification, b.Reference, b.Borrowable
                ", connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable booksTable = new DataTable();
                adapter.Fill(booksTable);

                dataGridViewBooks.DataSource = booksTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading books: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Loan_Return DashBoardForm = new Loan_Return();
            DashBoardForm.Show(); // Show the dashboard form
            this.Hide(); // Hide the current form
        }

        private void dataGridViewBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
