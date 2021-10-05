using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Core.Build;
using Wolfe.FluentCli.Core.Build.Internal;
using Wolfe.FluentCli.Core.Build.Public;
using Wolfe.FluentCli.Exceptions;

namespace Wolfe.FluentCli.Internal
{
    internal class OptionsBuilder<TArgs> : IOptionsBuilder<TArgs>
    {
        private readonly List<CliOption> _parameters = new();
        private OptionFactory _optionFactory;

        public IOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required) =>
            AddOption(new CliParameter
            {
                ShortName = shortName,
                LongName = longName,
                Required = required
            });

        public IOptionsBuilder<TArgs> AddOption(CliParameter parameter) => AddOption(new CliOption()
        {
            LongName = parameter.LongName,
            ShortName = parameter.ShortName,
            Required = parameter.Required
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

        public CliOptions Build()
        {
            return new() { Options = _parameters, OptionMap = _optionFactory };
        }
    }
}
