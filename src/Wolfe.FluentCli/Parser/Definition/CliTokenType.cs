namespace Wolfe.FluentCli.Parser.Definition
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
