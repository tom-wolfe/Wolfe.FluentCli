namespace Wolfe.FluentCli.Parser.Models
{
    internal enum CliTokenType
    {
        Identifier,
        ShortArgumentMarker,
        LongArgumentMarker,
        Assignment,
        StringLiteral,
        Eof,
    }
}
