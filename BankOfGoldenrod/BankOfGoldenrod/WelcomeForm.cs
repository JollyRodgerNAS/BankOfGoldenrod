using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BankOfGoldenrod
{
    public partial class WelcomeForm : Form
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
                    command.Parameters.AddWithValue("@userName", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            connection.Close();
                            
                            MessageBox.Show("Login successful!");
                            theName = NameCapture();
                            MessageBox.Show($"{theName[0]} {theName[1]}");
                            this.Hide();
                            TellingForm frm = new TellingForm(theName);
                            frm.ShowDialog();
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

        private List<string> NameCapture()
        {
            string connectionString = str.ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            List<string> tellerName = new List<string>();

            string nameQuery = "select firstName, lastName from bank_tellers where userName = @userName and password = @password;";
            using (MySqlCommand loginCommand = new MySqlCommand(nameQuery, connection))
            {
                loginCommand.Parameters.AddWithValue("@userName", username);
                loginCommand.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader nameReader = loginCommand.ExecuteReader())
                {
                    if (nameReader.HasRows)
                    {
                        MessageBox.Show("Name captured!");
                        while (nameReader.Read())
                        {
                            tellerName.Add(nameReader[0].ToString());
                            tellerName.Add(nameReader[1].ToString());
                        }
                        MessageBox.Show($"{tellerName[0]}");
                        MessageBox.Show($"{tellerName[1]}");
                        connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Name not captured!");
                        connection.Close();
                    }
                }
            }
            return tellerName;
        }
    }
}
