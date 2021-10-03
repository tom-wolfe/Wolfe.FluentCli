using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Mapping
{
    internal class ManualOptionMap<TArgs> : IOptionMap
    {
        private readonly Func<Dictionary<string, string>, TArgs> _map;

        public ManualOptionMap(Func<Dictionary<string, string>, TArgs> map)
        {
            _map = map;
        }

        public object CreateFrom(Dictionary<string, string> values)
        {
            return _map(values);
        }
    }
}
