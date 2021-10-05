using System;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Builders;
using Wolfe.FluentCli.Internal;

namespace Wolfe.FluentCli
{
    public class Cli
    {
        public static IFluentCli Build(Action<ICliBuilder> cli)
        {
            var builder = CliBuilder.Create();
            cli?.Invoke(builder);
            return builder.Build();
        }
    }
}
