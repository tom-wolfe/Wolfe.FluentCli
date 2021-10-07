using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliArguments
    {
        public CliArguments() : this(null, null) { }

        public CliArguments(List<CliParameter> options, ArgumentsFactory factory)
        {
            Options = options ?? new List<CliParameter>();
            Factory = factory ?? (_ => null);
        }

        public List<CliParameter> Options { get; }
        public ArgumentsFactory Factory { get; }
    }
}
