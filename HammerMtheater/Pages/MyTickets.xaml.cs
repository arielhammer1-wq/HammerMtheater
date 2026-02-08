using System.Linq;
using System.Windows.Controls;
using Model;
using MoviesInterface;

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
            MoviesFunctions api = new MoviesFunctions();
            TicketList allTickets = await api.GetAllTickets();

            var myTickets = allTickets
                .Where(t => t.User.Id == App.CurrentUser.Id)
                .ToList();

            TicketsList.ItemsSource = myTickets;
        }
    }
}
