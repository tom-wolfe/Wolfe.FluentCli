using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Parser.Models
{
    public class CliDefinition : CliCommandDefinition
    {
        public static CliDefinition FromCommand(CliCommand command)
        {
            var def = new CliDefinition();
            AssignCommand(command, def);
            return def;
        }

        private static CliNamedCommandDefinition FromNamedCommand(CliNamedCommand command)
        {
            var def = new CliNamedCommandDefinition();
            def.Aliases.Add(command.Name);
            AssignCommand(command, def);
            return def;
        }

        private static void AssignCommand(CliCommand command, CliCommandDefinition def)
        {
            // TODO: Set unnamed args.
            foreach (var namedArg in command.Options.Options)
            {
                def.NamedArguments.Add(new CliNamedArgumentDefinition
                {
                    AllowedValues = AllowedValues.One,
                    LongName = namedArg.LongName,
                    ShortName = namedArg.ShortName
                });
            }

            foreach (var subcommand in command.SubCommands)
            {
                var subcommandDef = FromNamedCommand(subcommand);
                def.Commands.Add(subcommandDef);
            }
        }
    }
}
