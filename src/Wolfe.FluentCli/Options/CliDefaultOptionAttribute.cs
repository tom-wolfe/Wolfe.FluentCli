using System;

namespace Wolfe.FluentCli.Options
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CliDefaultOptionAttribute : Attribute
    {
        public CliDefaultOptionAttribute(bool required = false)
        {
            Required = required;
        }

        public bool Required { get; }
    }
}
