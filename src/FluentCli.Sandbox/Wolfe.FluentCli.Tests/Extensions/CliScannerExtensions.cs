using Wolfe.FluentCli.Parser;
using Wolfe.FluentCli.Parser.Models;

namespace Wolfe.FluentCli.Tests.Extensions
{
    internal static class CliScannerExtensions
    {
        public static ICliScanner AssertNextToken(this ICliScanner scanner, CliTokenType type, string value)
        {
            var nextToken = scanner.GetNextToken();
            nextToken.AssertEqual(type, value);
            return scanner;
        }

        public static ICliScanner AssertNextToken(this ICliScanner scanner, CliToken token)
        {
            var nextToken = scanner.GetNextToken();
            nextToken.AssertEqual(token);
            return scanner;
        }
    }
}
