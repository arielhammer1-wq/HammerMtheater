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
            this.Loaded += (s, e) => RefreshData();
        }

        public async void RefreshData()
        {
            try
            {
                LoadingSpinner.Visibility = Visibility.Visible;
                MainDataGrid.ItemsSource = null;

                MoviesFunctions api = new MoviesFunctions();
                object result = null;

                switch (_currentMode)
                {
                    case "Movies": result = await api.GetAllMovies(); break;
                    case "Theaters": result = await api.GetAllTheaters(); break;
                    case "Users": result = await api.GetAllUsers(); break;
                    case "Tickets": result = await api.GetAllTickets(); break;
                    default: MessageBox.Show("Data currently unavailable"); break;
                }

                MainDataGrid.ItemsSource = (System.Collections.IEnumerable)result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                LoadingSpinner.Visibility = Visibility.Collapsed;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => RefreshData();

        // Navigation Handlers
        private void NavUsers_Click(object sender, RoutedEventArgs e) { _currentMode = "Users"; ViewTitle.Text = "User Management"; SetButtonActive(BtnUsers); RefreshData(); }
        private void NavTickets_Click(object sender, RoutedEventArgs e) { _currentMode = "Tickets"; ViewTitle.Text = "Ticket Logs"; SetButtonActive(BtnTickets); RefreshData(); }
        private void NavMovies_Click(object sender, RoutedEventArgs e) { _currentMode = "Movies"; ViewTitle.Text = "Movie Catalog"; SetButtonActive(BtnMovies); RefreshData(); }
        private void NavTheaters_Click(object sender, RoutedEventArgs e) { _currentMode = "Theaters"; ViewTitle.Text = "Theater Management"; SetButtonActive(BtnTheaters); RefreshData(); }

        // THE FIX: Changed 'Button' to 'RadioButton'
        private void SetButtonActive(RadioButton activeBtn)
        {
            // Reset all to white
            BtnMovies.Foreground = BtnTheaters.Foreground = BtnUsers.Foreground = BtnTickets.Foreground = Brushes.White;

            // Set active to Emerald Green
            activeBtn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00C896"));
            activeBtn.IsChecked = true;
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
            if (MessageBox.Show("Delete permanently?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Add your delete logic here via API
                RefreshData();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Optional: Implement filtering logic here
        }
    }
}