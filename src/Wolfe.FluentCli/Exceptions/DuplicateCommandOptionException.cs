using System;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    internal class DuplicateCommandOptionException : Exception
    {
        public DuplicateCommandOptionException() { }

        public DuplicateCommandOptionException(string optionName)
            : base($"An option named {optionName} has already been defined.") { }

        public DuplicateCommandOptionException(string message, Exception innerException)
            : base(message, innerException) { }

        protected DuplicateCommandOptionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}