﻿using System.Globalization;
namespace Esacco.Helpers
{
    public class IsIdNumberGreaterThanZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            // Check if the value is a string and try to parse it to an integer
            if (int.TryParse(value.ToString(), out int parsedIdNumber))
            {
                return parsedIdNumber > 0;
            }

            // If the value is not an int or cannot be parsed as an int, return false
            return false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
