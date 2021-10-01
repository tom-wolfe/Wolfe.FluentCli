using System.Reflection;

namespace Wolfe.FluentCli.Mapping
{
    public interface IPropertyNamingStrategy
    {
        public string GetShortName(PropertyInfo property);
        public string GetLongName(PropertyInfo property);
    }
}
