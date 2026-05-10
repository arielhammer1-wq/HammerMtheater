using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HammerMtheater.UserControls;
using Model;
using MoviesInterface;
namespace HammerMtheater.Pages
{
    public partial class SeatSelection : Page
    {
        private readonly Movie _movie;
        private readonly Theater _theater;
        private readonly MovieHall _hall;
        private readonly List<SeatControl> _seatControls = new();

        public SeatSelection(Movie movie, Theater theater, MovieHall hall)
        {
            InitializeComponent();
            _movie = movie;
            _theater = theater;
            _hall = hall;

            BuildSeats();
        }

        private void BuildSeats()
        {
            LeftSeats.Children.Clear();
            CenterSeats.Children.Clear();
            RightSeats.Children.Clear();
            _seatControls.Clear();

            // 1. Fetch taken seats from the DB you updated
            TicketDB db = new TicketDB();
            List<int> takenSeats = db.GetTakenSeats(_movie.Id, _hall.Id);

            int total = _hall.AmountOfSeats;

            for (int i = 1; i <= total; i++)
            {
                // 2. Determine if seat is available (true if NOT in the taken list)
                bool isAvailable = !takenSeats.Contains(i);

                var seat = new SeatControl(i, isAvailable);

                // Only allow clicking if the seat is free
                if (isAvailable)
                {
                    seat.SeatSelected += Seat_Selected;
                }
                else
                {
                    seat.IsEnabled = false; // Prevents clicking entirely
                }

                _seatControls.Add(seat);

                // Seat Layout Logic
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

        private void Continue_Click(object sender, RoutedEventArgs e)
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

            NavigationService.Navigate(
                new TicketSummary(_movie, _theater, _hall, selectedSeats)
            );
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}