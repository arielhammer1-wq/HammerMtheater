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

        // מחיר לכרטיס אחד
        private int _currentPrice;

        public TicketSummary(Movie movie, Theater theater, MovieHall hall, List<int> seats)
        {
            InitializeComponent();

            _movie = movie;
            _theater = theater;
            _hall = hall;
            _seats = seats;

            // ⬅️ חישוב מחיר דינמי כאן
            _currentPrice = CalculateTicketPrice();

            LoadSummary();
        }

        private void LoadSummary()
        {
            MovieText.Text = $"Movie: {_movie.MovieName}";
            TheaterText.Text = $"Theater: {_theater.NameOfTheater}";
            HallText.Text = $"Hall: {_hall.HallName}";
            SeatsText.Text = $"Seats: {string.Join(", ", _seats)}";

            int totalPrice = _seats.Count * _currentPrice;
            PriceText.Text = $"Total: ₪{totalPrice}";
        }

        /// <summary>
        /// כאן כל הלוגיקה של Dynamic Pricing
        /// </summary>
        private int CalculateTicketPrice()
        {
            int basePrice = 40;

            // דוגמה: אולם גדול = יותר יקר
            if (_hall.AmountOfSeats > 150)
                basePrice += 10;

            // דוגמה: סופ״ש
            if (System.DateTime.Now.DayOfWeek == System.DayOfWeek.Friday ||
                System.DateTime.Now.DayOfWeek == System.DayOfWeek.Saturday)
                basePrice += 5;

            return basePrice;
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            MoviesFunctions api = new MoviesFunctions();
            User currentUser = App.CurrentUser;

            if (currentUser == null)
            {
                MessageBox.Show("User not logged in");
                return;
            }

            foreach (int seat in _seats)
            {
                Ticket t = new Ticket
                {
                    SeatNumber = seat,
                    TicketPrice = _currentPrice,
                    User = currentUser,
                    Movie = _movie,
                    Theater = _theater,
                    Hall = _hall
                };

                await api.InsertTicket(t);
            }

            MessageBox.Show("Tickets purchased successfully!");

            // מעבר ל־MyTickets
            NavigationService.Navigate(new MyTickets());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
