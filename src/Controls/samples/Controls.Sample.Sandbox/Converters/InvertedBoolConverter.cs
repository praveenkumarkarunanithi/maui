using System.Globalization;

namespace Maui.Controls.Sample.Converters
{
	/// <summary>
	/// Converts a boolean value to its inverted state.
	/// Used to show bot messages when IsUser is false.
	/// </summary>
	public class InvertedBoolConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return !boolValue;
			}
			return false;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return !boolValue;
			}
			return false;
		}
	}
}
