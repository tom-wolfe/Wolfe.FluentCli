﻿using System.Threading.Tasks;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Models;

namespace Wolfe.FluentCli.Commands
{
    internal class NullCommand : ICommandHandler
    {
        public Task Execute(CliContext context) => Task.CompletedTask;
    }
}