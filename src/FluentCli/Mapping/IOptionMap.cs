using System.Collections.Generic;

namespace FluentCli.Mapping
{
    public interface IOptionMap
    {
        public object CreateFrom(Dictionary<string, string> values);
    }
}
