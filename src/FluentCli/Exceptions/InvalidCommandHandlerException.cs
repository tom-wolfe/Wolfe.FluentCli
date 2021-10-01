using System;
using System.Runtime.Serialization;

namespace FluentCli.Exceptions
{
    [Serializable]
    internal class InvalidCommandHandlerException : Exception
    {
        public InvalidCommandHandlerException() { }

        public InvalidCommandHandlerException(string commandName)
            : base($"Handler for command '{commandName}' is invalid.") { }

        public InvalidCommandHandlerException(string message, Exception innerException)
            : base(message, innerException) { }

        protected InvalidCommandHandlerException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}