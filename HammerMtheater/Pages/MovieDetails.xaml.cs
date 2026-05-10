using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Core;
using Model;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Web; // Necessary for HttpUtility

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

            LoadLiveScore();
        }

        private async void LoadLiveScore()
        {
            if (LiveScoreText == null) return;
            LiveScoreText.Text = "Fetching Score...";

            string score = await GetLiveRatingAsync(_movie.MovieName);

            LiveScoreText.Text = (score == "N/A" || score == "Score Unavailable")
                ? "No Rating"
                : $"TMDB: {score}";
        }

        private async Task<string> GetLiveRatingAsync(string movieName)
        {
            try
            {
                string apiKey = "a6103d37a74c0928d5a5156e7b12c495";
                string url = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={Uri.EscapeDataString(movieName)}";

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var data = JObject.Parse(response);

                    // Added safer null-checking for API results
                    var result = data["results"]?.FirstOrDefault();
                    var rating = result?["vote_average"]?.ToString();

                    return !string.IsNullOrEmpty(rating) ? $"{rating}/10" : "N/A";
                }
            }
            catch { return "Score Unavailable"; }
        }

        private async void WatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            string videoId = GetYouTubeId(_movie.TrailerUrl);
            if (string.IsNullOrEmpty(videoId))
            {
                MainSnackbar.MessageQueue?.Enqueue("Trailer link is invalid.");
                return;
            }

            string embedUrl = $"https://www.youtube.com/embed/{videoId}?autoplay=1";

            var trailerWindow = new Window
            {
                Title = $"{_movie.MovieName} — Trailer",
                Width = 1100,
                Height = 650,
                Background = System.Windows.Media.Brushes.Black,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this),
                WindowState = WindowState.Maximized
            };

            var browser = new Microsoft.Web.WebView2.Wpf.WebView2();
            trailerWindow.Content = browser;
            trailerWindow.Show();

            await browser.EnsureCoreWebView2Async();
            browser.CoreWebView2.Navigate(embedUrl);

            trailerWindow.Closed += (s, args) => browser.Dispose();
        }

        private string GetYouTubeId(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            try
            {
                url = url.Trim().TrimEnd('\\', '/');
                var uri = new Uri(url);

                if (uri.Host.Contains("youtu.be"))
                    return uri.Segments.Last().Trim('/');

                var query = HttpUtility.ParseQueryString(uri.Query);
                if (query.AllKeys.Contains("v"))
                    return query["v"];

                if (uri.AbsolutePath.Contains("embed"))
                    return uri.Segments.Last().Trim('/');

                return string.Empty;
            }
            catch { return string.Empty; }
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new SelectTheater(_movie));

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

 
    }
}