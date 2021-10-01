using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentCli.Mapping;
using FluentCli.Models;

namespace FluentCli.Core
{
    internal class OptionsBuilder<TOptions> : IOptionsBuilder<TOptions>
    {
        private IPropertyNamingStrategy _namingStrategy;
        private ITypeConverter _typeConverter;

        public IOptionsBuilder<TOptions> UseNamingStrategy(IPropertyNamingStrategy strategy)
        {
            _namingStrategy = strategy;
            return this;
        }

        public IOptionsBuilder<TOptions> UseTypeConverter(ITypeConverter converter)
        {
            _typeConverter = converter;
            return this;
        }

        public CliOptions Build()
        {
            var map = new Dictionary<string, PropertyInfo>();
            var options = new List<CliOption>();

            var type = typeof(TOptions);
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

    public interface IOptionsBuilder<TOptions>
    {
        IOptionsBuilder<TOptions> UseNamingStrategy(IPropertyNamingStrategy strategy);
        IOptionsBuilder<TOptions> UseTypeConverter(ITypeConverter converter);
        CliOptions Build();
    }
}
