using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands.exclamation
{
    [Group("tox")]
    public class ClearTextLog : ModuleBase<SocketCommandContext>
    {
        private Logger _logger { get; } = Logger.GetLogger(typeof(ClearTextLog));

        [Command("cleartextlogs")]
        [Summary("return none")]
        [RequireContext(ContextType.Guild)]
        public async Task CommandAction(params string[] iArgs)
        {
            // var msg = Context.Channel.GetMessagesAsync((int)100 + 1).Flatten();
            var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            // await Task.CompletedTask;
        }
    }
}
