using System;
using System.Collections.Generic;
using System.Linq;
using FluentCli.Models;

namespace FluentCli.Parser
{
    class FluentCliParser : IFluentCliParser
    {
        private static readonly string[] OptionMarkers = { "--", "-", "/" };

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
            var commands = new List<string>();
            var options = new Dictionary<string, string>();
            var argQueue = new Queue<string>(args);

            while (argQueue.Count > 0)
            {
                var current = argQueue.Dequeue();
                if (IsOption(current))
                {
                    // If the next arg is another option, default to true, otherwise consume the option value;
                    var optionValue = IsOption(argQueue.Peek()) ? true.ToString() : argQueue.Dequeue();
                    options.Add(StripOptionMarker(current), optionValue);
                }
                else
                {
                    // It's a command.
                    commands.Add(current.Trim());
                }
            }

            return new FluentCliInstruction()
            {
                Commands = commands.ToArray(),
                Options = options
            };
        }

        private static bool IsOption(string input) => OptionMarkers.Any(input.StartsWith);
        private static string StripOptionMarker(string input) => input[OptionMarkers.First(input.StartsWith).Length..];
    }
}