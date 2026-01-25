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

        private SeatControl? _selectedSeat;

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

            int totalSeats = _hall.AmountOfSeats;

            for (int i = 1; i <= totalSeats; i++)
            {
                // כרגע כולם פנויים – בהמשך תחבר ל-Tickets
                bool isAvailable = true;

                var seat = new SeatControl(i, isAvailable);
                seat.SeatSelected += Seat_Selected;

                SeatsGrid.Children.Add(seat);
            }
        }

        private void Seat_Selected(object sender, RoutedEventArgs e)
        {
            if (_selectedSeat != null)
                _selectedSeat.SetSelected(false);

            _selectedSeat = sender as SeatControl;
            _selectedSeat?.SetSelected(true);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            //if (_selectedSeat == null)
            //{
            //    MessageBox.Show("Please select a seat");
            //    return;
            //}

            //NavigationService.Navigate(
            //    new TicketSummary(
            //        _movie,
            //        _theater,
            //        _hall,
            //        _selectedSeat.SeatNumber
            //    )
            //);
        }
    }
}
