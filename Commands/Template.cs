using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands
{
    public class TemplateCmd : ModuleBase
    {
        private Logger _logger { get; } = Logger.GetLogger(typeof(ModuleBase));

        [Command("template")]
        [RequireContext(ContextType.DM | ContextType.Guild, ErrorMessage = "(DM | Guild) Usage: !template [arg1]")]
        public async Task Template(params string[] iArgs)
        {
            await Task.CompletedTask;
        }
    }
}
