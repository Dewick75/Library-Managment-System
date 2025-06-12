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
    public partial class BooksIssue : Form
    {

        private readonly string connectionString = "Data Source=DESKTOP-133\\SQLEXPRESS;Initial Catalog=C153091DB;Integrated Security=True;";
        public BooksIssue()
        {
            InitializeComponent();
            LoadMembers();
            LoadCopyNumbers();
        }

        private void LoadMembers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT MemberId, Name FROM Members";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                cmbUsers.DisplayMember = "Name";
                cmbUsers.ValueMember = "MemberId";
                cmbUsers.DataSource = dataTable;

                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "MemberId";
                comboBox2.DataSource = dataTable;
            }
        }

        private void LoadCopyNumbers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT bc.CopyId, bc.CopyNumber, b.Title, 
                           CASE WHEN bc.Available = 1 THEN 'Available' ELSE 'Borrowed' END AS Status
                    FROM BookCopies bc
                    JOIN Books b ON bc.BookId = b.BookId";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Add a display column combining copy number and status
                dataTable.Columns.Add("CopyDisplay", typeof(string),
                    "CopyNumber + ' - ' + Title + ' (' + Status + ')'");

                cmbCopyNumbers.DisplayMember = "CopyDisplay";
                cmbCopyNumbers.ValueMember = "CopyId";
                cmbCopyNumbers.DataSource = dataTable;

                returnCombo.DisplayMember = "CopyDisplay";
                returnCombo.ValueMember = "CopyId";
                returnCombo.DataSource = dataTable;
            }
        }


        private void LoadAvailableCopies()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT bc.CopyId, bc.CopyNumber, b.Title, 
                           CASE WHEN bc.Available = 1 THEN 'Available' ELSE 'Borrowed' END AS Status
                    FROM BookCopies bc
                    JOIN Books b ON bc.BookId = b.BookId";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Add a display column combining copy number and status
                dataTable.Columns.Add("CopyDisplay", typeof(string),
                    "CopyNumber + ' - ' + Title + ' (' + Status + ')'");

                cmbCopyNumbers.DisplayMember = "CopyDisplay";
                cmbCopyNumbers.ValueMember = "CopyId";
                cmbCopyNumbers.DataSource = dataTable;

                returnCombo.DisplayMember = "CopyDisplay";
                returnCombo.ValueMember = "CopyId";
                returnCombo.DataSource = dataTable;
            }
        }


        private void LoanBook_Click(object sender, EventArgs e)
        {
            if (cmbUsers.SelectedValue == null)
            {
                MessageBox.Show("Please select a member.");
                return;
            }

            string memberId = cmbUsers.SelectedValue.ToString();
            int copyId = (int)cmbCopyNumbers.SelectedValue;

            LoanBooks(memberId, copyId);
            DisplayUserLoans(memberId);
            LoadAvailableCopies();
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Please select a member.");
                return;
            }

            string memberId = comboBox2.SelectedValue.ToString();
            int copyId = (int)returnCombo.SelectedValue;

            ReturnBook(memberId, copyId);
            DisplayUserLoans(memberId);
            LoadAvailableCopies();
        }


        private void LoanBooks(string memberId, int copyId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the book is available and if the member can borrow more books
                string checkQuery = @"
                    SELECT COUNT(*) FROM Loans WHERE MemberId = @MemberId AND ReturnDate IS NULL;
                    SELECT Available FROM BookCopies WHERE CopyId = @CopyId;
                    SELECT Borrowable FROM Books b
                    JOIN BookCopies bc ON b.BookId = bc.BookId
                    WHERE bc.CopyId = @CopyId;";

                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@MemberId", memberId);
                checkCommand.Parameters.AddWithValue("@CopyId", copyId);

                using (SqlDataReader reader = checkCommand.ExecuteReader())
                {
                    reader.Read();
                    int loanCount = reader.GetInt32(0);
                    reader.NextResult();
                    reader.Read();
                    bool isAvailable = reader.GetBoolean(0);
                    reader.NextResult();
                    reader.Read();
                    bool isBorrowable = reader.GetBoolean(0);

                    if (loanCount >= 5)
                    {
                        MessageBox.Show("Member has already borrowed the maximum number of books.");
                        return;
                    }

                    if (!isAvailable)
                    {
                        MessageBox.Show("The book is not available for loan.");
                        return;
                    }

                    if (!isBorrowable)
                    {
                        MessageBox.Show("This book cannot be borrowed.");
                        return;
                    }
                }

                // Loan the book
                string loanQuery = @"
                    INSERT INTO Loans (MemberId, CopyId, LoanDate, ReturnDate)
                    VALUES (@MemberId, @CopyId, @LoanDate, @ReturnDate);
                    
                    UPDATE BookCopies 
                    SET Available = 0
                    WHERE CopyId = @CopyId;";

                SqlCommand loanCommand = new SqlCommand(loanQuery, connection);
                loanCommand.Parameters.AddWithValue("@MemberId", memberId);
                loanCommand.Parameters.AddWithValue("@CopyId", copyId);
                loanCommand.Parameters.AddWithValue("@LoanDate", DateTime.Now);
                loanCommand.Parameters.AddWithValue("@ReturnDate", DateTime.Now.AddDays(14));

                int rowsAffected = loanCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Book loaned successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to loan the book.");
                }
            }
        }


        private void ReturnBook(string memberId, int copyId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string returnQuery = @"
                    UPDATE Loans 
                    SET ReturnDate = @ActualReturnDate
                    WHERE CopyId = @CopyId 
                    AND MemberId = @MemberId
                    AND ReturnDate > @ActualReturnDate;

                    UPDATE BookCopies 
                    SET Available = 1
                    WHERE CopyId = @CopyId;";

                SqlCommand returnCommand = new SqlCommand(returnQuery, connection);
                returnCommand.Parameters.AddWithValue("@MemberId", memberId);
                returnCommand.Parameters.AddWithValue("@CopyId", copyId);
                returnCommand.Parameters.AddWithValue("@ActualReturnDate", DateTime.Now);

                int rowsAffected = returnCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Book returned successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to return the book.");
                }
            }
        }



        private void DisplayUserLoans(string memberId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT 
                            l.LoanId, 
                            m.MemberId, 
                            m.Name AS MemberName, 
                            bc.CopyId, 
                            bc.CopyNumber, 
                            b.BookId, 
                            b.Title, 
                            b.Publisher, 
                            b.Year, 
                            l.LoanDate, 
                            l.ReturnDate 
                        FROM Loans l
                        INNER JOIN Members m ON l.MemberId = m.MemberId
                        INNER JOIN BookCopies bc ON l.CopyId = bc.CopyId
                        INNER JOIN Books b ON bc.BookId = b.BookId
                        WHERE l.MemberId = @MemberId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MemberId", memberId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dgvBorrowedBooks.DataSource = dataTable;

                // Auto-adjust column width for better visibility
                dgvBorrowedBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loan_Return DashBoardForm = new Loan_Return();
            DashBoardForm.Show();
            this.Hide();
        }
    }
}
