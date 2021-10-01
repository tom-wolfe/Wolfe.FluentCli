#nullable enable
using System;

namespace FluentCli.Core
{
    internal class DefaultServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => Activator.CreateInstance(serviceType);
    }
}
