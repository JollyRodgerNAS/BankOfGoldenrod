using MySql.Data.MySqlClient;

namespace BankOfGoldenrod
{
    public partial class WelcomeForm : Form
    {
        MySqlConnectionStringBuilder str = new MySqlConnectionStringBuilder()
        {
            Server = "localhost",
            Port = 3306,
            Database = "bank_database",
            UserID = "test",
            Password = "test123",
            SslMode = MySqlSslMode.Disabled
        };
        public WelcomeForm()
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
        private void loginButton_Click(object sender, EventArgs e)
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            if (usernameTextBox.Text != "" && passwordTextBox.Text != "")
            {
                string username = usernameTextBox.Text;
                string password = passwordTextBox.Text;

                string query = "select userName, password from bank_tellers where userName = @userName and password = @password;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("userName", username);
                    command.Parameters.AddWithValue("password", password);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Login successful!");
                            this.Hide();
                            TellingForm frm = new TellingForm();
                            frm.ShowDialog();
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Username or password is incorrect!");
                            connection.Close();
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Input both username and password");
            }

            connection.Close();


        }

        private void createNewLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            CreateNewBankTellerForm frm = new CreateNewBankTellerForm();
            frm.ShowDialog();
        }
    }
}
