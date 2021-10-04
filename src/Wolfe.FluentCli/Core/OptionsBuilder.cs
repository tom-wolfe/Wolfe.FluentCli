using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wolfe.FluentCli.Mapping;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Core
{
    internal class OptionsBuilder<TArgs> : IOptionsBuilder<TArgs>
    {
        private IPropertyNamingStrategy _namingStrategy;
        private ITypeConverter _typeConverter;

        public IOptionsBuilder<TArgs> UseNamingStrategy(IPropertyNamingStrategy strategy)
        {
            _namingStrategy = strategy;
            return this;
        }

        public IOptionsBuilder<TArgs> UseTypeConverter(ITypeConverter converter)
        {
            _typeConverter = converter;
            return this;
        }

        public CliOptions Build()
        {
            var map = new Dictionary<string, PropertyInfo>();
            var options = new List<CliOption>();

            var type = typeof(TArgs);
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var option = CreateOption(prop);
                options.Add(option);
                map.Add(option.LongName, prop);
            }

            return new CliOptions()
            {
                Options = options,
                OptionMap = new AutomaticOptionMap(type, map, _typeConverter)
            };
        }

        private CliOption CreateOption(PropertyInfo property)
        {
            var strategy = _namingStrategy ?? new KebabCasePropertyNamingStrategy();

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

    public interface IOptionsBuilder<TArgs>
    {
        IOptionsBuilder<TArgs> UseNamingStrategy(IPropertyNamingStrategy strategy);
        IOptionsBuilder<TArgs> UseTypeConverter(ITypeConverter converter);
        CliOptions Build();
    }
}
