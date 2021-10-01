using System;
using System.Text;

namespace FluentCli.Extensions
{
    internal static class StringExtensions
    {
        public static string ToKebabCase(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.Length < 2) return input;

            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(input[0]));
            for (var i = 1; i < input.Length; ++i)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    sb.Append('-');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
