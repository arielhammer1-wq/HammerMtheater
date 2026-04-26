using Model;
using MoviesInterface;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HammerMtheater.Pages
{
    public partial class UserProfile : Page
    {
        private readonly MoviesFunctions _api = new MoviesFunctions();

        public UserProfile()
        {
            InitializeComponent();
            this.Loaded += UserProfile_Loaded;
        }

        private async void UserProfile_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadUserData();
             LoadTickets();
        }

        private async Task LoadUserData()
        {
            try
            {
                if (App.CurrentUser != null)
                {
                    UserNameText.Text = App.CurrentUser.Username;
                    UserEmailText.Text = App.CurrentUser.Email;
                }
                else
                {
                    MessageBox.Show("Error: Session expired or no user logged in.", "Authentication Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading profile: " + ex.Message);
            }
        }

        private async void LoadTickets()
        {
            // 1. Start Loader (Ensure you have a UI element named LoadingOverlay in your XAML)
            if (LoadingOverlay != null) LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                // 2. Fetch data
                TicketList allTickets = await _api.GetAllTickets();

                // 3. Filter data
                var myTickets = allTickets
                    .Where(t => t.User.Id == App.CurrentUser.Id)
                    .ToList();

                // 4. Bind to UI
                TicketsList.ItemsSource = myTickets;

                // Small delay so the user actually sees the beautiful loader transition
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load tickets: " + ex.Message);
            }
            finally
            {
                // 5. Hide Loader
                if (LoadingOverlay != null) LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Profile editing coming soon!", "Information");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}