using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands
{
    public class UserInfoCmd : ModuleBase
    {
        private const string FULL_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private const string RECENT_DATETIME_FORMAT = "HH:mm:ss";
        private Logger _logger { get; } = Logger.GetLogger(typeof(ModuleBase));


        [Command("userinfo")]
        [RequireContext(ContextType.Guild, ErrorMessage = "(Guild) Usage: !userinfo [userID]")]
        public async Task UserInfo(SocketGuildUser iUser = null)
        {
            SocketGuildUser user = iUser;
            if (null == user)
            {
                user = Context.User as SocketGuildUser;
            }

            string replyTxt = null;
            var embed = new EmbedBuilder();

            if (user.IsBot)
            {
                replyTxt = "Bots are not human.";
            }
            else
            {
                // embed.WithColor(Color.Red);
                embed.WithAuthor(x =>
                {
                    x.Name = user.Username;
                    x.IconUrl = user.GetAvatarUrl();
                });
                embed.AddField("User", $"{user.Username}#{user.Discriminator}", true);
                embed.AddField("Member ID", user.Id, true);
                embed.AddField("Status", user.Status, true);
                embed.AddField("Joined Guild", user.JoinedAt.Value.ToString(FULL_DATETIME_FORMAT), true);
                embed.AddField("Account Created", user.CreatedAt.ToString(FULL_DATETIME_FORMAT), true);
                embed.AddField("Roles", user.Roles.Count - 1, true);
                embed.WithTimestamp(DateTimeOffset.Now);
                // embed.WithThumbnailUrl(user.GetAvatarUrl());
                embed.WithFooter(x =>
                {
                    x.Text = $"Requested By {Context.User.Username}";
                    x.IconUrl = Context.User.GetAvatarUrl();
                });

                if (null == user.Activity)
                {
                    embed.AddField("Activity", "None", true);
                }
                else
                {
                    if (user.Activity is SpotifyGame spot)
                    {
                        // embed.WithColor(Color.Green);
                        embed.AddField("Listening To", "Spotify", true);
                        embed.AddField("Track", spot.TrackTitle, true);
                        embed.AddField("Artist(s)", string.Join(", ", spot.Artists), true);
                        embed.AddField("Album", spot.AlbumTitle, true);
                        // embed.WithThumbnailUrl(spot.AlbumArtUrl);
                    }
                    // why not found.
                    // else if (user.Activity is CustomStatusGame statusGame)
                    // {
                    //     embed.AddField("Activity", statusGame.Name, true)
                    //         embed.AddField("Details", statusGame.Details, true);
                    //     embed.AddField("Playing Since", statusGame.CreatedAt, true);
                    //     embed.WithColor(Color.Magenta);
                    // }
                    else if (user.Activity is RichGame richGame)
                    {
                        // embed.WithColor(Color.Gold);
                        embed.AddField("Activity", richGame.Name, true);
                        embed.AddField("Details", richGame.Details, true);
                        embed.AddField("Playing Since", richGame.Timestamps.Start.Value.ToString(RECENT_DATETIME_FORMAT), true);
                        embed.AddField("Time Playing", (DateTimeOffset.Now - richGame.Timestamps.Start).Value.ToString(RECENT_DATETIME_FORMAT), true);
                        // embed.WithThumbnailUrl(richGame.SmallAsset.GetImageUrl() ?? user.GetAvatarUrl());
                    }
                    else
                    {
                        object activity = "None";
                        if (null != user.Activity.Name)
                        {
                            activity = user.Activity.Name;
                        }
                        embed.AddField("Activity", activity, true);
                    }
                }
            }
            await ReplyAsync(replyTxt, embed: embed.Build());
        }
    }
}
