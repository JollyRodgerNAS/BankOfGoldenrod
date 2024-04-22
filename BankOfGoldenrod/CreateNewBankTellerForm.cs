using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BankOfGoldenrod
{
    public partial class CreateNewBankTellerForm : Form
    {
        MySqlConnectionStringBuilder str = new MySqlConnectionStringBuilder()
        {
            Server = "localhost",
            Port = 3306,
            Database = "bank_database",
            UserID = "test",
            Password = "test123",
            SslMode = MySqlSslMode.Required
        };
        public CreateNewBankTellerForm()
        {
            InitializeComponent();
            this.FormClosing += ExitApplication;
        }
        private void ExitApplication(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string searchQuery = "select firstName, lastName from bank_tellers where firstName = @firstName and lastName = @lastName;";

            if (firstNameTextBox.Text != "" && lastNameTextBox.Text != "" && usernameTextBox.Text != "" && passwordTextBox.Text != "" && emailTextBox.Text != "")
            {
                string firstname = firstNameTextBox.Text;
                string lastname = lastNameTextBox.Text;

                using (MySqlCommand searchCommand = new MySqlCommand(searchQuery, connection))
                {
                    searchCommand.Parameters.AddWithValue("@firstName", firstname);
                    searchCommand.Parameters.AddWithValue("@lastName", lastname);

                    using (MySqlDataReader reader = searchCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Teller already exists!");
                            connection.Close();
                        }
                        else
                        {
                            CreateTeller();
                            firstNameTextBox.ResetText();
                            lastNameTextBox.ResetText();
                            usernameTextBox.ResetText();
                            passwordTextBox.ResetText();
                            emailTextBox.ResetText();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Make sure data is entered into all fields!");
            }
            connection.Close();

        }

        private void returnToLoginButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            WelcomeForm frm = new WelcomeForm();
            frm.ShowDialog();
        }

        private void CreateTeller()
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string insertQuery = "insert into bank_tellers (firstName, lastName, userName, password, email) values (@firstName, @lastName, @userName, @password, @email);";

            string firstname = firstNameTextBox.Text;
            string lastname = lastNameTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string email = emailTextBox.Text;

            using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@firstName", firstname);
                command.Parameters.AddWithValue("@lastName", lastname);
                command.Parameters.AddWithValue("@userName", username);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@email", email);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Teller created successfully!");
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Error creating teller!");
                    connection.Close();
                }
            }
        }
    }
}
