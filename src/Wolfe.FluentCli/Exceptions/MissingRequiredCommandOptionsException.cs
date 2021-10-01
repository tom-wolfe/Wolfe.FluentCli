using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    internal class MissingRequiredCommandOptionsException : Exception
    {
        public MissingRequiredCommandOptionsException() { }

        public MissingRequiredCommandOptionsException(IEnumerable<string> missingOptions)
            : base($"The following required options are missing: {string.Join(", ", missingOptions)}") { }

        public MissingRequiredCommandOptionsException(string message, Exception innerException)
            : base(message, innerException) { }

        protected MissingRequiredCommandOptionsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}