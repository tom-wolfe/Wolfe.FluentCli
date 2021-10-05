namespace Wolfe.FluentCli.Core.Models
{
    public class CliOption
    {
        public string ShortName { get; init; }
        public string LongName { get; init; }
        public string Description { get; init; }
        public bool Required { get; init; }
    }
}
