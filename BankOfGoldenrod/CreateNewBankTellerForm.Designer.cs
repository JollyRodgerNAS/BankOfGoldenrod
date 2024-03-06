namespace BankOfGoldenrod
{
    partial class CreateNewBankTellerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            enterInfoLabel = new Label();
            firstNameLabel = new Label();
            lastNameLabel = new Label();
            usernameLabel = new Label();
            passwordLabel = new Label();
            emailLabel = new Label();
            firstNameTextBox = new TextBox();
            lastNameTextBox = new TextBox();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            emailTextBox = new TextBox();
            createButton = new Button();
            SuspendLayout();
            // 
            // enterInfoLabel
            // 
            enterInfoLabel.AutoSize = true;
            enterInfoLabel.Location = new Point(252, 9);
            enterInfoLabel.Margin = new Padding(5, 0, 5, 0);
            enterInfoLabel.Name = "enterInfoLabel";
            enterInfoLabel.Size = new Size(160, 30);
            enterInfoLabel.TabIndex = 0;
            enterInfoLabel.Text = "Enter teller info:";
            // 
            // firstNameLabel
            // 
            firstNameLabel.AutoSize = true;
            firstNameLabel.Location = new Point(12, 67);
            firstNameLabel.Name = "firstNameLabel";
            firstNameLabel.Size = new Size(165, 30);
            firstNameLabel.TabIndex = 1;
            firstNameLabel.Text = "Enter first name:";
            // 
            // lastNameLabel
            // 
            lastNameLabel.AutoSize = true;
            lastNameLabel.Location = new Point(12, 120);
            lastNameLabel.Name = "lastNameLabel";
            lastNameLabel.Size = new Size(162, 30);
            lastNameLabel.TabIndex = 2;
            lastNameLabel.Text = "Enter last name:";
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new Point(12, 173);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(163, 30);
            usernameLabel.TabIndex = 3;
            usernameLabel.Text = "Enter username:";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(12, 226);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(159, 30);
            passwordLabel.TabIndex = 4;
            passwordLabel.Text = "Enter password:";
            // 
            // emailLabel
            // 
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(12, 279);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(122, 30);
            emailLabel.TabIndex = 5;
            emailLabel.Text = "Enter email:";
            // 
            // firstNameTextBox
            // 
            firstNameTextBox.Location = new Point(209, 64);
            firstNameTextBox.Name = "firstNameTextBox";
            firstNameTextBox.Size = new Size(173, 35);
            firstNameTextBox.TabIndex = 6;
            // 
            // lastNameTextBox
            // 
            lastNameTextBox.Location = new Point(209, 117);
            lastNameTextBox.Name = "lastNameTextBox";
            lastNameTextBox.Size = new Size(173, 35);
            lastNameTextBox.TabIndex = 7;
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(209, 170);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(173, 35);
            usernameTextBox.TabIndex = 8;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(209, 223);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(173, 35);
            passwordTextBox.TabIndex = 9;
            // 
            // emailTextBox
            // 
            emailTextBox.Location = new Point(209, 276);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(173, 35);
            emailTextBox.TabIndex = 10;
            // 
            // createButton
            // 
            createButton.Location = new Point(252, 342);
            createButton.Name = "createButton";
            createButton.Size = new Size(163, 55);
            createButton.TabIndex = 11;
            createButton.Text = "Create";
            createButton.UseVisualStyleBackColor = true;
            createButton.Click += createButton_Click;
            // 
            // CreateNewBankTellerForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Goldenrod;
            ClientSize = new Size(727, 424);
            Controls.Add(createButton);
            Controls.Add(emailTextBox);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(lastNameTextBox);
            Controls.Add(firstNameTextBox);
            Controls.Add(emailLabel);
            Controls.Add(passwordLabel);
            Controls.Add(usernameLabel);
            Controls.Add(lastNameLabel);
            Controls.Add(firstNameLabel);
            Controls.Add(enterInfoLabel);
            Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5, 6, 5, 6);
            Name = "CreateNewBankTellerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Create New";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label enterInfoLabel;
        private Label firstNameLabel;
        private Label lastNameLabel;
        private Label usernameLabel;
        private Label passwordLabel;
        private Label emailLabel;
        private TextBox firstNameTextBox;
        private TextBox lastNameTextBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private TextBox emailTextBox;
        private Button createButton;
    }
}