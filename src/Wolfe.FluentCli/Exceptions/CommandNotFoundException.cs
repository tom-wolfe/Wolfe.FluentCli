using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException() { }

        public CommandNotFoundException(string message)
            : base(message) { }

        public CommandNotFoundException(IEnumerable<string> currentCommandChain, string commandName)
            : base($"Command '{commandName}' not found in chain '{string.Join(' ', currentCommandChain)}'") { }

        public CommandNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected CommandNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}