using System;

namespace Wolfe.FluentCli.Arguments
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CliArgumentAttribute : Attribute
    {
        public CliArgumentAttribute(string shortName, string longName, bool required = false)
        {
            ShortName = shortName;
            LongName = longName;
            Required = required;
        }

        public string ShortName { get; }
        public string LongName { get; }
        public bool Required { get; }
    }
}
