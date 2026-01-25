using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HammerMtheater.UserControls
{
    public partial class SeatControl : UserControl
    {
        public int SeatNumber { get; private set; }
        public bool IsAvailable { get; private set; }

        public event RoutedEventHandler? SeatSelected;

        public SeatControl(int seatNumber, bool isAvailable)
        {
            InitializeComponent();

            SeatNumber = seatNumber;
            IsAvailable = isAvailable;

            SeatText.Text = seatNumber.ToString();
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (IsAvailable)
                Root.Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)); // ירוק
            else
                Root.Background = new SolidColorBrush(Color.FromRgb(120, 120, 120)); // אפור
        }

        private void Seat_Click(object sender, MouseButtonEventArgs e)
        {
            if (!IsAvailable)
                return;

            SeatSelected?.Invoke(this, new RoutedEventArgs());
        }

        public void SetSelected(bool selected)
        {
            if (!IsAvailable)
                return;

            Root.Background = selected
                ? Brushes.Red
                : new SolidColorBrush(Color.FromRgb(76, 175, 80));
        }
    }
}
