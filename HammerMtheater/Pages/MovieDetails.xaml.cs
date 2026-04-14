using Model;
using System;
using System.Linq;
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

            try
            {
                // 1. Prepare the WebView2 BEFORE showing the dialog
                if (TrailerBrowser.CoreWebView2 == null)
                {
                    await TrailerBrowser.EnsureCoreWebView2Async();
                }

                // 2. Convert standard URL to Embed URL
                string videoId = GetYouTubeId(_movie.TrailerUrl);
                if (string.IsNullOrEmpty(videoId))
                {
                    MessageBox.Show("Could not parse YouTube ID.");
                    return;
                }

                // Modestbranding and rel=0 make it look cleaner in your UI
                string embedUrl = $"https://www.youtube.com/embed/{videoId}?autoplay=1&rel=0&modestbranding=1";
                TrailerBrowser.CoreWebView2.Navigate(embedUrl);

                // 3. SHOW THE DIALOG
                // Passing TrailerContainer tells the DialogHost to use that specific Grid
                await DialogHost.Show(TrailerContainer, "TrailerDialog");
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 error: " + ex.Message);
            }
        }

        // Robust YouTube ID Parser
        private string GetYouTubeId(string url)
        {
            try
            {
                var uri = new Uri(url);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                if (query.AllKeys.Contains("v")) return query["v"];

                return uri.Segments.Last();
            }
            catch
            {
                return string.Empty;
            }
        }

        private void DialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            // Stop the video audio immediately when the user clicks 'Close'
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