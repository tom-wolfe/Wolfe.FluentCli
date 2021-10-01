using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FluentCli.Exceptions
{
    [Serializable]
    internal class MissingCommandException : Exception
    {
        public MissingCommandException() { }

        public MissingCommandException(string message)
            : base(message) { }

        public MissingCommandException(IEnumerable<string> currentCommandChain, string commandName)
            : base($"Command '{commandName}' not found in chain '{string.Join(' ', currentCommandChain)}'") { }

        public MissingCommandException(string message, Exception innerException)
            : base(message, innerException) { }

        protected MissingCommandException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}