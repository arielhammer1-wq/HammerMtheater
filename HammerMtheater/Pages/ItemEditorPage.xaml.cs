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

            // Show relevant fields based on mode
            if (_mode == "Movies")
            {
                MovieFields.Visibility = Visibility.Visible;
                TheaterFields.Visibility = Visibility.Collapsed;
            }
            else
            {
                MovieFields.Visibility = Visibility.Collapsed;
                TheaterFields.Visibility = Visibility.Visible;
            }

            PageHeader.Text = _isEdit ? $"Edit {_mode}" : $"Insert New {_mode}";
            if (_isEdit) FillFields();
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
                    Movie m = _isEdit ? (Movie)_item : new Movie();
                    m.MovieName = TxtName.Text;
                    m.MovieLength = int.Parse(TxtLength.Text);
                    m.AgeRatingName = new AgeRating { Id = int.Parse(TxtAge.Text) };
                    m.ReleaseDate = DateTime.Parse(TxtDate.Text);
                    m.Genre = new MovieGenre { Id = int.Parse(TxtGenre.Text) };
                    m.PosterUrl = TxtPoster.Text;
                    m.TrailerUrl = TxtTrailer.Text;

                    if (_isEdit) await api.UpdateMovie(m);
                    else await api.InsertMovie(m);
                }
                else
                {
                    Theater t = _isEdit ? (Theater)_item : new Theater();
                    t.NameOfTheater = TxtName.Text;
                    t.Address = TxtAddress.Text;
                    t.StreetNumber = int.Parse(TxtStreet.Text);
                    t.CityCode = new City { Id = int.Parse(TxtCity.Text) };

                    if (_isEdit) await api.UpdateTheater(t);
                    else await api.InsertTheater(t);
                }

                MessageBox.Show("Success!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Please check that dates and ID numbers are correct.\n\n" + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}