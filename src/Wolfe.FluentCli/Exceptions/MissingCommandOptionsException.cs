using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    public class MissingCommandOptionsException : Exception
    {
        public MissingCommandOptionsException() { }

        public MissingCommandOptionsException(IEnumerable<string> missingOptions)
            : base($"The following required options are missing: {string.Join(", ", missingOptions)}") { }

        public MissingCommandOptionsException(string message, Exception innerException)
            : base(message, innerException) { }

        protected MissingCommandOptionsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}