using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliArguments
    {
        public CliArguments() : this(null, null) { }

        public CliArguments(List<CliParameter> parameters, ArgumentsFactory factory)
        {
            Parameters = parameters ?? new List<CliParameter>();
            Factory = factory ?? (_ => null);
        }

        public List<CliParameter> Parameters { get; }
        public ArgumentsFactory Factory { get; }
    }
}
