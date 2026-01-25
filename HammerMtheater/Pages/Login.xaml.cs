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

            string password = _isPasswordVisible
                ? PasswordVisible.Text
                : PasswordHidden.Password;

            // בדיקת שדות
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

                // ⬇️ שליפת כל המשתמשים מה־DB
                UserList users = await api.GetAllUsers();

                if (users == null || users.Count == 0)
                {
                    MessageBox.Show(
                        "Connected to server, but no users were returned.\nCheck database.",
                        "Login Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                // ⬇️ חיפוש משתמש תואם
                User currentUser = users.Find(u =>
                    u.Username == UsernameBox.Text.Trim() &&
                    u.Pass == password &&
                    u.Email == EmailBox.Text.Trim()
                );

                if (currentUser != null)
                {
                    MessageBox.Show("Login successful!", "Success");

                    // מעבר לדף הבית
                    NavigationService.Navigate(new HomePage());
                }
                else
                {
                    ErrorText.Text = "Invalid username, email or password";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "❌ Failed to connect to server:\n\n" + ex.Message,
                    "Connection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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

        private void skip_Click(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = "admin";
            PasswordHidden.Password = "12341234";
            EmailBox.Text = "admin@gmail.com";
        }
    }
}
