namespace Wolfe.FluentCli.Core.Builders
{
    /// <summary>
    /// Defines the contract for building a set of arguments to be passed to a command.
    /// </summary>
    /// <typeparam name="TArgs">The type of arguments to be passed to the command.</typeparam>
    public interface IArgumentsBuilder<TArgs>
    {
        /// <summary>
        /// Adds a single argument to the command.
        /// </summary>
        /// <param name="shortName">The short, usually single-letter name for the argument.</param>
        /// <param name="longName">The longer, kebab-cased name for the argument.</param>
        /// <param name="required">True if the argument must be supplied; otherwise false.</param>
        /// <returns>The current instance of <see cref="IArgumentsBuilder{TArgs}"/>.</returns>
        IArgumentsBuilder<TArgs> AddArgument(string shortName, string longName, bool required);

        /// <summary>
        /// Configures the arguments to be constructed from a custom factory method to facilitate custom types.
        /// </summary>
        /// <param name="factory">The factory method responsible for constructing the args instance.</param>
        /// <returns>The current instance of <see cref="IArgumentsBuilder{TArgs}"/>.</returns>
        IArgumentsBuilder<TArgs> UseFactory(ArgumentsFactory factory);
    }
}
