using System;
using System.Collections.Generic;
using System.Reflection;

namespace Wolfe.FluentCli.Mapping
{
    internal class AutomaticOptionMap : IOptionMap
    {
        private readonly Type _model;
        private readonly Dictionary<string, PropertyInfo> _propertyMap;
        private readonly ITypeConverter _typeConverter;

        public AutomaticOptionMap(Type model, Dictionary<string, PropertyInfo> map, ITypeConverter typeConverter)
        {
            _model = model;
            _propertyMap = map;
            _typeConverter = typeConverter ?? new DefaultTypeConverter();
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
                var property = _propertyMap[key];
                if (property == null) continue;

                var propValue = _typeConverter.Convert(value, property.PropertyType);
                property.SetValue(model, propValue);
            }
        }
    }
}
