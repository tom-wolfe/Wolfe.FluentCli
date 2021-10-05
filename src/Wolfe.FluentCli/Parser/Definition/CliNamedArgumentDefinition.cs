namespace Wolfe.FluentCli.Parser.Models
{
    internal class CliNamedArgumentDefinition : CliArgumentDefinition
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }
}
