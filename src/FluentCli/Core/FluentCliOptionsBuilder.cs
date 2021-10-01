using System;
using FluentCli.Models;

namespace FluentCli.Core
{
    public class FluentCliOptionsBuilder : IFluentCliOptionsBuilder
    {
        private readonly Type _model;

        protected FluentCliOptionsBuilder(Type model)
        {
            _model = model;
        }

        public static FluentCliOptionsBuilder Create<THandler>() => new(typeof(THandler));

        public FluentCliOptions Build() => new()
        {
            Model = _model
        };
    }

    public interface IFluentCliOptionsBuilder
    {
        FluentCliOptions Build();
    }
}
