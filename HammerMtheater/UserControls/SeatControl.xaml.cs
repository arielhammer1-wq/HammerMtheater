using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HammerMtheater.UserControls
{
    public partial class SeatControl : UserControl
    {
        public int SeatNumber { get; }
        public bool IsAvailable { get; }
        public bool IsSelected { get; private set; }

        public event RoutedEventHandler? SeatSelected;

        public SeatControl(int seatNumber, bool isAvailable)
        {
            InitializeComponent();
            SeatNumber = seatNumber;
            IsAvailable = isAvailable;

            if (SeatText != null) SeatText.Text = seatNumber.ToString();
            UpdateColor();
        }

        private void Seat_Click(object sender, MouseButtonEventArgs e)
        {
            if (!IsAvailable) return;
            SeatSelected?.Invoke(this, new RoutedEventArgs());
        }

        public void ToggleSelected()
        {
            if (!IsAvailable) return;
            IsSelected = !IsSelected;
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (Root == null || Backrest == null || SeatCushion == null) return;

            if (!IsAvailable)
            {
                Backrest.Background = Brushes.Gray;
                SeatCushion.Background = Brushes.Gray;
                Root.Opacity = 0.5;
            }
            else if (IsSelected)
            {
                Backrest.Background = Brushes.Red;
                SeatCushion.Background = Brushes.Red;
                Root.Opacity = 1.0;
            }
            else
            {
                Backrest.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
                SeatCushion.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
                Root.Opacity = 1.0;
            }
        }
    }
}