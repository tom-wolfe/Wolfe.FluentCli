using System;

namespace FluentCli.Mapping
{
    internal class DefaultTypeConverter : ITypeConverter
    {
        public object Convert(string value, Type type)
        {
            return System.Convert.ChangeType(value, type);
        }
    }
}
