using System;
using System.Windows;
using System.Windows.Controls;
using Model;
using MoviesInterface;
using System.Windows.Navigation; // Make sure this is included for NavigationService

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
            string password = _isPasswordVisible ? PasswordVisible.Text : PasswordHidden.Password;

            if (string.IsNullOrWhiteSpace(UsernameBox.Text) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                ErrorText.Text = "Please fill all fields to continue.";
                return;
            }

            try
            {
                (sender as Button).IsEnabled = false;
                ErrorText.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00C896"));
                ErrorText.Text = "Authenticating...";

                MoviesFunctions api = new MoviesFunctions();
                UserList users = await api.GetAllUsers();

                if (users == null || users.Count == 0)
                {
                    SetErrorMessage("Database connection established, but no users found.");
                    return;
                }

                // Search for user - using .Equals and StringComparison for safer matching
                User currentUser = users.Find(u =>
                    u.Username?.Trim().Equals(UsernameBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true &&
                    u.Pass == password.Trim() &&
                    u.Email?.Trim().Equals(EmailBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true
                );

                if (currentUser != null)
                {
                    // ✅ THE FIX: Uncommented this line so the rest of the app knows who is logged in
                    App.CurrentUser = currentUser;

                    if (currentUser.Roleid == 7) // Admin
                    {
                        NavigationService.Navigate(new OperatorDashboard());
                    }
                    else // Regular User
                    {
                        NavigationService.Navigate(new HomePage());
                    }
                }
                else
                {
                    SetErrorMessage("Invalid credentials. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error:\n\n" + ex.Message, "Network Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorText.Text = "";
            }
            finally
            {
                (sender as Button).IsEnabled = true;
            }
        }

        // Helper to keep code clean
        private void SetErrorMessage(string message)
        {
            ErrorText.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF5252"));
            ErrorText.Text = message;
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                PasswordHidden.Password = PasswordVisible.Text;
                PasswordVisible.Visibility = Visibility.Collapsed;
                PasswordHidden.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
            }
            else
            {
                PasswordVisible.Text = PasswordHidden.Password;
                PasswordHidden.Visibility = Visibility.Collapsed;
                PasswordVisible.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline;
            }

            _isPasswordVisible = !_isPasswordVisible;
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            // Assuming Signup page exists
            // NavigationService.Navigate(new Signup());
            NavigationService.Navigate(new Signup());
        }

        private void skip_Click_admin(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = "admin";
            PasswordHidden.Password = "12341234";
            EmailBox.Text = "admin@gmail.com";

            // Clear visible password if toggled
            if (_isPasswordVisible) TogglePassword_Click(null, null);
        }

        private void skip_Click_user(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = "ASDASD";
            PasswordHidden.Password = "erererer";
            EmailBox.Text = "asdasdasd@gmail.com";

            // Clear visible password if toggled
            if (_isPasswordVisible) TogglePassword_Click(null, null);
        }
    }
}