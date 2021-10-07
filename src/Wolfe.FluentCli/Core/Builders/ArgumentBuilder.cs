using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Core.Internal;
using Wolfe.FluentCli.Exceptions;

namespace Wolfe.FluentCli.Core.Builders
{
    internal class ArgumentBuilder<TArgs> : IArgumentsBuilder<TArgs>
    {
        private readonly List<CliParameter> _parameters = new();
        private ArgumentsFactory argsFactory;
        
        public IArgumentsBuilder<TArgs> AddArgument(string shortName, string longName, bool required) =>
            AddArgument(new CliParameter
            {
                ShortName = shortName,
                LongName = longName,
                Required = required
            });

        public IArgumentsBuilder<TArgs> AddArgument(CliParameter arg)
        {
            if (_parameters.Find(o => o.ShortName.Equals(arg.ShortName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new CliBuildException($"Command with short name {arg.ShortName} already exists.");

            if (_parameters.Find(o => o.LongName.Equals(arg.LongName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new CliBuildException($"Command with long name {arg.LongName} already exists.");

            _parameters.Add(arg);
            return this;
        }

        public IArgumentsBuilder<TArgs> UseFactory(ArgumentsFactory factory)
        {
            argsFactory = factory;
            return this;
        }

        public CliArguments Build() => new(_parameters, argsFactory);
    }
}
