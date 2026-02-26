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
            MovieFields.Visibility = (_mode == "Movies") ? Visibility.Visible : Visibility.Collapsed;
            TheaterFields.Visibility = (_mode == "Theaters") ? Visibility.Visible : Visibility.Collapsed;
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
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MoviesFunctions api = new MoviesFunctions();

                if (_mode == "Movies")
                {
                    Movie m;

                    if (_isEdit)
                    {
                        // We use the object we ALREADY HAVE. 
                        // It definitely has the correct ID because we clicked it in the grid.
                        m = (Movie)_item;
                    }
                    else
                    {
                        m = new Movie();
                    }

                    // Now we fill the data. If _isEdit is true, m.Id is already set!
                    m.MovieName = TxtName.Text;
                    m.MovieLength = int.Parse(TxtLength.Text);
                    m.ReleaseDate = DateTime.Parse(TxtDate.Text);
                     = new AgeRating { Id = int.Parse(TxtAge.Text) };
                    int ageId = int.Parse(TxtAge.Text);
                    m.AgeRatingName = await api(ageId);
                    int genreId = int.Parse(TxtGenre.Text);
                    m.Genre = await api.GetMovieGenreById(genreId);
                    m.PosterUrl = TxtPoster.Text;
                    m.TrailerUrl = TxtTrailer.Text;

                    if (_isEdit)
                        await api.UpdateMovie(m);

                }
                else if (_mode == "Theaters")
                {
                    Theater t = _isEdit ? (Theater)_item : new Theater();
                    t.NameOfTheater = TxtName.Text;
                    t.Address = TxtAddress.Text;
                    t.StreetNumber = int.Parse(TxtStreet.Text);
                    int code = int.Parse(TxtCity.Text);
                    t.CityCode = await api.GetCityById(code);
                    

                    if (_isEdit) await api.UpdateTheater(t);
                    
                }

                MessageBox.Show("Saved Successfully!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical Error: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}