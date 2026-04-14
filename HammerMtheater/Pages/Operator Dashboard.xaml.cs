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
                // 1. Show Spinner and clear grid
                LoadingSpinner.Visibility = Visibility.Visible;
                MainDataGrid.ItemsSource = null;

                MoviesFunctions api = new MoviesFunctions();

                // 2. Fetch Data based on mode
                object result = null;
                switch (_currentMode)
                {
                    case "Movies":
                        result = await api.GetAllMovies();
                        break;

                    case "Theaters":
                        result = await api.GetAllTheaters();
                        break;

                    case "Users":
                        result = await api.GetAllUsers();
                        break;

                    case "Tickets":
                        result = await api.GetAllTickets();
                        break;

                    default:
                        MessageBox.Show("New Data currently unavailable");
                        break;
                }

                // 3. Assign data to grid
                MainDataGrid.ItemsSource = (System.Collections.IEnumerable)result;
            }
            catch (Exception ex)
            {
                /* Log error */
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                // 4. Always hide the spinner, even if an error occurs
                LoadingSpinner.Visibility = Visibility.Collapsed;
            }
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