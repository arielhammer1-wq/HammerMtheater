using Model;
using MoviesInterface;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

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
            MoviesFunctions api = new MoviesFunctions();
            MovieList movies = await api.GetAllMovies();

            _allGenres = movies
                .GroupBy(m => m.Genre.GenreName)
                .Select(g => new GenreGroup
                {
                    GenreName = g.Key,
                    Movies = g.ToList()
                })
                .ToList();

            Genres = _allGenres;
            Refresh();
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
                            .Where(m => m.MovieName.ToLower().Contains(text))
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
