using Wolfe.FluentCli.Parser;
using Wolfe.FluentCli.Parser.Models;
using Wolfe.FluentCli.Tests.Extensions;
using Xunit;

namespace Wolfe.FluentCli.Tests.Parser
{
    public class CliScannerTests
    {
        [Fact]
        public void EmptyString_ReturnsEof()
        {
            new CliScanner("")
                .AssertReadToken(CliToken.Eof);
        }

        [Fact]
        public void ShortArgumentMarker_ReturnsShortArgumentMarker()
        {
            new CliScanner("-")
                .AssertReadToken(CliTokenType.ShortArgumentMarker, "-");
        }

        [Fact]
        public void LongArgumentMarker_ReturnsLongArgumentMarker()
        {
            new CliScanner("--")
                .AssertReadToken(CliTokenType.LongArgumentMarker, "--");
        }

        [Fact]
        public void Identifier_ReturnsIdentifier()
        {
            new CliScanner("foo")
                .AssertReadToken(CliTokenType.Identifier, "foo");
        }

        [Fact]
        public void QuotedString_ReturnsQuotedString()
        {
            new CliScanner("\"foo bar\"")
                .AssertReadToken(CliTokenType.StringLiteral, "foo bar");
        }

        [Fact]
        public void ShortArgument_ReturnsShortArgument()
        {
            new CliScanner("-foo")
                .AssertReadToken(CliTokenType.ShortArgumentMarker, "-")
                .AssertReadToken(CliTokenType.Identifier, "foo");
        }

        [Fact]
        public void ShortArgumentWithValue_ReturnsShortArgumentWithValue()
        {
            new CliScanner("-foo blah")
                .AssertReadToken(CliTokenType.ShortArgumentMarker, "-")
                .AssertReadToken(CliTokenType.Identifier, "foo")
                .AssertReadToken(CliTokenType.Identifier, "blah");
        }

        [Fact]
        public void ShortArgumentWithAssignment_ReturnsShortArgumentWithAssignment()
        {
            new CliScanner("-foo=blah")
                .AssertReadToken(CliTokenType.ShortArgumentMarker, "-")
                .AssertReadToken(CliTokenType.Identifier, "foo")
                .AssertReadToken(CliTokenType.Assignment, "=")
                .AssertReadToken(CliTokenType.Identifier, "blah");
        }

        [Fact]
        public void AlternateStringMarkers_DoesNotTerminate()
        {
            new CliScanner("'single \"quoted\" string'")
                .AssertReadToken(CliTokenType.StringLiteral, "single \"quoted\" string");
        }

        [Fact]
        public void EscapedQuote_DoesNotTerminate()
        {
            new CliScanner("'George\\'s Marvelous Medicine'")
                .AssertReadToken(CliTokenType.StringLiteral, "George's Marvelous Medicine");
        }

        [Fact]
        public void Complex_ScansCorrectly()
        {
            new CliScanner("--foo-bar=blah \"test spaces\" /arg name")
                .AssertReadToken(CliTokenType.LongArgumentMarker, "--")
                .AssertReadToken(CliTokenType.Identifier, "foo-bar")
                .AssertReadToken(CliTokenType.Assignment, "=")
                .AssertReadToken(CliTokenType.Identifier, "blah")
                .AssertReadToken(CliTokenType.StringLiteral, "test spaces")
                .AssertReadToken(CliTokenType.ShortArgumentMarker, "/")
                .AssertReadToken(CliTokenType.Identifier, "arg")
                .AssertReadToken(CliTokenType.Identifier, "name");
        }
    }
}
