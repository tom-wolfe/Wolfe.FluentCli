using System;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Builders;

namespace Wolfe.FluentCli
{
    public static class Cli
    {
        public static IFluentCli Build(Action<ICliBuilder> cli)
        {
            var builder = CliBuilder.Create();
            cli?.Invoke(builder);
            return builder.Build();
        }
    }
}
