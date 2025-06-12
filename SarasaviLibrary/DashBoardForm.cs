using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SarasaviLibrary
{
    public partial class Loan_Return : Form
    {
        public Loan_Return()
        {
            InitializeComponent(); // Initialize the form components
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            
                DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                
                if (result == DialogResult.Yes)
                {
          
                    Login loginForm = new Login();
                    loginForm.Show();

                    this.Close();
                }
            
        }

        private void Add_books_Click(object sender, EventArgs e)
        {
            BookAdding BookAdding = new BookAdding();
            BookAdding.Show(); // Show the BookAdding form
            this.Hide(); // Hide the Dashboard form
        }

        private void View_Books_Click(object sender, EventArgs e)
        {
            ViewBooks ViewBooks = new ViewBooks();
            ViewBooks.Show(); // Show the BookInquary form
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Members Members = new Members();
            Members.Show(); // Show the BookAdding form
            this.Hide(); // Hide the Dashboard form
        }

        private void View_Members_Click(object sender, EventArgs e)
        {
            RegisterdMembers RegisterdMembers = new RegisterdMembers();
            RegisterdMembers.Show(); // Show the BookInquary form
            this.Hide();
        }

        private void Book_Inquary_Click(object sender, EventArgs e)
        {
            BookInquary BookInquary = new BookInquary();
            BookInquary.Show(); // Show the BookInquary form
            this.Hide(); // Hide the Dashboard form
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BooksIssue BooksIssue = new BooksIssue();
            BooksIssue.Show(); // Show the LoanReturn form
            this.Hide(); // Hide the Dashboard form
        }
    }
}
