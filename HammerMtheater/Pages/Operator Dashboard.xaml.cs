using MoviesInterface;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HammerMtheater.Pages
{
    public partial class OperatorDashboard : Page
    {
        private string _currentMode = "Theaters";

        public OperatorDashboard()
        {
            InitializeComponent();
            RefreshData();
        }

        private async void RefreshData()
        {
            try
            {
                MoviesFunctions api = new MoviesFunctions();
                if (_currentMode == "Theaters")
                {
                    MainDataGrid.ItemsSource = await api.GetAllTheaters();
                }
                else
                {
                    MainDataGrid.ItemsSource = await api.GetAllMovies();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void InsertItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ItemEditorPage(_currentMode));
        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext;
            if (selectedItem != null)
            {
                NavigationService.Navigate(new ItemEditorPage(_currentMode, selectedItem));
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext;
            if (selectedItem == null) return;

            var result = MessageBox.Show("Delete this record permanently?", "Confirm Delete",
                                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    MoviesFunctions api = new MoviesFunctions();
                    // Implement your actual Delete logic here using api
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete failed: " + ex.Message);
                }
            }
        }

        private void NavMovies_Click(object sender, RoutedEventArgs e)
        {
            _currentMode = "Movies";
            ViewTitle.Text = "Movie Management";
            BtnMovies.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E50914"));
            BtnTheaters.Foreground = Brushes.White;
            RefreshData();
        }

        private void NavTheaters_Click(object sender, RoutedEventArgs e)
        {
            _currentMode = "Theaters";
            ViewTitle.Text = "Theater Management";
            BtnTheaters.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E50914"));
            BtnMovies.Foreground = Brushes.White;
            RefreshData();
        }
    }
}