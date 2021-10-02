using System;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    public class CommandHandlerException : Exception
    {
        public CommandHandlerException() { }

        public CommandHandlerException(string commandName, string message)
            : base($"Error with '{commandName}' handler: {message}") { }

        protected CommandHandlerException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}