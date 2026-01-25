using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)
                .MainFrame.Navigate(new MoviePage());
        }
    }
}
