using System;
using System.IO;
using System.Text;
using Wolfe.FluentCli.Parser.Definition;

namespace Wolfe.FluentCli.Parser
{
    internal class CliScanner : ICliScanner
    {
        private const int EOF = -1;
        private const char ASSIGNMENT = '=';
        private const char ESCAPE = '\\';
        private static readonly string StringMarkers = "\"'";
        private static readonly string ArgMarkers = "-/";

        private CliToken _peekedToken;

        private readonly TextReader _input;

        public CliScanner(string input)
            : this(new StringReader(input)) { }

        public CliScanner(TextReader input)
        {
            _input = input;
        }

        public CliToken Peek() => _peekedToken ??= ConsumeToken();

        public CliToken Read()
        {
            if (_peekedToken == null) { return ConsumeToken(); }

            var temp = _peekedToken;
            _peekedToken = null;
            return temp;
        }

        private CliToken ConsumeToken()
        {
            while (true)
            {
                var cur = _input.Peek();
                if (cur == EOF) { return CliToken.Eof; }
                var curChar = (char)cur;

                // Ignore whitespace
                if (char.IsWhiteSpace(curChar))
                {
                    _input.Read();
                    continue;
                }

                if (StringMarkers.Contains(curChar))
                    return ScanQuotedString();
                if (curChar == ASSIGNMENT)
                    return ScanAssignment();
                return ArgMarkers.Contains(curChar)
                    ? ScanArgumentMarker()
                    : ScanIdentifier();
            }
        }

        private CliToken ScanQuotedString()
        {
            var marker = (char)_input.Read();
            var quotedString = SeekUntil(c => c == marker, allowEscape: true);
            _input.Read(); // Consume the closing marker
            return new CliToken(CliTokenType.StringLiteral, quotedString);
        }

        private CliToken ScanAssignment()
        {
            var symbol = ((char)_input.Read()).ToString();
            return new CliToken(CliTokenType.Assignment, symbol);
        }

        private CliToken ScanIdentifier()
        {
            var identifier = SeekUntil(c => !char.IsLetterOrDigit(c) && c != '-');
            return new CliToken(CliTokenType.Identifier, identifier);
        }

        private CliToken ScanArgumentMarker()
        {
            var marker = ((char)_input.Read()).ToString();
            var type = CliTokenType.ShortArgumentMarker;
            if (marker == "-" && _input.Peek() == '-')
            {
                marker += (char)_input.Read();
                type = CliTokenType.LongArgumentMarker;
            }
            return new CliToken(type, marker);
        }

        private string SeekUntil(Func<char, bool> condition, bool allowEscape = false)
        {
            var buffer = new StringBuilder();
            while (true)
            {
                var cur = _input.Peek();
                if (cur == EOF) { break; }
                var curChar = (char)cur;
                if (curChar == ESCAPE && allowEscape)
                {
                    _input.Read(); // Consume the escape character.
                    buffer.Append((char)_input.Read());
                    continue;
                }
                if (condition(curChar)) { break; }
                buffer.Append(curChar);
                _input.Read();
            }
            return buffer.ToString();
        }
    }
}
