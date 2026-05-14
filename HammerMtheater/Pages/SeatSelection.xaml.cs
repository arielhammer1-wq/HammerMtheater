using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HammerMtheater.UserControls;
using Model;
using MoviesInterface; // Make sure this is here to access your API functions

namespace HammerMtheater.Pages
{
    public partial class SeatSelection : Page
    {
        private readonly Movie _movie;
        private readonly Theater _theater;
        private readonly MovieHall _hall;

        // Connect to your API functions
        private readonly MoviesFunctions _api = new MoviesFunctions();

        private readonly List<SeatControl> _seatControls = new();

        public SeatSelection(Movie movie, Theater theater, MovieHall hall)
        {
            InitializeComponent();
            _movie = movie;
            _theater = theater;
            _hall = hall;

            // Call the async method to fetch data and draw seats
            LoadSeats();
        }

        private async void LoadSeats()
        {
            // 1. Fetch the list of already taken seat numbers for this specific movie and hall
            // Use the GetTakenSeatsByScreening method we added to your MoviesFunctions
            List<int> takenSeats = await _api.GetTakenSeatsByScreening(_movie.Id, _hall.Id);

            LeftSeats.Children.Clear();
            CenterSeats.Children.Clear();
            RightSeats.Children.Clear();
            _seatControls.Clear();

            int total = _hall.AmountOfSeats;

            for (int i = 1; i <= total; i++)
            {
                // 2. Check if the current seat number exists in the "taken" list from the DB
                bool isAvailable = !takenSeats.Contains(i);

                var seat = new SeatControl(i, isAvailable);

                // Only allow clicking if the seat is actually available
                if (isAvailable)
                {
                    seat.SeatSelected += Seat_Selected;
                }

                _seatControls.Add(seat);

                // UI Layout Logic
                if (i % 3 == 0)
                    LeftSeats.Children.Add(seat);
                else if (i % 3 == 1)
                    CenterSeats.Children.Add(seat);
                else
                    RightSeats.Children.Add(seat);
            }
        }

        private void Seat_Selected(object sender, RoutedEventArgs e)
        {
            var seat = sender as SeatControl;
            seat?.ToggleSelected();
        }

        private async void Continue_Click(object sender, RoutedEventArgs e)
        {
            var selectedSeats = _seatControls
                .Where(s => s.IsSelected)
                .Select(s => s.SeatNumber)
                .ToList();

            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Select at least one seat");
                return;
            }

            // --- SAFETY CHECK 1: Ensure your objects aren't null before saving ---
            if (_movie == null || _hall == null || _api == null)
            {
                MessageBox.Show("Critical Error: Movie data is missing!");
                return;
            }

            foreach (int seatNum in selectedSeats)
            {
                TakenSeat ts = new TakenSeat
                {
                    MovieId = _movie.Id,
                    HallId = _hall.Id,
                    SeatNumber = seatNum
                };
                await _api.InsertTakenSeat(ts);
            }

            // --- SAFETY CHECK 2: Ensure Navigation is available ---
            if (this.NavigationService != null)
            {
                NavigationService.Navigate(new TicketSummary(_movie, _theater, _hall, selectedSeats));
            }
            else
            {
                MessageBox.Show("Navigation Error: Frame not found.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}