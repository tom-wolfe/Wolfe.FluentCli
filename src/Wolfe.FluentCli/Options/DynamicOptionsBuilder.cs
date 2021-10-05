using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wolfe.FluentCli.Core.Models;

namespace Wolfe.FluentCli.Options
{
    internal static class DynamicOptionsBuilder
    {
        public static void UseDynamicArgs<TArgs>(this IOptionsBuilder<TArgs> options)
        {
            var map = new Dictionary<string, PropertyInfo>();
            var type = typeof(TArgs);
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var option = CreateOption(prop);
                options.AddOption(option);
                map.Add(option.LongName, prop);
            }

            var optionMap = new AutomaticOptionMap(type, map);

            options.UseMap(optionMap.CreateFrom);
        }

        private static CliOption CreateOption(PropertyInfo property)
        {
            var strategy = new KebabCasePropertyNamingStrategy();
            if (property.GetCustomAttributes(typeof(FluentCliOptionAttribute), true).FirstOrDefault() is not FluentCliOptionAttribute attribute)
            {
                return new CliOption
                {
                    ShortName = strategy.GetShortName(property),
                    LongName = strategy.GetLongName(property),
                    Required = false,
                    Description = string.Empty
                };
            }
            else
            {
                return new CliOption
                {
                    ShortName = attribute.ShortName,
                    LongName = attribute.LongName,
                    Required = attribute.Required,
                    Description = attribute.Description
                };
            }
        }
    }
}
