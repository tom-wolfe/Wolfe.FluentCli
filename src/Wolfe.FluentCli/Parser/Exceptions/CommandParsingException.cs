using System;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Parser.Exceptions
{
    [Serializable]
    public class CommandParsingException : Exception
    {
        public CommandParsingException() { }

        public CommandParsingException(string message) : base(message) { }

        protected CommandParsingException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
