#nullable enable
using System;

namespace Wolfe.FluentCli.Core
{
    internal class DefaultServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => Activator.CreateInstance(serviceType);
    }
}
