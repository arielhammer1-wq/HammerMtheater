using Model;
using MoviesInterface;
using System;
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
            // Force a refresh every single time this page becomes visible
            this.Loaded += (s, e) => RefreshData();
        }

        public async void RefreshData()
        {
            try
            {
                MoviesFunctions api = new MoviesFunctions();
                // Clear old data first to force a UI update
                MainDataGrid.ItemsSource = null;

                if (_currentMode == "Movies")
                    MainDataGrid.ItemsSource = (await api.GetAllMovies());
                else if (_currentMode == "Theaters")
                    MainDataGrid.ItemsSource = (await api.GetAllTheaters());
            }
            catch (Exception ex) { /* Log error */ }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData(); // Manual refresh
        }

        private void NavUsers_Click(object sender, RoutedEventArgs e) { _currentMode = "Users"; ViewTitle.Text = "Users"; SetButtonActive(BtnUsers); RefreshData(); }
        private void NavTickets_Click(object sender, RoutedEventArgs e) { _currentMode = "Tickets"; ViewTitle.Text = "Tickets"; SetButtonActive(BtnTickets); RefreshData(); }
        private void NavMovies_Click(object sender, RoutedEventArgs e) { _currentMode = "Movies"; ViewTitle.Text = "Movies"; SetButtonActive(BtnMovies); RefreshData(); }
        private void NavTheaters_Click(object sender, RoutedEventArgs e) { _currentMode = "Theaters"; ViewTitle.Text = "Theaters"; SetButtonActive(BtnTheaters); RefreshData(); }

        private void SetButtonActive(Button activeBtn)
        {
            BtnMovies.Foreground = BtnTheaters.Foreground = BtnUsers.Foreground = BtnTickets.Foreground = Brushes.White;
            activeBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E50914"));
        }

        private void InsertItem_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ItemEditorPage(_currentMode));

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext;
            if (selectedItem != null) NavigationService.Navigate(new ItemEditorPage(_currentMode, selectedItem));
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext;
            if (selectedItem == null) return;
            if (MessageBox.Show("Delete permanently?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // api.Delete logic here
                RefreshData();
            }
        }

    }
}