using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Builders;
using Wolfe.FluentCli.Core.Internal;
using Wolfe.FluentCli.Exceptions;

namespace Wolfe.FluentCli.Internal
{
    internal class OptionsBuilder<TArgs> : IOptionsBuilder<TArgs>
    {
        private readonly List<CliOption> _parameters = new();
        private OptionFactory _optionFactory;

        public IOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required) =>
            AddOption(new CliOption
            {
                ShortName = shortName,
                LongName = longName,
                Required = required
            });

        public IOptionsBuilder<TArgs> AddOption(CliOption parameter)
        {
            if (_parameters.Find(o => o.ShortName.Equals(parameter.ShortName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new CliBuildException($"Command with short name {parameter.ShortName} already exists.");

            if (_parameters.Find(o => o.LongName.Equals(parameter.LongName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new CliBuildException($"Command with long name {parameter.LongName} already exists.");

            _parameters.Add(parameter);
            return this;
        }

        public IOptionsBuilder<TArgs> UseFactory(OptionFactory factory)
        {
            _optionFactory = factory;
            return this;
        }

        public CliOptions Build() => new(_parameters, _optionFactory);
    }
}
