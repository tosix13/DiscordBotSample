using Discord;
using Discord.Commands;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text.Json;

namespace DiscordBot.NetCore.Commands
{
    public class Recipe
    {
        public string foodImageUrl { get; set; }
        public string mediumImageUrl { get; set; }
        public string nickname { get; set; }
        public int pickup { get; set; }
        public string rank { get; set; }
        public string recipeCost { get; set; }
        public string recipeDescription { get; set; }
        public int recipeId { get; set; }
        public string recipeIndication { get; set; }
        public List<string> recipeMaterial { get; set; }
        public string recipePublishday { get; set; }
        public string recipeTitle { get; set; }
        public string recipeUrl { get; set; }
        public int shop { get; set; }
        public string smallImageUrl { get; set; }
    }

    public class Response
    {
        public List<Recipe> result { get; set; }
    }

    public class DinnerCmd : ModuleBase
    {
        private string RAKUTEN_API_PATH = $"https://app.rakuten.co.jp/services/api/Recipe/CategoryRanking/20170426?format=json&applicationId={EntryPoint._config.rakuten_applicationID}";

        private Logger _logger { get; } = Logger.GetLogger(typeof(ModuleBase));

        [Command("dinner")]
        [RequireContext(ContextType.DM | ContextType.Guild, ErrorMessage = "(DM | Guild) Usage: !dinner")]
        public async Task Dinner(params string[] iArgs)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetFromJsonAsync<Response>(RAKUTEN_API_PATH);
            foreach (var recipe in response.result)
            {
                await SendEmbed(recipe);
            }

            await Task.CompletedTask;
        }

        private async Task SendEmbed(Recipe iRacipe)
        {
            var embed = new EmbedBuilder();
            embed.AddField("レシピ名", $"{iRacipe.recipeTitle}", true);
            embed.AddField("URL", $"{iRacipe.recipeUrl}", true);
            embed.AddField("予算", $"{iRacipe.recipeCost}", true);
            embed.WithThumbnailUrl(iRacipe.smallImageUrl);
            await ReplyAsync(embed: embed.Build());
        }
    }
}
