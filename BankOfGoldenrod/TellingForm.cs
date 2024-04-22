using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Mysqlx;
using Mysqlx.Crud;

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
            SslMode = MySqlSslMode.Required
        };
        public TellingForm(List<string> name)
        {
            InitializeComponent();
            Load += FormLoad;
            this.FormClosing += ExitApplication;
            theName = name;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            PopulateHolderComboBox();
            PopulateAccountComboBox();
            PopulateDeleteAndReopenComboBox();
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


            var holder = chooseAddAccountHolderComboBox.SelectedItem;
            var accSelection = chooseAddAccountComboBox.SelectedItem;
            var check = amountAddTextBox.Text;
            decimal amount;
            decimal funds;
            string tellerId = GetTellerId();

            if (holder != null && accSelection != null)
            {
                if (decimal.TryParse(check, out amount) && amount > 0)
                {
                    string fullName = holder.ToString();
                    string[] splitName = fullName.Split(" ");
                    string firstName = splitName[0];
                    string lastName = splitName[1];
                    string account = accSelection.ToString();
                    string holderId = GetHolderId(firstName, lastName);

                    funds = GetFunds(account, firstName, lastName);

                    MessageBox.Show($"The funds retrieved are {funds}");

                    funds += amount;

                    MessageBox.Show($"New account amount is {funds}");

                    if (account == "Checking Account")
                    {
                        connection.Open();

                        string updateQuery = "update account_holders set checkingAccount = @funds where " +
                            "firstName = @firstName and lastName = @lastName;";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@funds", funds);
                            updateCommand.Parameters.AddWithValue("@firstName", firstName);
                            updateCommand.Parameters.AddWithValue("@lastName", lastName);

                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Funds updated successfully");
                                connection.Close();
                                chooseAddAccountComboBox.ResetText();
                                chooseAddAccountHolderComboBox.ResetText();
                                amountAddTextBox.ResetText();
                                connection.Open();

                                string insertTransactionQuery = "insert into transaction_history (accountId, tellerId, " +
                                    "tellerFirstName, tellerLastName, amount) values (@accountId, @tellerId, @tellerFirstName, " +
                                    "@tellerLastName, @amount);";

                                using (MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionQuery, connection))
                                {
                                    insertTransactionCommand.Parameters.AddWithValue("@accountId", holderId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerId", tellerId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerFirstName", theName[0]);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerLastName", theName[1]);
                                    insertTransactionCommand.Parameters.AddWithValue("@amount", amount);

                                    int rows = insertTransactionCommand.ExecuteNonQuery();

                                    if (rows > 0)
                                    {
                                        MessageBox.Show("Transaction posted successfully!");
                                        connection.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error posting transaction!");
                                        connection.Close();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error updating funds");
                                connection.Close();
                            }
                        }
                    }
                    else if (account == "Savings Account")
                    {
                        connection.Open();

                        string updateQuery = "update account_holders set savingAccount = @funds where " +
                            "firstName = @firstName and lastName = @lastName;";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@funds", funds);
                            updateCommand.Parameters.AddWithValue("@firstName", firstName);
                            updateCommand.Parameters.AddWithValue("@lastName", lastName);

                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Funds updated successfully");
                                connection.Close();
                                chooseAddAccountComboBox.ResetText();
                                chooseAddAccountHolderComboBox.ResetText();
                                amountAddTextBox.ResetText();
                                connection.Open();

                                string insertTransactionQuery = "insert into transaction_history (accountId, tellerId, " +
                                    "tellerFirstName, tellerLastName, amount) values (@accountId, @tellerId, @tellerFirstName, " +
                                    "@tellerLastName, @amount);";

                                using (MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionQuery, connection))
                                {
                                    insertTransactionCommand.Parameters.AddWithValue("@accountId", holderId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerId", tellerId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerFirstName", theName[0]);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerLastName", theName[1]);
                                    insertTransactionCommand.Parameters.AddWithValue("@amount", amount);

                                    int rows = insertTransactionCommand.ExecuteNonQuery();

                                    if (rows > 0)
                                    {
                                        MessageBox.Show("Transaction posted successfully!");
                                        connection.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error posting transaction!");
                                        connection.Close();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error updating funds");
                                connection.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error after retreiving funds.");
                    }
                }
                else
                {
                    MessageBox.Show("Enter a decimal amount greater than 0!");
                }
            }
            else
            {
                MessageBox.Show("Make sure to select an account holder and an account type!");
            }
        }

        private void confirmRemoveButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            var holder = chooseRemoveAccountHolderComboBox.SelectedItem;
            var accSelection = chooseRemoveAccountComboBox.SelectedItem;
            var check = amountRemoveTextBox.Text;
            decimal amount;
            string tellerId = GetTellerId();

            if (holder != null && accSelection != null)
            {
                string fullName = holder.ToString();
                string[] splitName = fullName.Split(" ");
                string firstName = splitName[0];
                string lastName = splitName[1];
                string account = accSelection.ToString();
                string holderId = GetHolderId(firstName, lastName);
                decimal funds = GetFunds(account, firstName, lastName);

                if (decimal.TryParse(check, out amount) && amount < 0 && (amount * -1) <= funds)
                {

                    MessageBox.Show($"The funds retrieved are {funds}");

                    funds += amount;

                    MessageBox.Show($"New account amount is {funds}");

                    if (account == "Checking Account")
                    {
                        connection.Open();

                        string updateQuery = "update account_holders set checkingAccount = @funds where " +
                            "firstName = @firstName and lastName = @lastName;";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@funds", funds);
                            updateCommand.Parameters.AddWithValue("@firstName", firstName);
                            updateCommand.Parameters.AddWithValue("@lastName", lastName);

                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Funds updated successfully");
                                connection.Close();
                                chooseRemoveAccountComboBox.ResetText();
                                chooseRemoveAccountHolderComboBox.ResetText();
                                amountRemoveTextBox.ResetText();
                                connection.Open();

                                string insertTransactionQuery = "insert into transaction_history (accountId, tellerId, " +
                                    "tellerFirstName, tellerLastName, amount) values (@accountId, @tellerId, @tellerFirstName, " +
                                    "@tellerLastName, @amount);";

                                using (MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionQuery, connection))
                                {
                                    insertTransactionCommand.Parameters.AddWithValue("@accountId", holderId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerId", tellerId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerFirstName", theName[0]);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerLastName", theName[1]);
                                    insertTransactionCommand.Parameters.AddWithValue("@amount", amount);

                                    int rows = insertTransactionCommand.ExecuteNonQuery();

                                    if (rows > 0)
                                    {
                                        MessageBox.Show("Transaction posted successfully!");
                                        connection.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error posting transaction!");
                                        connection.Close();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error updating funds");
                                connection.Close();
                            }
                        }
                    }
                    else if (account == "Savings Account")
                    {
                        connection.Open();

                        string updateQuery = "update account_holders set savingAccount = @funds where " +
                            "firstName = @firstName and lastName = @lastName;";

                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@funds", funds);
                            updateCommand.Parameters.AddWithValue("@firstName", firstName);
                            updateCommand.Parameters.AddWithValue("@lastName", lastName);

                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Funds updated successfully");
                                connection.Close();
                                chooseRemoveAccountComboBox.ResetText();
                                chooseRemoveAccountHolderComboBox.ResetText();
                                amountRemoveTextBox.ResetText();
                                connection.Open();

                                string insertTransactionQuery = "insert into transaction_history (accountId, tellerId, " +
                                    "tellerFirstName, tellerLastName, amount) values (@accountId, @tellerId, @tellerFirstName, " +
                                    "@tellerLastName, @amount);";

                                using (MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionQuery, connection))
                                {
                                    insertTransactionCommand.Parameters.AddWithValue("@accountId", holderId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerId", tellerId);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerFirstName", theName[0]);
                                    insertTransactionCommand.Parameters.AddWithValue("@tellerLastName", theName[1]);
                                    insertTransactionCommand.Parameters.AddWithValue("@amount", amount);

                                    int rows = insertTransactionCommand.ExecuteNonQuery();

                                    if (rows > 0)
                                    {
                                        MessageBox.Show("Transaction posted successfully!");
                                        connection.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error posting transaction!");
                                        connection.Close();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error updating funds");
                                connection.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error after retreiving funds.");
                    }
                }
                else
                {
                    MessageBox.Show("Enter a negative decimal amount to subtract from funds available in account! Make sure the amount subtracted is less than amount available!");
                }
            }
            else
            {
                MessageBox.Show("Make sure to select an account holder and an account type!");
            }
        }

        private void confirmTransferButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            //connection.Open();

            var holder = chooseTransferAccountHolderComboBox.SelectedItem;
            var accSelection = chooseTransferAccountComboBox.SelectedItem;
            var check = amountTransferTextBox.Text;
            var transferSelection = recieveTransferComboBox.SelectedItem;
            
            string tellerId = GetTellerId();
            decimal amount;
            

            if (holder != null && accSelection != null && transferSelection != null)
            {
                string account = accSelection.ToString();
                string transfer = transferSelection.ToString();
                string fullName = holder.ToString();
                string[] splitName = fullName.Split(" ");
                string firstName = splitName[0];
                string lastName = splitName[1];

                if (account != transfer)
                {
                    decimal funds = GetFunds(account, firstName, lastName);
                    decimal transferFunds = GetFunds(transfer, firstName, lastName);

                    if (decimal.TryParse(check, out amount) && amount > 0 && amount <= funds)
                    {
                        funds -= amount;
                        transferFunds += amount;

                        if (account == "Checking Account")
                        {
                            string updateFundsQuery = "update account_holders set checkingAccount = @funds " +
                                "where firstName = @firstName and lastName = @lastName;";
                            string updateTransferFundsQuery = "update account_holders set savingAccount = @transferFunds " +
                                "where firstName = @firstName and lastName = @lastName;";

                            connection.Open();
                            using (MySqlCommand updateFunds = new MySqlCommand(updateFundsQuery, connection))
                            {
                                updateFunds.Parameters.AddWithValue("@funds", funds);
                                updateFunds.Parameters.AddWithValue("@firstName", firstName);
                                updateFunds.Parameters.AddWithValue("@lastName", lastName);

                                int rowsAffected = updateFunds.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Checking Account funds updated!");
                                }
                                else
                                {
                                    MessageBox.Show("Error updating checking account funds!");
                                }
                            }
                            connection.Close();

                            connection.Open();
                            using (MySqlCommand updateTransferFunds  = new MySqlCommand(updateTransferFundsQuery, connection))
                            {
                                updateTransferFunds.Parameters.AddWithValue("@transferFunds", transferFunds);
                                updateTransferFunds.Parameters.AddWithValue("@firstName", firstName);
                                updateTransferFunds.Parameters.AddWithValue("@lastName", lastName);

                                int rowsAffected = updateTransferFunds.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Savings Account funds updated!");
                                }
                                else
                                {
                                    MessageBox.Show("Error updating savings account funds!");
                                }
                            }
                            connection.Close();
                            chooseTransferAccountHolderComboBox.ResetText();
                            chooseTransferAccountComboBox.ResetText();
                            recieveTransferComboBox.ResetText();
                            amountTransferTextBox.ResetText();
                        }
                        else if (account == "Savings Account")
                        {
                            string updateFundsQuery = "update account_holders set savingAccount = @funds " +
                                "where firstName = @firstName and lastName = @lastName;";
                            string updateTransferFundsQuery = "update account_holders set checkingAccount = @transferFunds " +
                                "where firstName = @firstName and lastName = @lastName;";

                            connection.Open();
                            using (MySqlCommand updateFunds = new MySqlCommand(updateFundsQuery, connection))
                            {
                                updateFunds.Parameters.AddWithValue("@funds", funds);
                                updateFunds.Parameters.AddWithValue("@firstName", firstName);
                                updateFunds.Parameters.AddWithValue("@lastName", lastName);

                                int rowsAffected = updateFunds.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Savings Account funds updated!");
                                }
                                else
                                {
                                    MessageBox.Show("Error updating savings account funds!");
                                }
                            }
                            connection.Close();

                            connection.Open();
                            using (MySqlCommand updateTransferFunds = new MySqlCommand(updateTransferFundsQuery, connection))
                            {
                                updateTransferFunds.Parameters.AddWithValue("@transferFunds", transferFunds);
                                updateTransferFunds.Parameters.AddWithValue("@firstName", firstName);
                                updateTransferFunds.Parameters.AddWithValue("@lastName", lastName);

                                int rowsAffected = updateTransferFunds.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Checking Account funds updated!");
                                }
                                else
                                {
                                    MessageBox.Show("Error updating checking account funds!");
                                }
                            }
                            connection.Close();
                            chooseTransferAccountHolderComboBox.ResetText();
                            chooseTransferAccountComboBox.ResetText();
                            recieveTransferComboBox.ResetText();
                            amountTransferTextBox.ResetText();
                        }
                        else
                        {
                            MessageBox.Show("Error retrieving account type!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter a decimal amount greater than 0 and less than  or equal to the amount in account your transferring from!");
                    }
                }
                else
                {
                    MessageBox.Show("Cannot transfer to and from the same account!");
                }
            }
            else
            {
                MessageBox.Show("Make sure all boxes have a selection!");
            }
        }

        private void confirmCloseButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            var holder = chooseCloseAccountHolderComboBox.SelectedItem;
            string tellerId = GetTellerId();
            
            if (holder != null)
            {
                string fullName = holder.ToString();
                string[] splitName = fullName.Split(" ");
                string firstName = splitName[0];
                string lastName = splitName[1];
                decimal checkingFunds = GetFunds("Checking Account", firstName, lastName);
                decimal savingFunds = GetFunds("Savings Account", firstName, lastName);

                if (checkingFunds == 0 && savingFunds == 0)
                {
                    string closeQuery = "update account_holders set isActive = 0 where firstName = @firstName and lastName = @lastName;";

                    using (MySqlCommand closeCommand = new MySqlCommand(closeQuery, connection))
                    {
                        closeCommand.Parameters.AddWithValue("@firstName", firstName);
                        closeCommand.Parameters.AddWithValue("@lastName", lastName);

                        int rowsAffected = closeCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account closed successfully!");
                            chooseCloseAccountHolderComboBox.ResetText();
                            chooseAddAccountHolderComboBox.Items.Clear();
                            chooseCloseAccountHolderComboBox.Items.Clear();
                            chooseRemoveAccountHolderComboBox.Items.Clear();
                            chooseTransferAccountHolderComboBox.Items.Clear();
                            chooseDeleteAccountHolderComboBox.Items.Clear();
                            chooseReopenAccountHolderComboBox.Items.Clear();
                            PopulateHolderComboBox();
                            PopulateDeleteAndReopenComboBox();

                        }
                        else
                        {
                            MessageBox.Show("Error closing account!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Account holder must have no remaining funds in both accounts to close the account!");
                }
            }
            else
            {
                MessageBox.Show("Make sure to select an account holder!");
            }
            connection.Close();
        }

        private void confirmDeleteButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            

            var holder = chooseDeleteAccountHolderComboBox.SelectedItem;

            if (holder != null)
            {
                string fullName = holder.ToString();
                string[] splitName = fullName.Split(" ");
                string firstName = splitName[0];
                string lastName = splitName[1];
                string holderId = GetHolderId(firstName, lastName);

                string deleteTransactionQuery = "delete from transaction_history where accountId = @holderId;";

                connection.Open();

                using (MySqlCommand deleteTransactionCommand = new MySqlCommand(deleteTransactionQuery, connection))
                {
                    deleteTransactionCommand.Parameters.AddWithValue("@holderId", holderId);

                    int rowsAffected = deleteTransactionCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Records deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Error deleting records!");
                    }
                }

                connection.Close();

                string deleteQuery = "delete from account_holders where isActive = 0 and firstName = @firstName " +
                    "and lastName = @lastName;";

                connection.Open();

                using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@firstName", firstName);
                    deleteCommand.Parameters.AddWithValue("@lastName", lastName);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Account deleted successfully!");
                        chooseDeleteAccountHolderComboBox.ResetText();
                        chooseAddAccountHolderComboBox.Items.Clear();
                        chooseCloseAccountHolderComboBox.Items.Clear();
                        chooseRemoveAccountHolderComboBox.Items.Clear();
                        chooseTransferAccountHolderComboBox.Items.Clear();
                        chooseDeleteAccountHolderComboBox.Items.Clear();
                        chooseReopenAccountHolderComboBox.Items.Clear();
                        PopulateHolderComboBox();
                        PopulateDeleteAndReopenComboBox();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting account!");
                    }
                }

                connection.Close();
            }
            else
            {
                MessageBox.Show("Make sure to select an account holder!");
            }
        }

        private void confirmReopenButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            var holder = chooseReopenAccountHolderComboBox.SelectedItem;

            if (holder != null)
            {
                string fullName = holder.ToString();
                string[] splitName = fullName.Split(" ");
                string firstName = splitName[0];
                string lastName = splitName[1];

                string reOpenQuery = "update account_holders set isActive = 1 where firstName = @firstName " +
                    "and lastName = @lastName;";

                using (MySqlCommand reOpenCommand = new MySqlCommand(reOpenQuery, connection))
                {
                    reOpenCommand.Parameters.AddWithValue("@firstName", firstName);
                    reOpenCommand.Parameters.AddWithValue("@lastName", lastName);

                    int rowsAffected = reOpenCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Successfully reopened account!");
                        chooseReopenAccountHolderComboBox.ResetText();
                        chooseAddAccountHolderComboBox.Items.Clear();
                        chooseCloseAccountHolderComboBox.Items.Clear();
                        chooseRemoveAccountHolderComboBox.Items.Clear();
                        chooseTransferAccountHolderComboBox.Items.Clear();
                        chooseDeleteAccountHolderComboBox.Items.Clear();
                        chooseReopenAccountHolderComboBox.Items.Clear();
                        PopulateHolderComboBox();
                        PopulateDeleteAndReopenComboBox();
                    }
                    else
                    {
                        MessageBox.Show("Error reopening account!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Make sure to select an account holder!");
            }
            connection.Close();
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


            if (firstNameTextBox.Text != "" && lastNameTextBox.Text != "" && usernameTextBox.Text != ""
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
                                var checkingAccountCheck = checkingAccountTextBox.Text;
                                var savingAccountCheck = savingsAccountTextBox.Text;
                                decimal checkingAccount;
                                decimal savingsAccount;

                                if (decimal.TryParse(checkingAccountCheck, out checkingAccount) &&
                                    decimal.TryParse(savingAccountCheck, out savingsAccount) &&
                                    checkingAccount > 0 && savingsAccount > 0)
                                {
                                    connection.Open();
                                    using (MySqlCommand command = new MySqlCommand(createQuery, connection))
                                    {
                                        //decimal checkingAccount = Convert.ToDecimal(checkingAccountTextBox.Text);
                                        //decimal savingsAccount = Convert.ToDecimal(savingsAccountTextBox.Text);

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
                                            chooseAddAccountHolderComboBox.Items.Clear();
                                            chooseCloseAccountHolderComboBox.Items.Clear();
                                            chooseRemoveAccountHolderComboBox.Items.Clear();
                                            chooseTransferAccountHolderComboBox.Items.Clear();
                                            PopulateHolderComboBox();

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
                                    MessageBox.Show("Make sure to put decimal amounts in the boxes that are greater than 0!");
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

        private string GetTellerId()
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

        private void PopulateHolderComboBox()
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string getHolderNameQuery = "select firstName, lastName from account_holders where isActive = 1;";

            using (MySqlCommand getHolderNameCommand = new MySqlCommand(getHolderNameQuery, connection))
            {
                using (MySqlDataReader reader = getHolderNameCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string first = reader.GetString(0);
                            string last = reader.GetString(1);
                            string fullName = first + " " + last;

                            chooseAddAccountHolderComboBox.Invoke((MethodInvoker)delegate
                            {
                                chooseAddAccountHolderComboBox.Items.Add(fullName);
                            });

                            chooseCloseAccountHolderComboBox.Invoke((MethodInvoker)delegate
                            {
                                chooseCloseAccountHolderComboBox.Items.Add(fullName);
                            });

                            chooseRemoveAccountHolderComboBox.Invoke((MethodInvoker)delegate
                            {
                                chooseRemoveAccountHolderComboBox.Items.Add(fullName);
                            });

                            chooseTransferAccountHolderComboBox.Invoke((MethodInvoker)delegate
                            {
                                chooseTransferAccountHolderComboBox.Items.Add(fullName);
                            });

                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("No records found");
                    }
                }
            }
            connection.Close();
        }

        private void PopulateDeleteAndReopenComboBox()
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string getHolderQuery = "select firstName, lastName from account_holders where isActive = 0;";

            using (MySqlCommand getHolderCommand = new MySqlCommand(getHolderQuery, connection))
            {
                using (MySqlDataReader reader = getHolderCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string first = reader.GetString(0);
                            string last = reader.GetString(1);
                            string fullName = first + " " + last;

                            chooseDeleteAccountHolderComboBox.Invoke((MethodInvoker)delegate
                            {
                                chooseDeleteAccountHolderComboBox.Items.Add(fullName);
                            });

                            chooseReopenAccountHolderComboBox.Invoke((MethodInvoker)delegate
                            {
                                chooseReopenAccountHolderComboBox.Items.Add(fullName);
                            });
                        }
                    }
                }
            }
            connection.Close();
        }

        private void PopulateAccountComboBox()
        {
            string checkingAccount = "Checking Account";
            string savingAccount = "Savings Account";

            chooseAddAccountComboBox.Invoke((MethodInvoker)delegate
            {
                chooseAddAccountComboBox.Items.Add(checkingAccount);
                chooseAddAccountComboBox.Items.Add(savingAccount);
            });

            chooseRemoveAccountComboBox.Invoke((MethodInvoker)delegate
            {
                chooseRemoveAccountComboBox.Items.Add(checkingAccount);
                chooseRemoveAccountComboBox.Items.Add(savingAccount);
            });

            chooseTransferAccountComboBox.Invoke((MethodInvoker)delegate
            {
                chooseTransferAccountComboBox.Items.Add(checkingAccount);
                chooseTransferAccountComboBox.Items.Add(savingAccount);
            });

            recieveTransferComboBox.Invoke((MethodInvoker)delegate
            {
                recieveTransferComboBox.Items.Add(checkingAccount);
                recieveTransferComboBox.Items.Add(savingAccount);
            });
        }

        private decimal GetFunds(string accountType, string firstName, string lastName)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            decimal funds;

            if (accountType == "Checking Account")
            {
                string getFundsQuery = "Select checkingAccount from account_holders where " +
                    "firstName = @firstName and lastName = @lastName;";

                using (MySqlCommand getFundsCommand = new MySqlCommand(getFundsQuery, connection))
                {
                    getFundsCommand.Parameters.AddWithValue("@firstName", firstName);
                    getFundsCommand.Parameters.AddWithValue("@lastName", lastName);

                    using (MySqlDataReader reader = getFundsCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                funds = reader.GetDecimal(0);
                                MessageBox.Show("Got funds!");
                                //connection.Close();
                                return funds;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error retrieving account!");
                            //connection.Close();
                            return -1;
                        }
                    }
                }
            }
            else if (accountType == "Savings Account")
            {
                string getFundsQuery = "Select savingAccount from account_holders where " +
                    "firstName = @firstName and lastName = @lastName;";

                using (MySqlCommand getFundsCommand = new MySqlCommand(getFundsQuery, connection))
                {
                    getFundsCommand.Parameters.AddWithValue("@firstName", firstName);
                    getFundsCommand.Parameters.AddWithValue("@lastName", lastName);

                    using (MySqlDataReader reader = getFundsCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                funds = reader.GetDecimal(0);
                                MessageBox.Show("Got funds!");
                                //connection.Close();
                                return funds;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error retrieving account!");
                            //connection.Close();
                            return -1;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Error with account selection");
                //connection.Close();
                return -1;
            }
            connection.Close();
            return 0;

        }

        private string GetHolderId(string firstName, string lastName)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string id;
            string getIdQuery = "select accountId from account_holders where " +
                "firstName = @firstName and lastName = @lastName;";

            using (MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection))
            {
                getIdCommand.Parameters.AddWithValue("@firstName", firstName);
                getIdCommand.Parameters.AddWithValue("@lastName", lastName);

                using (MySqlDataReader reader = getIdCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = reader[0].ToString();
                            return id;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error gettind Id");
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
