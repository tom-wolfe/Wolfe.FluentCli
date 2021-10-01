using FluentCli.Exceptions;
using FluentCli.Mapping;
using FluentCli.Models;
using System;
using System.Collections.Generic;

namespace FluentCli.Core
{
    internal class ManualOptionsBuilder<TOptions> : IManualOptionsBuilder<TOptions>
    {
        private readonly List<CliOption> _options = new();
        private IOptionMap _optionMap;

        public IManualOptionsBuilder<TOptions> AddOption(string shortName, string longName, bool required) =>
            AddOption(new CliOption
            {
                ShortName = shortName,
                LongName = longName,
                Required = required
            });

        public IManualOptionsBuilder<TOptions> AddOption(CliOption option)
        {
            if (_options.Find(o => o.ShortName.Equals(option.ShortName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new DuplicateCommandOptionException(option.ShortName);

            if (_options.Find(o => o.LongName.Equals(option.LongName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new DuplicateCommandOptionException(option.LongName);

            _options.Add(option);
            return this;
        }

        public IManualOptionsBuilder<TOptions> UseMap(Func<Dictionary<string, string>, TOptions> map)
        {
            _optionMap = new ManualOptionMap<TOptions>(map);
            return this;
        }

        public CliOptions Build()
        {
            return new() { Options = _options, OptionMap = _optionMap };
        }
    }

    public interface IManualOptionsBuilder<TOptions>
    {
        IManualOptionsBuilder<TOptions> AddOption(string shortName, string longName, bool required);
        IManualOptionsBuilder<TOptions> AddOption(CliOption option);
        IManualOptionsBuilder<TOptions> UseMap(Func<Dictionary<string, string>, TOptions> map);
        CliOptions Build();
    }
}
