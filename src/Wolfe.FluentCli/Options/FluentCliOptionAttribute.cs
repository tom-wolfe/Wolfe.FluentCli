using System;

namespace Wolfe.FluentCli.Options
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentCliOptionAttribute : Attribute
    {
        public FluentCliOptionAttribute(string shortName, string longName, bool required = false)
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
