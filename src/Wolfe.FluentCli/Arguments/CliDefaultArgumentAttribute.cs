using System;

namespace Wolfe.FluentCli.Arguments
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CliDefaultArgumentAttribute : Attribute
    {
        public CliDefaultArgumentAttribute(bool required = false)
        {
            Required = required;
        }

        public bool Required { get; }
    }
}
