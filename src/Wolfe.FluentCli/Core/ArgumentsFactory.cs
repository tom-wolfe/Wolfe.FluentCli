namespace Wolfe.FluentCli.Core
{
    /// <summary>
    /// Describes a method that constructs a new insance of the arguments required to execute a CLI command.
    /// </summary>
    /// <param name="context">The command and the arguments from which to construct the arguments.</param>
    /// <returns>A new instance of the command arguments object.</returns>
    public delegate object ArgumentsFactory(CliContext context);
}
