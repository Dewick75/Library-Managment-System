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



        //private void Loads()
        //{
        //    try
        //    {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand("SELECT MemberId, Name, Sex, NationalID, Address, Telephone FROM Members", connection);
        //        SqlDataAdapter adapter = new SqlDataAdapter(command);
        //        DataTable dataTable = new DataTable();
        //        adapter.Fill(dataTable);

        //        Table.DataSource = dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred while loading registered members: " + ex.Message);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        private void Loads(string search = "")
        {
            try
            {
                connection.Open();
                string query = "SELECT MemberId, Name, Sex, NationalID, Address, Telephone FROM Members";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query += " WHERE Name LIKE @Search OR MemberId LIKE @Search";
                }
                SqlCommand command = new SqlCommand(query, connection);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    command.Parameters.AddWithValue("@Search", "%" + search + "%");
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                Table.DataSource = dataTable;
                connection.Close();
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

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            // Open the Members form as a dialog for adding a new member
            using (var addForm = new Members())
            {
                Members membersForm = new Members();
                membersForm.Show();
                this.Hide();

            }
        }

        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            if (Table.CurrentRow == null)
            {
                MessageBox.Show("Please select a member to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var memberId = Table.CurrentRow.Cells["MemberId"].Value.ToString();
            if (MessageBox.Show("Are you sure you want to delete this member?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Members WHERE MemberId=@MemberId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@MemberId", memberId);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Member deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                    Loads();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting member: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Loads(txtSearch.Text.Trim());
        }
    }
}
