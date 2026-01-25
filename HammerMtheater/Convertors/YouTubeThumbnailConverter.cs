using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HammerMtheater.Converters
{
    public class YouTubeThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            string url = value.ToString();
            string videoId = null;

            if (url.Contains("watch?v="))
                videoId = url.Split("watch?v=").Last();
            else if (url.Contains("youtu.be/"))
                videoId = url.Split("youtu.be/").Last();

            if (string.IsNullOrEmpty(videoId))
                return null;

            return $"https://img.youtube.com/vi/{videoId}/hqdefault.jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
