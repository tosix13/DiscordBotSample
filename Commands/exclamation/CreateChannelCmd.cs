using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands.exclamation
{
    [Group("tox")]
    public class CreateChannelCmd : ModuleBase<SocketCommandContext>
    {
        private Logger _logger { get; } = Logger.GetLogger(typeof(ModuleBase));

        [Command("createchannel")]
        [Summary("create voice chat and text")]
        [RequireContext(ContextType.Guild, ErrorMessage = "(Guild) Usage: !createchannel [title] [isCreateTextChannel true|false]")]
        public async Task CommandAction(string title, string isCreateTextChannel = "true")
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception("Invalid argument.");
            }

            var newVoiceChannel = await Context.Guild.CreateVoiceChannelAsync($"{title}_通話");
            await newVoiceChannel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, OverwritePermissions.AllowAll(newVoiceChannel));

            bool toCreateTextChannel;
            try
            {
                toCreateTextChannel = System.Convert.ToBoolean(isCreateTextChannel);
            }
            catch
            {
                throw new Exception("Invalid argument.");
            }

            if (toCreateTextChannel)
            {
                var newTextChannel = await Context.Guild.CreateTextChannelAsync($"{title}_聞き専");
                await newTextChannel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, OverwritePermissions.AllowAll(newVoiceChannel));
            }
            await Task.CompletedTask;
        }
    }
}
