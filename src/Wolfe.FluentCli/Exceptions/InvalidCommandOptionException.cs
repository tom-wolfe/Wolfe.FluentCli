using System;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    public class InvalidCommandOptionException : Exception
    {
        public InvalidCommandOptionException() { }

        public InvalidCommandOptionException(string commandName, string optionName)
            : base($"Command '{commandName}' has no option named {optionName}.") { }

        public InvalidCommandOptionException(string message, Exception innerException)
            : base(message, innerException) { }

        protected InvalidCommandOptionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}