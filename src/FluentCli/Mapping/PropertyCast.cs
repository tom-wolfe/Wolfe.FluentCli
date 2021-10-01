using System;
using System.Reflection;

namespace FluentCli.Mapping
{
    internal class PropertyCast
    {
        public PropertyInfo Property { get; init; }
        public Func<string, object> Cast { get; init; }
    }
}
