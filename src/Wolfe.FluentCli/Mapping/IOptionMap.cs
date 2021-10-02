using System.Collections.Generic;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Mapping
{
    public interface IOptionMap
    {
        public object CreateFrom(Dictionary<string, CliArgument> values);
    }
}
