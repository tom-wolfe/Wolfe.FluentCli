namespace Wolfe.FluentCli.Parser.Definition
{
    internal record CliToken(CliTokenType Type, string Value)
    {
        public static readonly CliToken Eof = new(CliTokenType.Eof, null);
    }
}
