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
                .AssertNextToken(CliToken.Eof);
        }

        [Fact]
        public void ShortArgumentMarker_ReturnsShortArgumentMarker()
        {
            new CliScanner("-")
                .AssertNextToken(CliTokenType.ShortArgumentMarker, "-");
        }

        [Fact]
        public void LongArgumentMarker_ReturnsLongArgumentMarker()
        {
            new CliScanner("--")
                .AssertNextToken(CliTokenType.LongArgumentMarker, "--");
        }

        [Fact]
        public void Identifier_ReturnsIdentifier()
        {
            new CliScanner("foo")
                .AssertNextToken(CliTokenType.Identifier, "foo");
        }

        [Fact]
        public void QuotedString_ReturnsQuotedString()
        {
            new CliScanner("\"foo bar\"")
                .AssertNextToken(CliTokenType.StringLiteral, "foo bar");
        }

        [Fact]
        public void ShortArgument_ReturnsShortArgument()
        {
            new CliScanner("-foo")
                .AssertNextToken(CliTokenType.ShortArgumentMarker, "-")
                .AssertNextToken(CliTokenType.Identifier, "foo");
        }

        [Fact]
        public void ShortArgumentWithValue_ReturnsShortArgumentWithValue()
        {
            new CliScanner("-foo blah")
                .AssertNextToken(CliTokenType.ShortArgumentMarker, "-")
                .AssertNextToken(CliTokenType.Identifier, "foo")
                .AssertNextToken(CliTokenType.Identifier, "blah");
        }

        [Fact]
        public void ShortArgumentWithAssignment_ReturnsShortArgumentWithAssignment()
        {
            new CliScanner("-foo=blah")
                .AssertNextToken(CliTokenType.ShortArgumentMarker, "-")
                .AssertNextToken(CliTokenType.Identifier, "foo")
                .AssertNextToken(CliTokenType.Assignment, "=")
                .AssertNextToken(CliTokenType.Identifier, "blah");
        }

        [Fact]
        public void AlternateStringMarkers_DoesNotTerminate()
        {
            new CliScanner("'single \"quoted\" string'")
                .AssertNextToken(CliTokenType.StringLiteral, "single \"quoted\" string");
        }

        [Fact]
        public void EscapedQuote_DoesNotTerminate()
        {
            new CliScanner("'George\\'s Marvelous Medicine'")
                .AssertNextToken(CliTokenType.StringLiteral, "George's Marvelous Medicine");
        }

        [Fact]
        public void Complex_ScansCorrectly()
        {
            new CliScanner("--foo-bar=blah \"test spaces\" /arg name")
                .AssertNextToken(CliTokenType.LongArgumentMarker, "--")
                .AssertNextToken(CliTokenType.Identifier, "foo-bar")
                .AssertNextToken(CliTokenType.Assignment, "=")
                .AssertNextToken(CliTokenType.Identifier, "blah")
                .AssertNextToken(CliTokenType.StringLiteral, "test spaces")
                .AssertNextToken(CliTokenType.ShortArgumentMarker, "/")
                .AssertNextToken(CliTokenType.Identifier, "arg")
                .AssertNextToken(CliTokenType.Identifier, "name");
        }
    }
}
