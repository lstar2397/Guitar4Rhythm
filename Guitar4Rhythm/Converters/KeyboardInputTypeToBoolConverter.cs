using System;
using System.Globalization;
using System.Windows.Data;
using Guitar4Rhythm.WindowsInput.Native;

namespace Guitar4Rhythm.Converters {
    public class KeyboardInputTypeToBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is KeyboardInputType &&
                parameter is KeyboardInputType) {
                return value.Equals(parameter);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool &&
                parameter is KeyboardInputType && (bool)value) {
                return parameter;
            }

            return Binding.DoNothing;
        }
    }
}
