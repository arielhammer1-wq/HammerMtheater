using Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;

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

        // ─────────────────────────────────────────
        // TRAILER
        // ─────────────────────────────────────────

        private async void WatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            string videoId = GetYouTubeId(_movie.TrailerUrl);
            string testUrl = $"https://www.youtube.com/watch?v={videoId}";

            var trailerWindow = new Window
            {
                Title = _movie.MovieName + " — Trailer",
                Width = 1100,
                Height = 650,
                Background = System.Windows.Media.Brushes.Black,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.ToolWindow,
                Owner = Window.GetWindow(this)
            };

            var browser = new Microsoft.Web.WebView2.Wpf.WebView2();
            trailerWindow.Content = browser;
            trailerWindow.Show();
            trailerWindow.WindowState = WindowState.Maximized;
            trailerWindow.Activate();

            await browser.EnsureCoreWebView2Async();

            browser.CoreWebView2.Settings.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0";

            browser.CoreWebView2.NavigationCompleted += async (s, args) =>
            {
                await browser.CoreWebView2.ExecuteScriptAsync(
                    "document.querySelector('video')?.requestFullscreen();"
                );
            };

            browser.CoreWebView2.Navigate(testUrl);

            trailerWindow.Closed += (s, args) =>
                browser.CoreWebView2?.Navigate("about:blank");
        }

        // ─────────────────────────────────────────
        // YOUTUBE ID PARSER
        // ─────────────────────────────────────────

        private string GetYouTubeId(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url)) return string.Empty;
                url = url.Trim().TrimEnd('\\', '/');
                var uri = new Uri(url);

                if (uri.Host.Contains("youtu.be"))
                    return uri.Segments.Last().Trim('/');

                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                if (query.AllKeys.Contains("v"))
                    return query["v"];

                if (uri.AbsolutePath.Contains("embed"))
                    return uri.Segments.Last().Trim('/');

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        // ─────────────────────────────────────────
        // NAVIGATION
        // ─────────────────────────────────────────

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SelectTheater(_movie));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        // ─────────────────────────────────────────
        // SHARE (UPDATED WITH TRAILER & GENRE)
        // ─────────────────────────────────────────

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            var shareWindow = new Window
            {
                Title = "Share Movie",
                Width = 460,
                Height = 280,
                Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1E2228")),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.ToolWindow,
                Owner = Window.GetWindow(this),
                Topmost = true,
                ResizeMode = ResizeMode.NoResize
            };

            var emailBox = new TextBox
            {
                Margin = new Thickness(30, 20, 30, 10),
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White,
                Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2A2F38")),
                BorderBrush = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#252A31")),
                Padding = new Thickness(10),
                Text = ""
            };

            var sendBtn = new Button
            {
                Content = "SEND",
                Width = 120,
                Height = 44,
                Margin = new Thickness(0, 0, 10, 0),
                Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00C896")),
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0B0E11")),
                FontWeight = FontWeights.Black
            };

            var cancelBtn = new Button
            {
                Content = "CANCEL",
                Width = 120,
                Height = 44,
                Background = System.Windows.Media.Brushes.Transparent,
                Foreground = System.Windows.Media.Brushes.Gray,
                FontWeight = FontWeights.Bold
            };

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(30, 10, 30, 0)
            };
            btnPanel.Children.Add(cancelBtn);
            btnPanel.Children.Add(sendBtn);

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "Share this movie",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(30, 30, 30, 10)
            });
            panel.Children.Add(new TextBlock
            {
                Text = "Enter recipient's email address:",
                FontSize = 13,
                Foreground = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(30, 0, 30, 8)
            });
            panel.Children.Add(emailBox);
            panel.Children.Add(btnPanel);

            shareWindow.Content = panel;

            cancelBtn.Click += (s, args) => shareWindow.Close();

            // Direct sending logic with Trailer and Genre integration
            sendBtn.Click += async (s, args) =>
            {
                string recipientEmail = emailBox.Text.Trim();

                // Regex Validation
                if (string.IsNullOrWhiteSpace(recipientEmail) ||
                    !System.Text.RegularExpressions.Regex.IsMatch(recipientEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MainSnackbar.MessageQueue?.Enqueue("Please enter a valid email address.");
                    return;
                }

                sendBtn.IsEnabled = false;
                sendBtn.Content = "SENDING...";

                try
                {
                    var emailService = new HammerMtheater.Services.EmailService();

                    // Sending Name, Trailer, and Genre
                    bool isSent = await emailService.SendMovieShareEmail(
                        recipientEmail,
                        _movie.MovieName,
                        _movie.TrailerUrl,
                        _movie.Genre?.GenreName ?? "Cinema"
                    );

                    if (isSent)
                    {
                        MainSnackbar.MessageQueue?.Enqueue($"✨ Shared {_movie.MovieName} with {recipientEmail}!");
                        shareWindow.Close();
                    }
                    else
                    {
                        MainSnackbar.MessageQueue?.Enqueue("❌ Failed to send. Check internet or App Password.");
                        sendBtn.IsEnabled = true;
                        sendBtn.Content = "SEND";
                    }
                }
                catch (Exception ex)
                {
                    MainSnackbar.MessageQueue?.Enqueue("Error: " + ex.Message);
                    sendBtn.IsEnabled = true;
                    sendBtn.Content = "SEND";
                }
            };

            shareWindow.ShowDialog();
        }

        // ─────────────────────────────────────────
        // CAST
        // ─────────────────────────────────────────

        private void ViewCast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new CastPage(_movie.Id));
            }
            catch
            {
                MainSnackbar.MessageQueue?.Enqueue("Cast information not available.");
            }
        }
    }
}