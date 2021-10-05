using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Builders;
using Wolfe.FluentCli.Core.Internal;

namespace Wolfe.FluentCli.Options
{
    internal class DynamicArgumentFactory<TArgs>
    {
        private const string UNNAMED_ARG_NAME = "";
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
            var unnamed = _propertyMap[UNNAMED_ARG_NAME];
            SetArgumentValue(model, unnamed.Item2, context.UnnamedArguments);

            foreach (var arg in context.NamedArguments)
            {
                var (_, property) = _propertyMap[arg.Name];
                if (property == null) continue;

                SetArgumentValue(model, property, arg);
            }
        }

        private static void SetArgumentValue(object model, PropertyInfo property, CliArgument arg)
        {
            // Check if it's a scalar/list value
            object argValue = GetPropertyAllowedValues(property) switch
            {
                AllowedValues.None => true,
                AllowedValues.One => arg.Value,
                AllowedValues.Many => arg.Values,
                _ => throw new Exception("Panic")
            };

            var propValue = Convert.ChangeType(argValue, property.PropertyType);
            property.SetValue(model, propValue);
        }

        private static CliOption CreateOption(PropertyInfo property)
        {
            var strategy = new KebabCasePropertyNamingStrategy();
            if (property.GetCustomAttributes(typeof(CliOptionAttribute), true).FirstOrDefault() is CliOptionAttribute attribute)
            {
                return new CliOption
                {
                    ShortName = attribute.ShortName,
                    LongName = attribute.LongName,
                    Required = attribute.Required,
                    AllowedValues = GetPropertyAllowedValues(property)
                };
            }
            if (property.GetCustomAttributes(typeof(CliDefaultOptionAttribute), true).FirstOrDefault() is CliDefaultOptionAttribute attr)
            {
                return new CliOption
                {
                    ShortName = UNNAMED_ARG_NAME,
                    LongName = UNNAMED_ARG_NAME,
                    Required = attr.Required,
                    AllowedValues = GetPropertyAllowedValues(property)
                };
            }

            return new CliOption
            {
                ShortName = strategy.GetShortName(property),
                LongName = strategy.GetLongName(property),
                Required = false,
                AllowedValues = GetPropertyAllowedValues(property)
            };
        }

        private static AllowedValues GetPropertyAllowedValues(PropertyInfo property)
        {
            if (property.PropertyType.IsAssignableFrom(typeof(List<string>)))
                return AllowedValues.Many;
            return property.PropertyType == typeof(bool) ? AllowedValues.None : AllowedValues.One;
        }
    }
}
