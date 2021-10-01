#nullable enable
using System;

namespace FluentCli.Service
{
    internal class DefaultServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => Activator.CreateInstance(serviceType);
    }
}
