using System;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    public class CliBuildException : Exception
    {
        public CliBuildException() { }
        public CliBuildException(string message) : base(message) { }
        public CliBuildException(string message, Exception innerException) : base(message, innerException) { }
        protected CliBuildException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}