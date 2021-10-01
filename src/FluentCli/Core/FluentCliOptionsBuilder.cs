using FluentCli.Exceptions;
using FluentCli.Mapping;
using FluentCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentCli.Core
{
    internal class FluentCliOptionsBuilder<TOptions> : IFluentCliOptionsBuilder<TOptions>
    {
        private List<FluentCliOption> _options = new();
        private OptionsMap<TOptions> _optionsMap;

        public IFluentCliOptionsBuilder<TOptions> WithMap(OptionsMap<TOptions> map)
        {
            _optionsMap = map;
            return this;
        }

        public IFluentCliOptionsBuilder<TOptions> AddOption(string shortName, string longName, bool required) =>
            AddOption(new FluentCliOption
            {
                ShortName = shortName,
                LongName = longName,
                Required = required
            });

        public IFluentCliOptionsBuilder<TOptions> AddOption(FluentCliOption option)
        {
            if (_options.Find(o => o.ShortName.Equals(option.ShortName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new DuplicateCommandOptionException(option.ShortName);

            if (_options.Find(o => o.LongName.Equals(option.LongName, StringComparison.OrdinalIgnoreCase)) != null)
                throw new DuplicateCommandOptionException(option.LongName);

            _options.Add(option);
            return this;
        }

        public FluentCliOptions Build()
        {
            switch (_optionsMap)
            {
                case null when _options.Any(): throw new Exception("Options without map");
                case null:
                    var map = PropertyMap<TOptions>.Create();
                    _options = map.Options;
                    _optionsMap = o => map.Create(o);
                    break;
            }

            return new() { Options = _options, OptionsMap = o => _optionsMap(o) };
        }
    }

    public interface IFluentCliOptionsBuilder<in TOptions>
    {
        IFluentCliOptionsBuilder<TOptions> WithMap(OptionsMap<TOptions> map);
        IFluentCliOptionsBuilder<TOptions> AddOption(string shortName, string longName, bool required);
        IFluentCliOptionsBuilder<TOptions> AddOption(FluentCliOption option);
        FluentCliOptions Build();
    }
}
