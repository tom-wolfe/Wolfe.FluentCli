#nullable enable
using System;

namespace FluentCli.Service
{
    class DefaultServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => Activator.CreateInstance(serviceType);
    }
}
