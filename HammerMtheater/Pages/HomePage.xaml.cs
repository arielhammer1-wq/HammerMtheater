using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HammerMtheater.Pages
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            // Get the full path to the video file in your debug folder
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "trailer.mp4");

            // Set the source and manually play
            BackgroundVideo.Source = new Uri(path, UriKind.Absolute);
            BackgroundVideo.Play();
        }
     
        

        // THIS IS THE MISSING PIECE:
        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Reset the video to the beginning
            BackgroundVideo.Position = TimeSpan.Zero;
            BackgroundVideo.Play();
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            // Replace 'MoviesPage' with your actual Movies page class name
            NavigationService.Navigate(new MoviePage());
        }

        private void My_Tickets_Click(object sender, RoutedEventArgs e)
        {
            // Replace 'MyTickets' with your actual Tickets page class name
            NavigationService.Navigate(new MyTickets());
        }
        private void Account_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserProfile());
        }

        private async void Support(object sender, RoutedEventArgs e)
        {
            var content = new StackPanel { Margin = new Thickness(20) };

            content.Children.Add(new TextBlock
            {
                Text = "SUPPORT",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 150)) // Using your #00C896 theme color
            });

            content.Children.Add(new TextBlock
            {
                Text = "For help, contact us at support@hammer.cinema\nHours: Sun–Thu 9am–10pm",
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.White
            });

            var closeButton = new Button
            {
                Content = "CLOSE",
                IsDefault = true,
                Margin = new Thickness(0, 20, 0, 0),
                Style = (Style)FindResource("MaterialDesignFlatButton")
            };

            // This is the correct C# syntax to close the dialog when clicked
            closeButton.Click += (s, args) => DialogHost.Close("RootDialog");

            content.Children.Add(closeButton);

            // Use the class name 'DialogHost' directly
            await DialogHost.Show(content, "RootDialog");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}