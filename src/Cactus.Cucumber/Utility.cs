using System.Globalization;

namespace Cactus.Cucumber
{
    public class Utility
    {
        //NOTE: Convert scientific notation to decimal should ONLY be used when comparing files for convenience, but NOT when converting Excel to feature!
        public static string ConvertScientificToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            {
                return value;
            }

            return RemoveTrailingZeros(result.ToString("0.#############################", CultureInfo.InvariantCulture));
        }

        public static string FormatDateTime(DateTime value)
        {
            string format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
                          + " "
                          + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
            return value.ToString(format, CultureInfo.CurrentCulture);
        }

        public static string NormalizeBooleanString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Trim().Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return "True";
            }

            if (value.Trim().Equals("False", StringComparison.OrdinalIgnoreCase))
            {
                return "False";
            }

            return value;
        }

        public static string RemoveTrailingZeros(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            int dotIndex = value.IndexOf('.');
            if (dotIndex <= 0 || dotIndex == value.Length - 1)
            {
                return value;
            }

            int startIndex = value[0] == '-' || value[0] == '+' ? 1 : 0;
            for (int i = startIndex; i < value.Length; i++)
            {
                if (i == dotIndex)
                {
                    continue;
                }

                if (!char.IsDigit(value[i]))
                {
                    return value;
                }
            }

            int endIndex = value.Length - 1;
            while (endIndex > dotIndex && value[endIndex] == '0')
            {
                endIndex--;
            }

            if (endIndex == dotIndex)
            {
                endIndex--;
            }

            return value.Substring(0, endIndex + 1);
        }
    }
}