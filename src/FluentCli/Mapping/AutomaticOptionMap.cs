using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentCli.Mapping
{
    internal class AutomaticOptionMap : IOptionMap
    {
        private readonly Type _model;
        private readonly Dictionary<string, PropertyCast> _propertyMap;

        public AutomaticOptionMap(Type model, Dictionary<string, PropertyCast> map)
        {
            _model = model;
            _propertyMap = map;
        }

        public object CreateFrom(Dictionary<string, string> values)
        {
            var model = Activator.CreateInstance(_model);
            ApplyTo(model, values);
            return model;
        }

        private void ApplyTo(object model, Dictionary<string, string> values)
        {
            foreach (var (key, value) in values)
            {
                var (_, propertyCast) =
                    _propertyMap.FirstOrDefault(k => k.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (propertyCast == null) continue;

                var propValue = propertyCast.Cast?.Invoke(value) ?? value;
                propertyCast.Property.SetValue(model, propValue);
            }
        }
    }
}
