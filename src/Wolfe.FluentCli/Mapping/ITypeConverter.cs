using System;

namespace Wolfe.FluentCli.Mapping
{
    public interface ITypeConverter
    {
        public object Convert(string value, Type type);
    }
}
