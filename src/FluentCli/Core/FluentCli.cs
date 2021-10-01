using FluentCli.Exceptions;
using FluentCli.Models;
using FluentCli.Parser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentCli.Core
{
    public class FluentCli : IFluentCli
    {
        private readonly IFluentCliParser _parser;
        private readonly IServiceProvider _serviceProvider;
        private readonly FluentCliCommand _rootCommand;

        public FluentCli(IServiceProvider serviceProvider, IFluentCliParser parser, FluentCliCommand rootCommand)
        {
            _parser = parser;
            _serviceProvider = serviceProvider;
            _rootCommand = rootCommand;
        }

        public Task Execute(string args)
        {
            var instruction = _parser.Parse(args);
            var command = ResolveCommand(instruction);
            return ExecuteCore(command);
        }

        public Task Execute(params string[] args)
        {
            var instruction = _parser.Parse(args);
            var command = ResolveCommand(instruction);
            return ExecuteCore(command);
        }

        protected virtual FluentCliCommand ResolveCommand(FluentCliInstruction instruction)
        {
            var currentCommand = _rootCommand;
            var currentCommandChain = new List<string>();
            foreach (var commandName in instruction.Commands)
            {
                var nextCommand = currentCommand.SubCommands.Find(c => c.Name.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));
                currentCommand = nextCommand ?? throw new MissingCommandException(currentCommandChain, commandName);
                currentCommandChain.Add(commandName);
            }

            return currentCommand;
        }

        protected virtual Task ExecuteCore(FluentCliCommand command)
        {
            var handler = _serviceProvider.GetService(command.Handler);
            var typedHandler = handler as ICommandHandler ?? throw new InvalidCommandHandlerException(command.Name);
            return typedHandler.Execute();
        }
    }
}
