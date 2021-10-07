using System.Collections.Generic;
using System.Linq;

namespace Wolfe.FluentCli.Core
{
    /// <summary>
    /// Represents an argument passed to a CLI command.
    /// </summary>
    public class CliArgument
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CliArgument"/> with a scalar value.
        /// </summary>
        /// <param name="value">The single value of the argument.</param>
        public CliArgument(string value) { Values = new List<string> { value }; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliArgument"/> with a list of values.
        /// </summary>
        /// <param name="values">The list of values that make up the argument.</param>
        public CliArgument(List<string> values) { Values = values ?? new List<string>(); }

        /// <summary>
        /// Gets the scalar value passed to the argument. 
        /// Where the argument supports multiple values, this property returns the first.
        /// </summary>
        public string Value => Values.FirstOrDefault();

        /// <summary>
        /// Gets the list of values passed to the argument. 
        /// Where the argument is a scalar value, this property returns a list containing the single value.
        /// </summary>
        public List<string> Values { get; }
    }
}
