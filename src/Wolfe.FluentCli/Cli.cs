using System;
using Wolfe.FluentCli.Builders;
using Wolfe.FluentCli.Core;

namespace Wolfe.FluentCli
{
    public class Cli
    {
        public static IFluentCli Build(Action<IFluentCliBuilder> cli)
        {
            var builder = FluentCliBuilder.Create();
            cli?.Invoke(builder);
            return builder.Build();
        }
    }
}
