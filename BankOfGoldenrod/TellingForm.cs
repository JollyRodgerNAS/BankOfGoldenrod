using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BankOfGoldenrod
{
    public partial class TellingForm : Form
    {
        public List<string> theName;
        MySqlConnectionStringBuilder str = new MySqlConnectionStringBuilder()
        {
            Server = "localhost",
            Port = 3306,
            Database = "bank_database",
            UserID = "test",
            Password = "test123",
            SslMode = MySqlSslMode.Disabled
        };
        public TellingForm(List<string> name)
        {
            InitializeComponent();
            this.FormClosing += ExitApplication;
            theName = name;
        }
        private void ExitApplication(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void confirmAddButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string tellerId = GetTellerId();
            MessageBox.Show($"{tellerId}");

            connection.Close();

        }

        

        private void confirmRemoveButton_Click(object sender, EventArgs e)
        {

        }

        private void confirmTransferButton_Click(object sender, EventArgs e)
        {

        }

        private void confirmCloseButton_Click(object sender, EventArgs e)
        {

        }

        private void createNewAccountHolderButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string userName = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string email = emailTextBox.Text;

            DateTime dt;


            if (firstNameTextBox.Text != "" &&  lastNameTextBox.Text != "" && usernameTextBox.Text != ""
                && passwordTextBox.Text != "" && birthdateTextBox.Text != "" && emailTextBox.Text != ""
                && checkingAccountTextBox.Text != "" && savingsAccountTextBox.Text != "")
            {
                string searchQuery = "select firstName, lastName from account_holders where firstName = @firstName and lastName = @lastName;";
                string createQuery = "insert into account_holders (firstName, lastName, userName, password, birthdate, " +
                    "email, checkingAccount, savingAccount, isActive) values (@firstName, @lastName, @userName, @password, @birthdate, " +
                    "@email, @checkingAccount, @savingAccount, @isActive);";

                using (MySqlCommand searchCommand = new MySqlCommand(searchQuery, connection))
                {
                    searchCommand.Parameters.AddWithValue("@firstName", firstName);
                    searchCommand.Parameters.AddWithValue("@lastName", lastName);

                    using (MySqlDataReader reader = searchCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Account Holder already exists!");
                            connection.Close();
                        }
                        else
                        {
                            connection.Close();
                            if (DateTime.TryParse(birthdateTextBox.Text, out dt))
                            {
                                connection.Open();
                                using (MySqlCommand command = new MySqlCommand(createQuery, connection))
                                {
                                    decimal checkingAccount = Convert.ToDecimal(checkingAccountTextBox.Text);
                                    decimal savingsAccount = Convert.ToDecimal(savingsAccountTextBox.Text);

                                    command.Parameters.AddWithValue("@firstName", firstName);
                                    command.Parameters.AddWithValue("@lastName", lastName);
                                    command.Parameters.AddWithValue("@userName", userName);
                                    command.Parameters.AddWithValue("@password", password);
                                    command.Parameters.AddWithValue("@birthdate", dt);
                                    command.Parameters.AddWithValue("@email", email);
                                    command.Parameters.AddWithValue("@checkingAccount", checkingAccount);
                                    command.Parameters.AddWithValue("@savingAccount", savingsAccount);
                                    command.Parameters.AddWithValue("@isActive", true);

                                    int rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        firstNameTextBox.ResetText();
                                        lastNameTextBox.ResetText();
                                        usernameTextBox.ResetText();
                                        passwordTextBox.ResetText();
                                        birthdateTextBox.ResetText();
                                        emailTextBox.ResetText();
                                        checkingAccountTextBox.ResetText();
                                        savingsAccountTextBox.ResetText();
                                        
                                        MessageBox.Show("Success!");

                                        connection.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Fail!");
                                        connection.Close();
                                    }

                                }
                            }
                            else
                            {
                                MessageBox.Show("Incorrect birthday format! Use YYYY-MM-DD");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Make sure all boxes have information entered!");
                connection.Close();
            }
        }

        public string GetTellerId()
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string tellerId;

            string getIdQuery = "select tellerId from bank_tellers where firstName = @firstName and lastName = @lastName;";

            using (MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection))
            {
                getIdCommand.Parameters.AddWithValue("@firstName", theName[0]);
                getIdCommand.Parameters.AddWithValue("@lastName", theName[1]);

                using (MySqlDataReader reader = getIdCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tellerId = reader[0].ToString();
                            return tellerId;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error getting Teller ID");
                        return null;
                    }
                }
            }
            connection.Close();
            return null;
        }
    }
}
