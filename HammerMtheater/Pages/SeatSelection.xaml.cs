using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HammerMtheater.UserControls;
using Model;

namespace HammerMtheater.Pages
{
    public partial class SeatSelection : Page
    {
        private readonly Movie _movie;
        private readonly Theater _theater;
        private readonly MovieHall _hall;

        private readonly List<SeatControl> _seats = new();

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
            SeatsGrid.Children.Clear();
            _seats.Clear();

            for (int i = 1; i <= _hall.AmountOfSeats; i++)
            {
                bool isAvailable = true; // בהמשך תבדוק DB

                var seat = new SeatControl(i, isAvailable);
                _seats.Add(seat);
                SeatsGrid.Children.Add(seat);
            }
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            var selectedSeats = _seats
                .Where(s => s.IsSelected)
                .Select(s => s.SeatNumber)
                .ToList();

            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Please select at least one seat");
                return;
            }

            NavigationService.Navigate(
                new TicketSummary(
                    _movie,
                    _theater,
                    _hall,
                    selectedSeats
                )
            );
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
