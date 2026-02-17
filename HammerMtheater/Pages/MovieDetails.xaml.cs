using Model;
using System;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace HammerMtheater.Pages
{
    public partial class MovieDetails : Page
    {
        private readonly Movie _movie;

        public MovieDetails(Movie movie)
        {
            InitializeComponent();
            _movie = movie;
            DataContext = movie;
        }

        private async void WatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            // Assuming your Movie model has a TrailerUrl property
            if (!string.IsNullOrEmpty(_movie.TrailerUrl))
            {
                TrailerPlayer.Source = new Uri(_movie.TrailerUrl, UriKind.RelativeOrAbsolute);
                await DialogHost.Show(TrailerPlayer, "TrailerDialog");
            }
            else
            {
                MessageBox.Show("Trailer not available for this movie.");
            }
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SelectTheater(_movie));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}