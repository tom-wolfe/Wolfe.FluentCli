using System;

namespace FluentCli.Mapping
{
    public interface ITypeConverter
    {
        public object Convert(string value, Type type);
    }
}
