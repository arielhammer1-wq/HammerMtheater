using Model;
using MoviesInterface;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class ItemEditorPage : Page
    {
        private string _mode;
        private object _item;
        private bool _isEdit;
        private MoviesFunctions _api = new MoviesFunctions();

        public ItemEditorPage(string mode, object item = null)
        {
            InitializeComponent();

            _mode = mode;
            _item = item;
            _isEdit = item != null;

            SetupUI();

            this.Loaded += async (s, e) =>
            {
                await LoadComboBoxes();

                if (_isEdit)
                    FillFields();
            };
        }

        private void SetupUI()
        {
            PageHeader.Text = _isEdit ? $"Edit {_mode}" : $"Insert New {_mode}";

            MovieFields.Visibility = (_mode == "Movies") ? Visibility.Visible : Visibility.Collapsed;
            TheaterFields.Visibility = (_mode == "Theaters") ? Visibility.Visible : Visibility.Collapsed;

            TxtInfo.Visibility = (_mode == "Users") ? Visibility.Visible : Visibility.Collapsed;

            if (_isEdit)
                BtnDelete.Visibility = Visibility.Visible;
        }

        private async Task LoadComboBoxes()
        {
            try
            {
                if (_mode == "Movies")
                {
                    CmbAgeRating.ItemsSource = await _api.GetAllAgeRatingInMovies();
                    CmbGenre.ItemsSource = await _api.GetAllMovieGenres();
                }
                else if (_mode == "Theaters")
                {
                    CmbCity.ItemsSource = await _api.GetAllCities();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dropdowns: " + ex.Message);
            }
        }

        private void FillFields()
        {
            if (_mode == "Movies" && _item is Movie m)
            {
                TxtName.Text = m.MovieName;
                TxtLength.Text = m.MovieLength.ToString();
                TxtDate.Text = m.ReleaseDate.ToString("yyyy-MM-dd");
                TxtPoster.Text = m.PosterUrl;
                TxtTrailer.Text = m.TrailerUrl;

                CmbAgeRating.SelectedValue = m.AgeRatingName?.Id;
                CmbGenre.SelectedValue = m.Genre?.Id;
            }
            else if (_mode == "Theaters" && _item is Theater t)
            {
                TxtName.Text = t.NameOfTheater;
                TxtAddress.Text = t.Address;
                TxtStreet.Text = t.StreetNumber.ToString();

                CmbCity.SelectedValue = t.CityCode?.Id;
            }
            else if (_mode == "Users" && _item is User u)
            {
                TxtName.Text = u.Username;
                TxtInfo.Text = u.Email;
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_mode == "Movies")
                {
                    if (CmbAgeRating.SelectedItem == null)
                    {
                        MessageBox.Show("Please choose an age rating");
                        return;
                    }

                    if (CmbGenre.SelectedItem == null)
                    {
                        MessageBox.Show("Please choose a genre");
                        return;
                    }

                    Movie m = _isEdit ? (Movie)_item : new Movie();

                    m.MovieName = TxtName.Text;
                    m.MovieLength = int.Parse(TxtLength.Text);
                    m.ReleaseDate = DateTime.Parse(TxtDate.Text);
                    m.AgeRatingName = CmbAgeRating.SelectedItem as AgeRating;
                    m.Genre = CmbGenre.SelectedItem as MovieGenre;
                    m.PosterUrl = TxtPoster.Text;
                    m.TrailerUrl = TxtTrailer.Text;

                    if (_isEdit)
                        await _api.UpdateMovie(m);
                    else
                        await _api.InsertMovie(m);
                }
                else if (_mode == "Theaters")
                {
                    if (CmbCity.SelectedItem == null)
                    {
                        MessageBox.Show("Please choose a city");
                        return;
                    }

                    Theater t = _isEdit ? (Theater)_item : new Theater();

                    t.NameOfTheater = TxtName.Text;
                    t.Address = TxtAddress.Text;
                    t.StreetNumber = int.Parse(TxtStreet.Text);
                    t.CityCode = CmbCity.SelectedItem as City;

                    if (_isEdit)
                        await _api.UpdateTheater(t);
                    else
                        await _api.InsertTheater(t);
                }
                else if (_mode == "Users")
                {
                    User u = _isEdit ? (User)_item : new User();

                    u.Username = TxtName.Text;
                    u.Email = TxtInfo.Text;

                    if (_isEdit)
                        await _api.UpdateUser(u);
                    else
                        await _api.InsertUser(u);
                }

                MessageBox.Show("Saved Successfully!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Saving: " + ex.Message);
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete this {_mode}?",
                "Confirm",
                MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                if (_mode == "Users" && _item is User u)
                {
                    await _api.DeleteUser(u);
                }
                else if (_mode == "Movies" && _item is Movie m)
                {
                    await _api.DeleteMovie(m);
                }
                else if (_mode == "Theaters" && _item is Theater t)
                {
                    await _api.DeleteTheater(t);
                }

                MessageBox.Show("Deleted Successfully!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Deleting: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}