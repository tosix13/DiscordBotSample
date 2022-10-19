using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands
{
    public class QuestionCmd : ModuleBase
    {
        [Command("question")]
        [RequireContext(ContextType.DM | ContextType.Guild, ErrorMessage = "(DM | Guild) Usage: !question [title] [items..]")]
        public async Task Question(params string[] iArgs)
        {
            if (iArgs.Length < 2)
            {
                throw new Exception("Invalid argument.");
            }

            string title = iArgs[0];
            string[] items = iArgs[1..];

            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithAuthor(Context.User.Username, Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl());

            Emoji icon = null;
            var descriptions = new StringBuilder();
            IEmote[] emotes = new IEmote[items.Length];
            for (int ith = 0; ith < items.Length; ith++)
            {
                icon = new Emoji(Common.ALPHABET_ICONS[ith]);
                descriptions.AppendLine($"{icon}\t{items[ith]}");
                emotes[ith] = icon;
            }

            embed.WithDescription(descriptions.ToString());
            await Context.Channel.SendMessageAsync(embed: embed.Build()).GetAwaiter().GetResult().AddReactionsAsync(emotes);
        }
    }
}
