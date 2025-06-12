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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent(); 
        }
        private void button1_Click(object sender, EventArgs e)  //Login Button here
        {
            try
            {
                if (Username.Text == "admin@gmail.com" && Password.Text == "admin")
                {
                    // Login successful, open the Dashboard form
                    Loan_Return dashboard = new Loan_Return();
                    dashboard.Show(); 
                    this.Hide(); 
                }
                else
                {
                    // Login failed, display an error message
                    MessageBox.Show("Invalid username or password. Please try again.\n Please Note that :- Use Username = admin@gmail.com and Password = admin to log the admin System panel",
                                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            catch (Exception ex)
            {
                // Display an error message if an exception occurs
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
