using Wolfe.FluentCli.Parser;
using Wolfe.FluentCli.Parser.Definition;

namespace Wolfe.FluentCli.Tests.Extensions
{
    internal static class CliScannerExtensions
    {
        public static ICliScanner AssertReadToken(this ICliScanner scanner, CliTokenType type, string value)
        {
            var nextToken = scanner.Read();
            nextToken.AssertEqual(type, value);
            return scanner;
        }

        public static ICliScanner AssertReadToken(this ICliScanner scanner, CliToken token)
        {
            var nextToken = scanner.Read();
            nextToken.AssertEqual(token);
            return scanner;
        }
    }
}
