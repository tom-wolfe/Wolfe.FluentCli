using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Exceptions;

namespace Wolfe.FluentCli.Core
{
    internal class ManualOptionsBuilder<TArgs> : IManualOptionsBuilder<TArgs>
    {
        private readonly List<CliOption> _options = new();
        private OptionFactory _optionFactory;

        public IManualOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required) =>
            AddOption(new CliOption
            {
                ShortName = shortName,
                LongName = longName,
                Required = required
            });

        public IManualOptionsBuilder<TArgs> AddOption(CliOption option)
        {
            if (_options.Find(o => o.ShortName.Equals(option.ShortName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new DuplicateCommandOptionException(option.ShortName);

            if (_options.Find(o => o.LongName.Equals(option.LongName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new DuplicateCommandOptionException(option.LongName);

            _options.Add(option);
            return this;
        }

        public IManualOptionsBuilder<TArgs> UseMap(OptionFactory factory)
        {
            _optionFactory = factory;
            return this;
        }

        public CliOptions Build()
        {
            return new() { Options = _options, OptionMap = _optionFactory };
        }
    }

    public interface IManualOptionsBuilder<TArgs>
    {
        IManualOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required);
        IManualOptionsBuilder<TArgs> AddOption(CliOption option);
        IManualOptionsBuilder<TArgs> UseMap(OptionFactory factory);
        CliOptions Build();
    }
}
