namespace BankOfGoldenrod
{
    partial class WelcomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            welcomeLabel = new Label();
            enterInfoLabel = new Label();
            usernameLabel = new Label();
            passwordLabel = new Label();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            createNewLabel = new LinkLabel();
            loginButton = new Button();
            SuspendLayout();
            // 
            // welcomeLabel
            // 
            welcomeLabel.AutoSize = true;
            welcomeLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            welcomeLabel.Location = new Point(120, 9);
            welcomeLabel.Name = "welcomeLabel";
            welcomeLabel.Size = new Size(309, 30);
            welcomeLabel.TabIndex = 0;
            welcomeLabel.Text = "Welcome to Bank of Goldenrod!";
            // 
            // enterInfoLabel
            // 
            enterInfoLabel.AutoSize = true;
            enterInfoLabel.Location = new Point(120, 64);
            enterInfoLabel.Name = "enterInfoLabel";
            enterInfoLabel.Size = new Size(221, 30);
            enterInfoLabel.TabIndex = 1;
            enterInfoLabel.Text = "Enter login info below:";
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new Point(12, 128);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(163, 30);
            usernameLabel.TabIndex = 2;
            usernameLabel.Text = "Enter username:";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(12, 186);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(159, 30);
            passwordLabel.TabIndex = 3;
            passwordLabel.Text = "Enter password:";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(177, 125);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(160, 35);
            usernameTextBox.TabIndex = 4;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(177, 183);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(160, 35);
            passwordTextBox.TabIndex = 5;
            // 
            // createNewLabel
            // 
            createNewLabel.AutoSize = true;
            createNewLabel.Location = new Point(347, 64);
            createNewLabel.Name = "createNewLabel";
            createNewLabel.Size = new Size(121, 30);
            createNewLabel.TabIndex = 6;
            createNewLabel.TabStop = true;
            createNewLabel.Text = "Create New";
            createNewLabel.LinkClicked += createNewLabel_LinkClicked;
            // 
            // loginButton
            // 
            loginButton.Location = new Point(177, 251);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(160, 59);
            loginButton.TabIndex = 7;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            // 
            // WelcomeForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Goldenrod;
            ClientSize = new Size(549, 369);
            Controls.Add(loginButton);
            Controls.Add(createNewLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(usernameLabel);
            Controls.Add(enterInfoLabel);
            Controls.Add(welcomeLabel);
            Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5, 6, 5, 6);
            Name = "WelcomeForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Welcome Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label welcomeLabel;
        private Label enterInfoLabel;
        private Label usernameLabel;
        private Label passwordLabel;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private LinkLabel createNewLabel;
        private Button loginButton;
    }
}
