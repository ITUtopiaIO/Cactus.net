namespace Cactus.Cucumber
{
    public class Utility
    {
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