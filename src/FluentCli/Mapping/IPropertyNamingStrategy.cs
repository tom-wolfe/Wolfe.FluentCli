using System.Reflection;

namespace FluentCli.Mapping
{
    public interface IPropertyNamingStrategy
    {
        public string GetShortName(PropertyInfo property);
        public string GetLongName(PropertyInfo property);
    }
}
