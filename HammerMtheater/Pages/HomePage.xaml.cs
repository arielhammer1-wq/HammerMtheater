using System;
using System.Windows;
using System.Windows.Controls;

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
    }
}