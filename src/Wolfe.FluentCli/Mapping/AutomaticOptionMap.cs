using System;
using System.Collections.Generic;
using System.Reflection;
using Wolfe.FluentCli.Core.Models;

namespace Wolfe.FluentCli.Mapping
{
    internal class AutomaticOptionMap
    {
        private readonly Type _model;
        private readonly Dictionary<string, PropertyInfo> _propertyMap;

        public AutomaticOptionMap(Type model, Dictionary<string, PropertyInfo> map)
        {
            _model = model;
            _propertyMap = map;
        }

        public object CreateFrom(Dictionary<string, CliArgument> values)
        {
            var model = Activator.CreateInstance(_model);
            ApplyTo(model, values);
            return model;
        }

        private void ApplyTo(object model, Dictionary<string, CliArgument> values)
        {
            foreach (var (key, value) in values)
            {
                var property = _propertyMap[key];
                if (property == null) continue;

                var propValue = Convert.ChangeType(value.Value, property.PropertyType);
                property.SetValue(model, propValue);
            }
        }
    }
}
