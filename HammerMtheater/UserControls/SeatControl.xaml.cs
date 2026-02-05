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

            SeatText.Text = seatNumber.ToString();
            UpdateColor();
        }

        private void Seat_Click(object sender, MouseButtonEventArgs e)
        {
            if (!IsAvailable) return;
            SeatSelected?.Invoke(this, new RoutedEventArgs());
        }

        public void ToggleSelected()
        {
            IsSelected = !IsSelected;
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (!IsAvailable)
                Root.Background = Brushes.Gray;
            else if (IsSelected)
                Root.Background = Brushes.Red;
            else
                Root.Background = Brushes.Green;
        }
    }
}
