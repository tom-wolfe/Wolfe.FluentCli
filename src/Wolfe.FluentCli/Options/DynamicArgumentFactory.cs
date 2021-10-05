using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Internal;
using Wolfe.FluentCli.Internal;

namespace Wolfe.FluentCli.Options
{
    internal class DynamicArgumentFactory<TArgs>
    {
        private readonly Dictionary<string, (CliOption, PropertyInfo)> _propertyMap;

        public DynamicArgumentFactory()
        {
            _propertyMap = typeof(TArgs).GetProperties().Select(p => (CreateOption(p), p)).ToDictionary(t => t.Item1.LongName);
        }

        public object Create(CliContext context)
        {
            var model = Activator.CreateInstance<TArgs>();
            ApplyTo(model, context);
            return model;
        }

        public void ConfigureBuilder(OptionsBuilder<TArgs> options)
        {
            foreach (var option in _propertyMap)
                options.AddOption(option.Value.Item1);
        }

        private void ApplyTo(object model, CliContext context)
        {
            foreach (var arg in context.NamedArguments)
            {
                var map = _propertyMap[arg.Name];
                if (map == default) continue;

                
                // Check if it's a scalar/list value
                object argValue = GetPropertyAllowedValues(map.Item2) switch
                {
                    AllowedValues.None => true,
                    AllowedValues.One => arg.Value,
                    AllowedValues.Many => arg.Values,
                    _ => throw new Exception("Panic")
                };

                var propValue = Convert.ChangeType(argValue, map.Item2.PropertyType);
                map.Item2.SetValue(model, propValue);
            }
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
                    AllowedValues = GetPropertyAllowedValues(property)
                };
            }
            else
            {
                return new CliOption
                {
                    ShortName = attribute.ShortName,
                    LongName = attribute.LongName,
                    Required = attribute.Required,
                    AllowedValues = GetPropertyAllowedValues(property)
                };
            }
        }

        private static AllowedValues GetPropertyAllowedValues(PropertyInfo property)
        {
            if (property.PropertyType.IsAssignableFrom(typeof(List<string>)))
                return AllowedValues.Many;
            return property.PropertyType == typeof(bool) ? AllowedValues.None : AllowedValues.One;
        }
    }
}
