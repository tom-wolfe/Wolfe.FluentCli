using System;

namespace FluentCli
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
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
