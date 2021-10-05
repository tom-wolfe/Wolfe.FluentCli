using System;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Core.Exceptions
{
    [Serializable]
    public class CliInterpreterException : Exception
    {
        public CliInterpreterException() { }
        public CliInterpreterException(string message) : base(message) { }
        public CliInterpreterException(string message, Exception innerException) : base(message, innerException) { }
        protected CliInterpreterException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
