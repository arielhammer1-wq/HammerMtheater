using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Model;
using MoviesInterface;

namespace HammerMtheater.Pages
{
    public partial class TicketSummary : Page
    {
        private readonly Movie _movie;
        private readonly Theater _theater;
        private readonly MovieHall _hall;
        private readonly List<int> _seats;

        private const int PRICE = 40;

        public TicketSummary(Movie movie, Theater theater, MovieHall hall, List<int> seats)
        {
            InitializeComponent();

            _movie = movie;
            _theater = theater;
            _hall = hall;
            _seats = seats;

            LoadSummary();
        }

        private void LoadSummary()
        {
            MovieText.Text = $"Movie: {_movie.MovieName}";
            TheaterText.Text = $"Theater: {_theater.NameOfTheater}";
            HallText.Text = $"Hall: {_hall.HallName}";
            SeatsText.Text = $"Seats: {string.Join(", ", _seats)}";
            PriceText.Text = $"Total: ₪{_seats.Count * PRICE}";
        }

       private async void Confirm_Click(object sender, RoutedEventArgs e)
{
    MoviesFunctions api = new MoviesFunctions();
    User currentUser = App.CurrentUser;

    foreach (int seat in _seats)
    {
        Ticket t = new Ticket
        {
            SeatNumber = seat,
            TicketPrice =t.TicketPrice,
            User = currentUser,
            Movie = _movie,
            Theater = _theater,
            Hall = _hall
        };

        await api.InsertTicket(t);
    }

    MessageBox.Show("Tickets purchased!");

    NavigationService.Navigate(new MyTickets());
}

        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
