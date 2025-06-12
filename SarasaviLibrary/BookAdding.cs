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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SarasaviLibrary
{
    public partial class BookAdding : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-133\SQLEXPRESS;Initial Catalog=C153091DB;Integrated Security=True;");

        public BookAdding()
        {
            InitializeComponent();
            LoadClassifications();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text) || string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (comboBox1.Text.Length > 1)
            {
                MessageBox.Show("Use only one character for classification.");
                return;
            }

            try
            {
                connection.Open();

                var cmd = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM ClassificationTable WHERE Classification = @classification) INSERT INTO ClassificationTable (Classification) VALUES (@classification); SELECT ClassificationId FROM ClassificationTable WHERE Classification = @classification;", connection);
                cmd.Parameters.AddWithValue("@classification", comboBox1.Text);
                int classificationId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO Books (ClassificationId, Title, Publisher, Year, Reference, Borrowable) OUTPUT INSERTED.BookId VALUES (@classificationId, @title, @publisher, @year, @isReference, @isBorrowable);", connection);
                cmd.Parameters.AddWithValue("@classificationId", classificationId);
                cmd.Parameters.AddWithValue("@title", textBox1.Text);
                cmd.Parameters.AddWithValue("@publisher", textBox2.Text);
                cmd.Parameters.AddWithValue("@year", textBox3.Text);
                cmd.Parameters.AddWithValue("@isReference", checkBox.Checked);
                cmd.Parameters.AddWithValue("@isBorrowable", checkBox2.Checked);
                int bookId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("SELECT COUNT(*) FROM BookCopies WHERE BookId = @bookId", connection);
                cmd.Parameters.AddWithValue("@bookId", bookId);
                int existingCopies = (int)cmd.ExecuteScalar();

                int numCopies = (int)numericUpDown1.Value;
                if (existingCopies + numCopies > 10)
                {
                    MessageBox.Show("Maximum Copies Exceeded (10 Copies)");
                    return;
                }

                for (int i = 1; i <= numCopies; i++)
                {
                    cmd = new SqlCommand("INSERT INTO BookCopies (BookId, CopyNumber) VALUES (@bookId, @copyNumber)", connection);
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    cmd.Parameters.AddWithValue("@copyNumber", comboBox1.Text + i.ToString("D4"));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Successfully added copies.");
                ClearFields();
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



        private void ClearFields()
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            checkBox.Checked = false;
            checkBox2.Checked = false;
            numericUpDown1.Value = 0;
        }

        private void LoadClassifications()
        {
            try
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT Classification FROM ClassificationTable ORDER BY Classification", connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read()) comboBox1.Items.Add(reader["Classification"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading classifications: " + ex.Message);
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Event handler for text changes in the book name text box
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Event handler for clicks on label3
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Event handler for clicks on label7
        }
    }
    
}
