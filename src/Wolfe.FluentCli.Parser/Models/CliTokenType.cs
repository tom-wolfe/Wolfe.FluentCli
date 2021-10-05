namespace Wolfe.FluentCli.Parser.Models
{
    public enum CliTokenType
    {
        Identifier,
        ShortArgumentMarker,
        LongArgumentMarker,
        Assignment,
        StringLiteral,
        Eof,
    }
}
