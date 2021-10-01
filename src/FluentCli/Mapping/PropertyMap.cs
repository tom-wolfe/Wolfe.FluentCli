using FluentCli.Extensions;
using FluentCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentCli.Mapping
{
    internal class PropertyMap<T>
    {
        private readonly Dictionary<string, PropertyCast> _propertyMap;

        private PropertyMap(Dictionary<string, PropertyCast> map, List<FluentCliOption> options)
        {
            _propertyMap = map;
            Options = options;
        }

        public List<FluentCliOption> Options { get; }

        public T Create(Dictionary<string, string> values)
        {
            var model = Activator.CreateInstance<T>();
            Apply(model, values);
            return model;
        }

        private void Apply(T model, Dictionary<string, string> values)
        {
            foreach (var (key, value) in values)
            {
                var (_, propertyCast) = _propertyMap.FirstOrDefault(k => k.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (propertyCast == null) continue;

                var propValue = propertyCast.Cast?.Invoke(value) ?? value;
                propertyCast.Property.SetValue(model, propValue);
            }
        }

        public static PropertyMap<T> Create()
        {
            var map = new Dictionary<string, PropertyCast>();
            var options = new List<FluentCliOption>();

            var type = typeof(T);
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var option = new FluentCliOption()
                {
                    ShortName = prop.Name[..1].ToLowerInvariant(),
                    LongName = prop.Name.ToKebabCase(),
                    Required = false,
                    Description = string.Empty
                };
                options.Add(option);

                map.Add(option.LongName, new PropertyCast()
                {
                    Property = prop,
                    Cast = x => x
                });
            }

            // TODO: Implement.

            return new PropertyMap<T>(map, options);
        }
    }
}
