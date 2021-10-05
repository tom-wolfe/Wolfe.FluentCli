using System;

namespace Wolfe.FluentCli.Mapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentCliOptionAttribute : Attribute
    {
        public FluentCliOptionAttribute(string shortName, string longName, bool required = false, string description = null)
        {
            ShortName = shortName;
            LongName = longName;
            Required = required;
            Description = description;
        }

        public string ShortName { get; }
        public string LongName { get; }
        public bool Required { get; }
        public string Description { get; }
    }
}
