using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentCli.Exceptions;
using FluentCli.Models;

namespace FluentCli.Core
{
    public class FluentCliOptionsBuilder<TOptions> : IFluentCliOptionsBuilder<TOptions>
    {
        private readonly Type _model;
        private readonly List<FluentCliOption> _options = new();

        protected FluentCliOptionsBuilder(Type model)
        {
            _model = model;
        }

        public static FluentCliOptionsBuilder<TOptions> Create() => new(typeof(TOptions));

        public IFluentCliOptionsBuilder<TOptions> AutoMap()
        {
            // TODO: Make this work.
            throw new NotImplementedException();
        }

        public IFluentCliOptionsBuilder<TOptions> AddOption(string shortName, string longName, bool required, Action<TOptions, string> assign) =>
            AddOption(new FluentCliOption
            {
                ShortName = shortName,
                LongName = longName,
                Required = required,
                Assign = (x, y) => assign(x, y),
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

        public FluentCliOptions Build() => new()
        {
            Model = _model,
            Options = _options
        };
    }

    public interface IFluentCliOptionsBuilder<TOptions>
    {
        IFluentCliOptionsBuilder<TOptions> AutoMap();
        IFluentCliOptionsBuilder<TOptions> AddOption(string shortName, string longName, bool required, Action<TOptions, string> assign);
        IFluentCliOptionsBuilder<TOptions> AddOption(FluentCliOption option);
        FluentCliOptions Build();
    }
}
