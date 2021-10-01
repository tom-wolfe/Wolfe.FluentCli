using System.Collections.Generic;

namespace FluentCli.Models
{
    public delegate TOptions OptionsMap<out TOptions>(Dictionary<string, string> options);
}
