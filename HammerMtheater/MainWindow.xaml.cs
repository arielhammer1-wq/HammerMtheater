using System.Windows;
using HammerMtheater.Pages;

namespace HammerMtheater
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Load Login page
            MainFrame.Navigate(new Login());
        }
    }
}
