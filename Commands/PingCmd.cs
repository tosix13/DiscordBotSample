using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands
{
    public class PingCmd : ModuleBase
    {
        /// <summary>
        /// pingの発言があった場合、pongを返します
        /// </summary>
        /// <returns></returns>
        [Command("ping")]
        [RequireContext(ContextType.DM | ContextType.Guild, ErrorMessage = "(DM | Guild) Usage: !ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}
