using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands
{
    public class DiceCmd : ModuleBase
    {
        /// <summary>
        /// サイコロを振ります
        /// </summary>
        /// <param name="face">振るダイスの面の数</param>
        /// <param name="throwCount">ダイスを振る回数</param>
        /// <returns></returns>
        [Command("dice")]
        [RequireContext(ContextType.DM | ContextType.Guild, ErrorMessage = "(DM | Guild) Usage: !dice [face] [throwCount]\n  face: 1以上 (Default 6)\n  throwCount: 1以上 (Default 10)")]
        public async Task Dice(byte face = 6, byte throwCount = 10)
        {
            if (face < 1 || throwCount < 1)
            {
                throw new Exception("Invalid argument.");
            }

            var result = 0;
            List<int> results = new List<int>();
            for (int ith = 0; ith < throwCount; ith++)
            {
                result = new Random().Next(1, face + 1);
                results.Add(result);
            }
            int sum = results.Sum();
            string average = ((double)sum / throwCount).ToString("#,0.00");

            // embed枠生成
            var embed = new EmbedBuilder();
            embed.WithTitle("出目");
            embed.WithDescription(string.Join(", ", results));

            await ReplyAsync($"{face}面ダイスを{throwCount}回振ったよ!\n  合計: {sum}、平均値: {average}", embed: embed.Build());
        }
    }
}
