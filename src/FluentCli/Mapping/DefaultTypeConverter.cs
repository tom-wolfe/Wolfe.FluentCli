using System;
using System.Collections.Generic;

namespace FluentCli.Mapping
{
    internal class DefaultTypeConverter : ITypeConverter
    {
        private static readonly Dictionary<Type, Func<string, object>> TypeConverters = new()
        {
            //{ typeof(int), v => int.Parse(v) },
            //{ typeof(float), v => float.Parse(v) },
            //{ typeof(decimal), v => decimal.Parse(v) },
            //{ typeof(double), v => double.Parse(v) },
            //{ typeof(long), v => long.Parse(v) },
            //{ typeof(DateTime), v => DateTime.Parse(v) },
        };

        public object Convert(string value, Type type)
        {
            return TypeConverters.ContainsKey(type) 
                ? TypeConverters[type].Invoke(value) 
                : System.Convert.ChangeType(value, type);
        }
    }
}
