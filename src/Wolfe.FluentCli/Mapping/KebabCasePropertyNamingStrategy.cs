using System;
using System.Reflection;
using System.Text;

namespace Wolfe.FluentCli.Mapping
{
    public class KebabCasePropertyNamingStrategy : IPropertyNamingStrategy
    {
        public string GetShortName(PropertyInfo property)
        {
            return property.Name[..1].ToLowerInvariant();
        }

        public string GetLongName(PropertyInfo property)
        {
            return ToKebabCase(property.Name);
        }

        private static string ToKebabCase(string input)
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
