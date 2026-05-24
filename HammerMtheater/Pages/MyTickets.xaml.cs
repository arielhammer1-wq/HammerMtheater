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
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                MoviesFunctions api = new MoviesFunctions();

                TicketList myTickets = await api.GetTicketsByUserId(App.CurrentUser.Id);

                TicketsList.ItemsSource = myTickets;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load tickets: " + ex.Message);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }

        
    }
}