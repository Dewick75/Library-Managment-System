using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SarasaviLibrary
{
    public partial class Members : Form
    {
        private readonly string connectionString = @"Data Source=DESKTOP-133\SQLEXPRESS;Initial Catalog=DatabaseLibrary;Integrated Security=True;";

        public Members()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate all fields
            if (!ValidateInputs())
                return;

            // Check for duplicate MemberId
            if (CheckForLibraryID(textMemberId.Text.Trim()))
            {
                MessageBox.Show("User with this Library Number already exists.", "Duplicate Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Insert new member
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Members (MemberId, Name, Sex, NationalID, Address, Telephone, Email)
                                     VALUES (@MemberId, @Name, @Sex, @NationalID, @Address, @Telephone, @Email)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MemberId", textMemberId.Text.Trim());
                        command.Parameters.AddWithValue("@Name", textName.Text.Trim());
                        command.Parameters.AddWithValue("@Sex", comboGender.Text.Trim());
                        command.Parameters.AddWithValue("@NationalID", textNationalID.Text.Trim());
                        command.Parameters.AddWithValue("@Address", textAddress.Text.Trim());
                        command.Parameters.AddWithValue("@Telephone", textTelephone.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Registered Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering member: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckForLibraryID(string memberId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Members WHERE MemberId = @MemberId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberId", memberId);
                    connection.Open();
                    int existingMembers = (int)command.ExecuteScalar();
                    return existingMembers > 0;
                }
            }
        }

        private bool ValidateInputs()
        {
            // Required fields
            if (string.IsNullOrWhiteSpace(textMemberId.Text) ||
                string.IsNullOrWhiteSpace(textName.Text) ||
                string.IsNullOrWhiteSpace(comboGender.Text) ||
                string.IsNullOrWhiteSpace(textNationalID.Text) ||
                string.IsNullOrWhiteSpace(textAddress.Text) ||
                string.IsNullOrWhiteSpace(textTelephone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Fill in all the fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Email validation
            if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Telephone validation (example: 10 digits)
            if (!Regex.IsMatch(textTelephone.Text.Trim(), @"^\d{10}$"))
            {
                MessageBox.Show("Please enter a valid 10-digit telephone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // National ID validation (example: 9 digits + 'V' or 12 digits)
            if (!Regex.IsMatch(textNationalID.Text.Trim(), @"^(\d{9}[Vv]|\d{12})$"))
            {
                MessageBox.Show("Please enter a valid National ID (e.g., 123456789V or 200012345678).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            textMemberId.Text = "";
            textName.Text = "";
            comboGender.SelectedIndex = -1;
            textNationalID.Text = "";
            textAddress.Text = "";
            textTelephone.Text = "";
            txtEmail.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loan_Return dashBoardForm = new Loan_Return();
            dashBoardForm.Show();
            this.Hide();
        }
    }
}
