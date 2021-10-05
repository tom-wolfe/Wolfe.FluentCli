using Wolfe.FluentCli.Core.Internal;

namespace Wolfe.FluentCli.Parser.Definition
{
    internal class CliDefinition : CliCommandDefinition
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
            def.Unnamed.AllowedValues = AllowedValues.None;

            foreach (var namedArg in command.Options.Options)
            {
                def.NamedArguments.Add(new CliNamedArgumentDefinition
                {
                    AllowedValues = namedArg.AllowedValues,
                    LongName = namedArg.LongName,
                    ShortName = namedArg.ShortName
                });
            }

            foreach (var cmd in command.Commands)
            {
                var cmdDef = FromNamedCommand(cmd);
                def.Commands.Add(cmdDef);
            }
        }
    }
}
