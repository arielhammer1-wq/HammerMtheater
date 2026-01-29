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

        private const int PRICE_PER_TICKET = 40;

        public TicketSummary(
            Movie movie,
            Theater theater,
            MovieHall hall,
            List<int> seats)
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
            PriceText.Text = $"Total: ₪{_seats.Count * PRICE_PER_TICKET}";
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            MoviesFunctions api = new MoviesFunctions();

            foreach (int seat in _seats)
            {
                Ticket t = new Ticket
                {
                    TicketPrice = PRICE_PER_TICKET,
                    SeatNumber = seat,
                    User = App.CurrentUser!,
                    Screening = null // זמני
                };

                await api.InsertTicket(t);
            }

            MessageBox.Show("Tickets purchased successfully!");
            NavigationService.Navigate(new HomePage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
