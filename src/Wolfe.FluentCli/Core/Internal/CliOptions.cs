using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliOptions
    {
        public CliOptions() : this(null, null) { }

        public CliOptions(List<CliOption> options, OptionFactory factory)
        {
            Options = options ?? new List<CliOption>();
            Factory = factory ?? (_ => null);
        }

        public List<CliOption> Options { get; }
        public OptionFactory Factory { get; }
    }
}
