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
    public partial class RegisterdMembers : Form
    {
        public RegisterdMembers()
        {
            InitializeComponent();
            Loads();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-133\SQLEXPRESS;Initial Catalog=DatabaseLibrary;Integrated Security=True;");



        private void Loads()
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT MemberId, Name, Sex, NationalID, Address, Telephone FROM Members", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                Table.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading registered members: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loan_Return DashBoardForm = new Loan_Return();
            DashBoardForm.Show();
            this.Hide();
        }

        private void Table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
