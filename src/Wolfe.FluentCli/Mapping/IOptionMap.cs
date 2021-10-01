using System.Collections.Generic;

namespace Wolfe.FluentCli.Mapping
{
    public interface IOptionMap
    {
        public object CreateFrom(Dictionary<string, string> values);
    }
}
