using Model;
using MoviesInterface;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class UserProfile : Page
    {
        private readonly MoviesFunctions _api = new MoviesFunctions();

        public UserProfile()
        {
            InitializeComponent();
            Loaded += UserProfile_Loaded;
        }

        private async void UserProfile_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProfile();
        }

        private async Task LoadProfile()
        {
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                if (App.CurrentUser == null)
                {
                    MessageBox.Show("Error: no user logged in.", "Authentication Error");
                    NavigationService.Navigate(new Login());
                    return;
                }

                LoadUserData();

                TicketList myTickets = await _api.GetTicketsByUserId(App.CurrentUser.Id);

                LoadStats(myTickets);
                LoadRecentTickets(myTickets);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load profile: " + ex.Message);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadUserData()
        {
            UserNameText.Text = App.CurrentUser.Username;
            UserEmailText.Text = App.CurrentUser.Email;
        }

        private void LoadStats(TicketList myTickets)
        {
            if (myTickets == null || myTickets.Count == 0)
            {
                TotalTicketsText.Text = "0";
                TotalSpentText.Text = "₪0";
                FavoriteTheaterText.Text = "None";
                LastMovieText.Text = "None";
                return;
            }

            int totalTickets = myTickets.Count;
            int totalSpent = myTickets.Sum(t => t.TicketPrice);

            string favoriteTheater = myTickets
                .Where(t => t.Theater != null)
                .GroupBy(t => t.Theater.NameOfTheater)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "None";

            string lastMovie = myTickets
                .OrderByDescending(t => t.Id)
                .FirstOrDefault()?.Movie?.MovieName ?? "None";

            TotalTicketsText.Text = totalTickets.ToString();
            TotalSpentText.Text = $"₪{totalSpent}";
            FavoriteTheaterText.Text = favoriteTheater;
            LastMovieText.Text = lastMovie;
        }

        private void LoadRecentTickets(TicketList myTickets)
        {
            if (myTickets == null)
            {
                RecentTicketsList.ItemsSource = null;
                return;
            }

            var recentTickets = myTickets
                .OrderByDescending(t => t.Id)
                .Take(3)
                .ToList();

            RecentTicketsList.ItemsSource = recentTickets;
        }

        private void ViewAllTickets_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MyTickets());
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditProfilePage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new HomePage());
        }
    }
}