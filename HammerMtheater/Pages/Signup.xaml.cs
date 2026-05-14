using Model;
using MoviesInterface;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HammerMtheater.Pages
{
    public partial class Signup : Page
    {
        MoviesFunctions moviesFunctions = new MoviesFunctions();
        private bool _isPasswordVisible = false;

        public Signup()
        {
            InitializeComponent();
        }

        private async void Signup_Click(object sender, RoutedEventArgs e)
        {
            SignupError.Text = "";
            string username = SignupUsername.Text.Trim();
            string email = SignupEmail.Text.Trim();
            string password = _isPasswordVisible ? SignupPasswordVisible.Text : SignupPassword.Password;

            // 1. Basic Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(ConfirmPassword.Password))
            {
                SetErrorMessage("All fields are required.");
                return;
            }

            if (password != ConfirmPassword.Password)
            {
                SetErrorMessage("Passwords do not match.");
                return;
            }

            if (password.Length < 8)
            {
                SetErrorMessage("Password must be at least 8 characters long.");
                return;
            }

            if (!IsValidEmail(email, out string emailError))
            {
                SetErrorMessage(emailError);
                return;
            }

            try
            {
                // UI Feedback: Loading
                BtnInitialize.IsEnabled = false;
                SignupError.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00C896"));
                SignupError.Text = "Creating account...";

                // 2. Check for existing users
                UserList users = await moviesFunctions.GetAllUsers();
                var existingUser = users.Find(u =>
                    u.Username?.Equals(username, StringComparison.OrdinalIgnoreCase) == true ||
                    u.Email?.Equals(email, StringComparison.OrdinalIgnoreCase) == true);

                if (existingUser != null)
                {
                    SetErrorMessage("Username or email already exists.");
                    BtnInitialize.IsEnabled = true;
                    return;
                }

                // 3. Create User
                User newUser = new User()
                {
                    Username = username,
                    Email = email,
                    Pass = password,
                    Roleid = 1 // Default Member Role
                };

                moviesFunctions.InsertUser(newUser);

                MessageBox.Show("Welcome to the theater! Your account has been initialized.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new Login());
            }
            catch (Exception ex)
            {
                SetErrorMessage("System error: " + ex.Message);
                BtnInitialize.IsEnabled = true;
            }
        }

        private void SetErrorMessage(string message)
        {
            SignupError.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5252"));
            SignupError.Text = message;
        }

        private void TogglePass_Click(object sender, RoutedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                SignupPassword.Password = SignupPasswordVisible.Text;
                SignupPasswordVisible.Visibility = Visibility.Collapsed;
                SignupPassword.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
            }
            else
            {
                SignupPasswordVisible.Text = SignupPassword.Password;
                SignupPassword.Visibility = Visibility.Collapsed;
                SignupPasswordVisible.Visibility = Visibility.Visible;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline;
            }
            _isPasswordVisible = !_isPasswordVisible;
        }

        private bool IsValidEmail(string email, out string error)
        {
            error = "";
            if (!email.Contains("@") || !email.Contains("."))
            {
                error = "Please enter a valid email address.";
                return false;
            }
            return true;
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}