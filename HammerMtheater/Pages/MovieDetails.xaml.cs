using Model;
using System;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Core;

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
            if (string.IsNullOrWhiteSpace(_movie?.TrailerUrl))
            {
                MessageBox.Show("Trailer not available.");
                return;
            }

            // Open dialog FIRST
            await DialogHost.Show(new object(), "TrailerDialog");

            try
            {
                // Initialize AFTER dialog is visible
                if (TrailerBrowser.CoreWebView2 == null)
                {
                    await TrailerBrowser.EnsureCoreWebView2Async();
                }

                string videoId = GetYouTubeId(_movie.TrailerUrl);

                string embedUrl =
                    $"https://www.youtube.com/embed/{videoId}?autoplay=1&controls=1";

                TrailerBrowser.CoreWebView2.Navigate(embedUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 error:\n" + ex.Message);
            }
        }

        private string GetYouTubeId(string url)
        {
            if (url.Contains("watch?v="))
                return url.Split("watch?v=")[1].Split('&')[0];

            if (url.Contains("youtu.be/"))
                return url.Split("youtu.be/")[1];

            return url;
        }

        private void DialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (TrailerBrowser?.CoreWebView2 != null)
            {
                TrailerBrowser.CoreWebView2.Navigate("about:blank");
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