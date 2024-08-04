using System.Text;

namespace Hiwu.SpecificationPattern.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 2)
                return value;

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string ToSnakeCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    if (i > 0)
                        sb.Append('_');
                    sb.Append(char.ToLowerInvariant(value[i]));
                }
                else
                {
                    sb.Append(value[i]);
                }
            }
            return sb.ToString();
        }
    }
}
