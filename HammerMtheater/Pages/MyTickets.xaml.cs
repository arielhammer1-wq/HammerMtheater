using Model;
using MoviesInterface;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class MyTickets : Page
    {
        public MyTickets()
        {
            InitializeComponent();
            LoadTickets();
        }

        private async void LoadTickets()
        {
            // 1. Start Loader
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                MoviesFunctions api = new MoviesFunctions();

                // 2. Fetch data (This is the long part)
                TicketList allTickets = await api.GetAllTickets();

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
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }

        
    }
}