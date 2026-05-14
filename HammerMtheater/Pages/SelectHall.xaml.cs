using Model;
using MoviesInterface;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace HammerMtheater.Pages
{
    public partial class SelectHall : Page
    {
        private readonly Movie _movie;
        private readonly Theater _theater;
        private readonly MoviesFunctions _api = new MoviesFunctions();

        public SelectHall(Movie movie, Theater theater)
        {
            InitializeComponent();
            _movie = movie;
            _theater = theater;
            LoadHalls();
        }

        private async void LoadHalls()
        {
            MovieHallList halls = await _api.GetAllMovieHalls();

            var filtered = halls
                .Where(h => h.Theater.Id == _theater.Id)
                .ToList();

            HallListBox.ItemsSource = filtered;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (HallListBox.SelectedItem is MovieHall hall)
            {
                NavigationService.Navigate(
                    new SeatSelection(_movie, _theater, hall)
                );
            }
            else
            {
                MessageBox.Show("Please select a hall");
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
