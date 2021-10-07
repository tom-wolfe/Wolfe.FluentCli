using System.Collections.Generic;

namespace Wolfe.FluentCli.Core
{
    /// <summary>
    /// Represents a named argument passed to a CLI command.
    /// </summary>
    public class CliNamedArgument : CliArgument
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CliNamedArgument"/> with a scalar value.
        /// </summary>
        /// <param name="name">The name of the argument. Where the argument has multiple aliases, this is the long/formal name.</param>
        /// <param name="value">The single value of the argument.</param>
        public CliNamedArgument(string name, string value) : base(value) { Name = name; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliNamedArgument"/> with a list of values.
        /// </summary>
        /// <param name="name">The name of the argument. Where the argument has multiple aliases, this is the long/formal name.</param>
        /// <param name="values">The list of values that make up the argument.</param>
        public CliNamedArgument(string name, List<string> values) : base(values) { Name = name; }

        /// <summary>
        /// Gets the long/formal name of the argument.
        /// </summary>
        public string Name { get; }
    }
}
