using System;
using System.Windows;
using System.Windows.Controls;
using Model;
using MoviesInterface;

namespace HammerMtheater.Pages
{
    public partial class Login : Page
    {
        private bool _isPasswordVisible = false;

        public Login()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            // 1. Correctly capture the password from whichever box is currently active
            string password = _isPasswordVisible ? PasswordVisible.Text : PasswordHidden.Password;

            // 2. Validate that no fields are empty
            if (string.IsNullOrWhiteSpace(UsernameBox.Text) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                ErrorText.Text = "Please fill all fields";
                return;
            }

            try
            {
                MoviesFunctions api = new MoviesFunctions();
                UserList users = await api.GetAllUsers();

                if (users == null || users.Count == 0)
                {
                    MessageBox.Show("No users found in database.", "Login Error");
                    return;
                }

                // 3. Search for the user with matching credentials
                // Using .Trim() prevents login failure from accidental trailing spaces
                User currentUser = users.Find(u =>
                    u.Username.Trim() == UsernameBox.Text.Trim() &&
                    u.Pass == password &&
                    u.Email.Trim() == EmailBox.Text.Trim()
                );

                if (currentUser != null)
                {
                    App.CurrentUser = currentUser;

                   
                    if (currentUser.Roleid == 7)
                    {
                        MessageBox.Show("Welcome, Operator!", "Admin Access");
                        NavigationService.Navigate(new OperatorDashboard());
                    }
                    else
                    {
                        MessageBox.Show("Login successful!", "Success");
                        NavigationService.Navigate(new HomePage());
                    }
                }
                else
                {
                    ErrorText.Text = "Invalid username, email, or password";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Connection Error:\n\n" + ex.Message, "Error");
            }
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                PasswordHidden.Password = PasswordVisible.Text;
                PasswordVisible.Visibility = Visibility.Collapsed;
                PasswordHidden.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
            }
            else
            {
                PasswordVisible.Text = PasswordHidden.Password;
                PasswordHidden.Visibility = Visibility.Collapsed;
                PasswordVisible.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
            }

            _isPasswordVisible = !_isPasswordVisible;
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Signup());
        }

       

        private void skip_Click_admin(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = "admin";
            PasswordHidden.Password = "12341234";
            EmailBox.Text = "admin@gmail.com";
        }

        private void skip_Click_user(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = "test";
            PasswordHidden.Password = "1234";
            EmailBox.Text = "test@mail.com";
        }
    }
}
