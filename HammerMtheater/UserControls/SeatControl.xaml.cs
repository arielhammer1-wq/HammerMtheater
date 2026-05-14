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
        public bool IsSelected { get; private set; }

        public event RoutedEventHandler SeatSelected;

        // 1. Required empty constructor for WPF Visual Tree
        public SeatControl()
        {
            InitializeComponent();
        }

        // 2. Chain to the empty constructor using : this()
        public SeatControl(int seatNumber, bool isAvailable) : this()
        {
            SeatNumber = seatNumber;
            IsAvailable = isAvailable;
            IsSelected = false;

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
            if (!IsAvailable) return;

            IsSelected = !IsSelected;
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (!IsAvailable)
            {
                Backrest.Background = Brushes.Gray;
                SeatCushion.Background = Brushes.Gray;
                Root.Opacity = 0.45;
                return;
            }

            if (IsSelected)
            {
                Backrest.Background = new SolidColorBrush(Color.FromRgb(0, 200, 150));
                SeatCushion.Background = new SolidColorBrush(Color.FromRgb(0, 200, 150));
                Root.Opacity = 1.0;
                return;
            }

            Backrest.Background = new SolidColorBrush(Color.FromRgb(37, 42, 49));
            SeatCushion.Background = new SolidColorBrush(Color.FromRgb(37, 42, 49));
            Root.Opacity = 1.0;
        }
    }
}