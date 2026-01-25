using Model;
using MoviesInterface;
using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class Signup : Page
    {
        MoviesFunctions moviesFunctions = new MoviesFunctions();

        public Signup()
        {
            InitializeComponent();
        }

        private async void  Signup_Click(object sender, RoutedEventArgs e)
        {
            string username = SignupUsername.Text;
            string password = SignupPassword.Password;
            string email = SignupEmail.Text;

            if (SignupUsername.Text == "" ||
                SignupPassword.Password == "" ||
                ConfirmPassword.Password == ""||
                SignupEmail.Text=="")
            {
                SignupError.Text = "All fields are required";
                return;
            }

            

            if (!IsValidEmail(email, out string emailError))
            {
                SignupError.Text = emailError;
                return;
            }
            UserList users = await moviesFunctions.GetAllUsers();

            var existingUser = users.Find(u =>
                u.Username == SignupUsername.Text || u.Email == SignupEmail.Text);

            if (existingUser != null)
            {
                SignupError.Text = "Username or email already exists.";
                return;
            }
            if (SignupPassword.Password != ConfirmPassword.Password)
            {
                SignupError.Text = "Passwords do not match";
                return;
            }
            if (password.Length < 8)
            {
                SignupError.Text = "Password must be at least 8 characters long";
                return;
            }

            User user = new User() { Email=email, Pass=password ,Username=username};
            moviesFunctions.InsertUser(user);

            MessageBox.Show("Account created!");



            NavigationService.Navigate(new Login());
        }
        private bool IsValidEmail(string email, out string error)
        {
            error = "";

            if (!email.Contains("@"))
            {
                error = "Email must contain '@'.";
                return false;
            }

            var parts = email.Split('@');
            if (parts.Length != 2)
            {
                error = "Email format is invalid.";
                return false;
            }

            string local = parts[0];
            string domain = parts[1];


            // Only allow letters, numbers, dots, underscores, dashes in local part
            if (!System.Text.RegularExpressions.Regex.IsMatch(local, @"^[A-Za-z0-9._-]+$"))
            {
                error = "Email local part contains invalid characters.";
                return false;
            }

            // בודק את סוף האימייל
            if (!System.Text.RegularExpressions.Regex.IsMatch(domain, @"^([-0-9A-Z]+\.)+([0-9A-Z]{2,4})$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                error = "Email domain is invalid.";
                return false;
            }
            return true; // Email is valid
        }
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
