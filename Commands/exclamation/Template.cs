using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands.exclamation
{
    [Group("tox")]
    public class TemplateCmd : ModuleBase<SocketCommandContext>
    {
        private Logger _logger { get; } = Logger.GetLogger(typeof(TemplateCmd));

        [Command("template")]
        [Summary("return none")]
        [RequireContext(ContextType.DM | ContextType.Guild, ErrorMessage = "(DM | Guild) Usage: !template [arg1]")]
        public async Task CommandAction(params string[] iArgs)
        {
            await Task.CompletedTask;
        }
    }
}
