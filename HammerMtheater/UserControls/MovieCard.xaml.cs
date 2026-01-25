using Model;
using System.Windows.Controls;
using System.Windows.Input;

namespace HammerMtheater.UserControls
{
    public partial class MovieCard : UserControl
    {
        public MovieCard()
        {
            InitializeComponent();
        }

        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is Movie movie)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow)
                    .MainFrame.Navigate(
                        new Pages.MovieDetails(movie)
                    );
            }
        }
    }
}
