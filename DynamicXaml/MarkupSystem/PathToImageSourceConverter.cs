using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DynamicXaml.MarkupSystem
{
    public class PathToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                          object parameter, CultureInfo culture)
        {
            var uri = "pack://application:,,," + (string) value;
            try
            {
				if (string.IsNullOrEmpty((string)value))
					return new BitmapImage();

                var bitmapImage = new BitmapImage(new Uri(uri));
                return bitmapImage;
            }
            catch(Exception x)
            {
                Debug.WriteLine("Path " + uri + " could not be parsed:" + x.Message);
                return new BitmapImage();
            }
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Backward conversion not implemented");
        }
    }
}