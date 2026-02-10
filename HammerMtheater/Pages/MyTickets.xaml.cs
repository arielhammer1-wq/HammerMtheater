using Model;
using MoviesInterface;
using System.Linq;
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
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }

        private async void LoadTickets()
        {
            MoviesFunctions api = new MoviesFunctions();
            TicketList allTickets = await api.GetAllTickets();

            var myTickets = allTickets
                .Where(t => t.User.Id == App.CurrentUser.Id)
                .ToList();

            TicketsList.ItemsSource = myTickets;
        }
    }
}
