namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliParameter
    {
        public string ShortName { get; init; }
        public string LongName { get; init; }
        public bool Required { get; init; }
        public AllowedValues AllowedValues { get; init; } = AllowedValues.One;
    }
}
