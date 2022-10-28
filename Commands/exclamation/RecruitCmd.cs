using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands.exclamation
{
    [Group("tox")]
    public class RecruitCmd : ModuleBase<SocketCommandContext>
    {
        [Command("recruit")]
        [Summary("create reqruit list")]
        [RequireContext(ContextType.Guild, ErrorMessage = "(Guild) Usage: !reqruit [title] [memberCnt]")]
        public async Task CommandAction(params string[] iArgs)
        {
            if (iArgs.Length < 2)
            {
                throw new Exception("Invalid argument.");
            }

            SocketGuildUser user = Context.User as SocketGuildUser;
            string title = iArgs[0];
            string memberCnt = iArgs[1];

            var embed = new EmbedBuilder();
            embed.WithAuthor(x =>
            {
                x.Name = user.Username;
                x.IconUrl = user.GetAvatarUrl();
            });
            embed.WithTitle($"{title} ({memberCnt}人)");
            embed.WithCurrentTimestamp();
            embed.WithAuthor(Context.User.Username, Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl());
            embed.Description = "";

            var emotes = new IEmote[] {
                new Emoji("\u2B06"),
                new Emoji("\u21A9"),
                new Emoji("\u26D4")
            };

            var message = await Context.Channel.SendMessageAsync(null, true, embed: embed.Build());
            await message.AddReactionsAsync(emotes);

            embed.Description += "\nAAAAAAAA";
            embed.Description += $"\n{Context.User.Mention}";

            await message.ModifyAsync(msg =>
            {
                msg.Embed = embed.Build();
            });
        }
    }
}
