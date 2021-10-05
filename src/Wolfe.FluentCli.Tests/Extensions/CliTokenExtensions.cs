using Wolfe.FluentCli.Parser.Definition;
using Xunit;

namespace Wolfe.FluentCli.Tests.Extensions
{
    internal static class CliTokenExtensions
    {
        public static void AssertEqual(this CliToken token, CliToken other)
        {
            Assert.Equal(other, token);
        }

        public static void AssertEqual(this CliToken token, CliTokenType type, string value)
        {
            Assert.NotNull(token);
            Assert.Equal(type, token.Type);
            Assert.Equal(value, token.Value);
        }
    }
}
