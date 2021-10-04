using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Mapping
{
    internal class ManualOptionMap<TArgs> : IOptionMap
    {
        private readonly Func<Dictionary<string, CliArgument>, TArgs> _map;

        public ManualOptionMap(Func<Dictionary<string, CliArgument>, TArgs> map)
        {
            _map = map;
        }

        public object CreateFrom(Dictionary<string, CliArgument> values)
        {
            return _map(values);
        }
    }
}
