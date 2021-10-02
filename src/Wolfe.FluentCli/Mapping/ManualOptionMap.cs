using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Mapping
{
    internal class ManualOptionMap<TOptions> : IOptionMap
    {
        private readonly Func<Dictionary<string, CliArgument>, TOptions> _map;

        public ManualOptionMap(Func<Dictionary<string, CliArgument>, TOptions> map)
        {
            _map = map;
        }

        public object CreateFrom(Dictionary<string, CliArgument> values)
        {
            return _map(values);
        }
    }
}
