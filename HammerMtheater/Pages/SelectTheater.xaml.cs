using Model;
using MoviesInterface;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class SelectTheater : Page
    {
        private readonly Movie _movie;
        private readonly MoviesFunctions _api;

        public SelectTheater(Movie movie)
        {
            InitializeComponent();
            _movie = movie;
            _api = new MoviesFunctions();

            LoadTheaters();
        }

        private async void LoadTheaters()
        {
            try
            {
                // ⬇️ בדיוק כמו Users / Movies
                TheaterList theaters = await _api.GetAllTheaters();
                TheaterListBox.ItemsSource = theaters;
                ;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load theaters:\n" + ex.Message);
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (TheaterListBox.SelectedItem is Theater theater)
            {
                NavigationService.Navigate(
                    new SelectHall(_movie, theater)
                );
            }
        }

    }
}
