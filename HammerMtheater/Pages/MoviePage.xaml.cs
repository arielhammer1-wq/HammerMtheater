using Model;
using MoviesInterface;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace HammerMtheater.Pages
{
    public partial class MoviePage : Page
    {
        // רשימת מקור (לא נוגעים בה)
        private List<GenreGroup> _allGenres = new();

        // רשימה מוצגת
        public List<GenreGroup> Genres { get; set; } = new();

        public MoviePage()
        {
            InitializeComponent();
            DataContext = this;
            LoadMovies();
        }

        private async void LoadMovies()
        {
            // Start loader (Add the LoadingOverlay Grid to your XAML if you haven't yet)
            // LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                MoviesFunctions api = new MoviesFunctions();
                MovieList movies = await api.GetAllMovies();

                _allGenres = movies
                    .GroupBy(m => m.Genre.GenreName)
                    .Select(g => new GenreGroup
                    {
                        GenreName = g.Key.ToUpper(), // Uppercase for that premium look
                        Movies = g.ToList()
                    })
                    .OrderBy(g => g.GenreName) // Sort alphabetically
                    .ToList();

                Genres = _allGenres;
                Refresh();
            }
            catch (Exception ex)
            {
                // Handle error elegantly
            }
            finally
            {
                // LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (e.Delta > 0)
            {
                scrollViewer.LineLeft();
                scrollViewer.LineLeft();
            }
            else
            {
                scrollViewer.LineRight();
                scrollViewer.LineRight();
            }
            e.Handled = true;
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = SearchBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(text))
            {
                Genres = _allGenres;
            }
            else
            {
                Genres = _allGenres
                    .Select(g => new GenreGroup
                    {
                        GenreName = g.GenreName,
                        Movies = g.Movies
                            .Where(m =>
                                m.MovieName.ToLower().Contains(text) ||
                                g.GenreName.ToLower().Contains(text)
                            )
                            .ToList()
                    })
                    .Where(g => g.Movies.Count > 0)
                    .ToList();
            }

            Refresh();
        }

       

        private void Refresh()
        {
            DataContext = null;
            DataContext = this;
        }
    }
}
