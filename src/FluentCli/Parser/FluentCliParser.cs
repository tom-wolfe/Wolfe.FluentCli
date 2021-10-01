using System;
using System.Linq;
using FluentCli.Models;

namespace FluentCli.Parser
{
    class FluentCliParser : IFluentCliParser
    {
        public FluentCliInstruction Parse(string args)
        {
            // TODO: Make this more flexible to allow for quoted strings.
            var argsArray = args.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return ParseCore(argsArray);
        }

        public FluentCliInstruction Parse(string[] args)
        {
            return ParseCore(args);
        }

        protected virtual FluentCliInstruction ParseCore(string[] args)
        {
            var commands = args
                .Select(a => a.ToLower().Trim())
                .Where(a => !string.IsNullOrEmpty(a))
                .ToArray();

            return new FluentCliInstruction()
            {
                Commands = commands
            };
        }
    }
}