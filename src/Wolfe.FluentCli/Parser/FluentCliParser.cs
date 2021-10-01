using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Parser
{
    internal class FluentCliParser : IFluentCliParser
    {
        private const char STRING_MARKER = '"';
        private static readonly string[] OptionMarkers = { "--", "-", "/" };

        public CliInstruction Parse(string args)
        {
            var parsedArgs = new List<string>();

            var stream = new Queue<char>(args);
            var buffer = new StringBuilder();

            while (stream.Count > 0)
            {
                var cur = stream.Dequeue();
                switch (cur)
                {
                    case STRING_MARKER:
                        while (true)
                        {
                            if (stream.Count == 0) { break; }
                            cur = stream.Dequeue();
                            if (cur == STRING_MARKER)
                            {
                                parsedArgs.Add(buffer.ToString());
                                buffer.Clear();
                                break;
                            }
                            buffer.Append(cur);
                        }

                        break;
                    case ' ':
                    case '\t':
                        if (buffer.Length > 0)
                        {
                            parsedArgs.Add(buffer.ToString());
                            buffer.Clear();
                        }
                        break;
                    default:
                        buffer.Append(cur);
                        break;
                }
            }
            if (buffer.Length > 0) { parsedArgs.Add(buffer.ToString()); }

            return ParseCore(parsedArgs);
        }

        public CliInstruction Parse(IEnumerable<string> args)
        {
            return ParseCore(args);
        }

        private static CliInstruction ParseCore(IEnumerable<string> args)
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
                    var optionValue = argQueue.Count == 0 || IsOption(argQueue.Peek()) ? true.ToString() : argQueue.Dequeue();
                    options.Add(StripOptionMarker(current), optionValue);
                }
                else
                {
                    // It's a command.
                    commands.Add(current.Trim());
                }
            }

            return new CliInstruction()
            {
                Commands = commands.ToArray(),
                Options = options
            };
        }

        private static bool IsOption(string input) => OptionMarkers.Any(input.StartsWith);
        private static string StripOptionMarker(string input) => input[OptionMarkers.First(input.StartsWith).Length..];
    }
}