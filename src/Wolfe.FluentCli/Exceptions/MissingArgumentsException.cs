using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Wolfe.FluentCli.Exceptions
{
    [Serializable]
    public class MissingArgumentsException : Exception
    {
        public MissingArgumentsException() { }

        public string[] MissingArguments { get; }

        public MissingArgumentsException(IEnumerable<string> missingOptions)
            : this(missingOptions.ToArray()) { }

        public MissingArgumentsException(string[] missingArguments)
            : base($"The following required options are missing: {string.Join(", ", missingArguments)}")
        {
            MissingArguments = missingArguments;
        }

        public MissingArgumentsException(string message, Exception innerException)
            : base(message, innerException) { }

        protected MissingArgumentsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}