using Model;
using MoviesInterface;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class CastPage : Page
    {
        private int _movieId;
        private readonly MoviesFunctions _functions = new MoviesFunctions();

        public CastPage(int movieId)
        {
            InitializeComponent();
            _movieId = movieId;
            // We must call this to trigger the API request
            Loaded += async (s, e) => await LoadCastAsync();
        }

        private async Task LoadCastAsync()
        {
            // CHECK THIS in your Output window:
            System.Diagnostics.Debug.WriteLine($"Loading cast for Movie ID: {_movieId}");

            var castList = await _functions.GetArtistsByMovieId(_movieId);

            if (castList != null && castList.Count > 0)
            {
                ActorsList.ItemsSource = castList;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}