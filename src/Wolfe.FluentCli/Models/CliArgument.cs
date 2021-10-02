namespace Wolfe.FluentCli.Models
{
    public class CliArgument
    {
        public CliArgument() : this(null) { }
        public CliArgument(object value) { Value = value; }

        public object Value { get; }
    }
}
