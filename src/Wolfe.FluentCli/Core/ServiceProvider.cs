#nullable enable
using System;

namespace Wolfe.FluentCli.Core
{
    /// <summary>
    /// Describes a method that returns an instance of the type specified by <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to provide an instance of.</param>
    /// <returns>An instance of the type specified by <paramref name="type"/>.</returns>
    public delegate object? ServiceProvider(Type type);
}
