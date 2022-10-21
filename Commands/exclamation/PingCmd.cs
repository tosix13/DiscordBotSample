using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands.exclamation
{
    [Group("tox")]
    public class PingCmd : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// pingの発言があった場合、pongを返します
        /// </summary>
        /// <returns></returns>
        [Command("ping")]
        [Summary("return pong.")]
        public async Task CommandAction()
        {
            await ReplyAsync("pong");
        }
    }
}
