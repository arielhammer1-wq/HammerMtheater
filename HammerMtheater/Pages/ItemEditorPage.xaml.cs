using Model;
using MoviesInterface;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
            _isEdit = (item != null);

            SetupUI();
            if (_isEdit) FillFields();
        }

        private void SetupUI()
        {
            PageHeader.Text = _isEdit ? $"Edit {_mode}" : $"Insert New {_mode}";

            // Show fields based on mode
            MovieFields.Visibility = (_mode == "Movies") ? Visibility.Visible : Visibility.Collapsed;
            TheaterFields.Visibility = (_mode == "Theaters") ? Visibility.Visible : Visibility.Collapsed;

            // For Users, we use TxtInfo for the Email
            TxtInfo.Visibility = (_mode == "Users") ? Visibility.Visible : Visibility.Collapsed;

            // Show Delete button only if we are EDITING an existing item
            if (_isEdit) BtnDelete.Visibility = Visibility.Visible;
        }

        private void FillFields()
        {
            if (_mode == "Movies" && _item is Movie m)
            {
                TxtName.Text = m.MovieName;
                TxtLength.Text = m.MovieLength.ToString();
                TxtAge.Text = m.AgeRatingName?.Id.ToString();
                TxtDate.Text = m.ReleaseDate.ToString("yyyy-MM-dd");
                TxtGenre.Text = m.Genre?.Id.ToString();
                TxtPoster.Text = m.PosterUrl;
                TxtTrailer.Text = m.TrailerUrl;
            }
            else if (_mode == "Theaters" && _item is Theater t)
            {
                TxtName.Text = t.NameOfTheater;
                TxtAddress.Text = t.Address;
                TxtStreet.Text = t.StreetNumber.ToString();
                TxtCity.Text = t.CityCode?.Id.ToString();
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
                    Movie m = _isEdit ? (Movie)_item : new Movie();
                    m.MovieName = TxtName.Text;
                    m.MovieLength = int.Parse(TxtLength.Text);
                    m.ReleaseDate = DateTime.Parse(TxtDate.Text);
                    m.AgeRatingName = await _api.GetAgeRatingById(int.Parse(TxtAge.Text));
                    m.Genre = await _api.GetMovieGenreById(int.Parse(TxtGenre.Text));
                    m.PosterUrl = TxtPoster.Text;
                    m.TrailerUrl = TxtTrailer.Text;

                    if (_isEdit) await _api.UpdateMovie(m);
                    else await _api.InsertMovie(m);
                }
                else if (_mode == "Theaters")
                {
                    Theater t = _isEdit ? (Theater)_item : new Theater();
                    t.NameOfTheater = TxtName.Text;
                    t.Address = TxtAddress.Text;
                    t.StreetNumber = int.Parse(TxtStreet.Text);
                    t.CityCode = await _api.GetCityById(int.Parse(TxtCity.Text));

                    if (_isEdit) await _api.UpdateTheater(t);
                    else await _api.InsertTheater(t);
                }
                else if (_mode == "Users")
                {
                    User u = _isEdit ? (User)_item : new User();
                    u.Username = TxtName.Text;
                    u.Email = TxtInfo.Text;

                    if (_isEdit) await _api.UpdateUser(u);
                    else await _api.InsertUser(u);
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
            var result = MessageBox.Show($"Are you sure you want to delete this {_mode}?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (_mode == "Users" && _item is User u)
                    {
                        // Pass the full User object 'u' to match your Task<int> DeleteUser(User user)
                        await _api.DeleteUser(u);
                    }
                    else if (_mode == "Movies" && _item is Movie m)
                    {
                        // Ensure your DeleteMovie in the interface also takes the object if needed
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
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}