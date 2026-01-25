using HammerMtheater;
using Model;
using MoviesInterface;
using System.Linq;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class MoviePage : Page
    {
        public MoviePage()
        {
            InitializeComponent();
            LoadMovies();
        }

        private async void LoadMovies()
        {
            MoviesFunctions api = new MoviesFunctions();
            MovieList movies = await api.GetAllMovies();

            var genres = movies
                .GroupBy(m => m.Genre.GenreName)
                .Select(g => new GenreGroup
                {
                    GenreName = g.Key,
                    Movies = g.ToList()
                })
                .ToList();

            DataContext = new { Genres = genres };
        }
    }
}
