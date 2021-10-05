using System;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Core.Exceptions
{
    [Serializable]
    public class CliExecutionException : Exception
    {
        public CliExecutionException() { }
        public CliExecutionException(string message) : base(message) { }
        public CliExecutionException(string message, Exception innerException) : base(message, innerException) { }
        protected CliExecutionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
