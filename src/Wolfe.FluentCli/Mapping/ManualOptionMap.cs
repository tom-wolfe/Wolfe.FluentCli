using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Mapping
{
    internal class ManualOptionMap<TOptions> : IOptionMap
    {
        private readonly Func<Dictionary<string, string>, TOptions> _map;

        public ManualOptionMap(Func<Dictionary<string, string>, TOptions> map)
        {
            _map = map;
        }

        public object CreateFrom(Dictionary<string, string> values)
        {
            return _map(values);
        }
    }
}
