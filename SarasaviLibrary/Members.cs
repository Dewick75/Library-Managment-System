using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SarasaviLibrary
{
    public partial class Members : Form
    {
        public Members()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-133\SQLEXPRESS;Initial Catalog=DatabaseLibrary;Integrated Security=True;");

        private void button1_Click(object sender, EventArgs e)
        {
            if (textName.Text != "" && comboGender.Text != "" && textNationalID.Text != "" && textAddress.Text != "" && textTelephone.Text != "" && textMemberId.Text != "")
            {
                int existingMembers = checkForLibraryID(textMemberId.Text);

                if (existingMembers != 1)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Members (MemberId, Name, Sex, NationalID, Address, Telephone, Email) VALUES (@MemberId, @Name, @Sex, @NationalID, @Address, @Telephone, @Email)", connection);
                    command.Parameters.AddWithValue("@MemberId", textMemberId.Text);
                    command.Parameters.AddWithValue("@Name", textName.Text);
                    command.Parameters.AddWithValue("@Sex", comboGender.Text);
                    command.Parameters.AddWithValue("@NationalID", textNationalID.Text);
                    command.Parameters.AddWithValue("@Address", textAddress.Text);
                    command.Parameters.AddWithValue("@Telephone", textTelephone.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);

                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Registered Successfully");
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("User with this Library Number already exists.");
                }
            }
            else
            {
                MessageBox.Show("Fill in all the fields.");
            }
        }

        private int checkForLibraryID(string userNumber)
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM Members WHERE MemberId = @MemberId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MemberId", userNumber);
            int ExsistingMembers = (int)command.ExecuteScalar();
            connection.Close();
            return ExsistingMembers;
        }

        private void ClearFields()
        {
            textMemberId.Text = "";
            textAddress.Text = "";
            comboGender.Text = "";
            textTelephone.Text = "";
         
            textName.Text = "";
            textNationalID.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loan_Return DashBoardForm = new Loan_Return();
            DashBoardForm.Show();
            this.Hide();
        }
    }
}
