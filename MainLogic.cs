using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.NetCore
{
    public class MainLogic
    {
        private Logger _logger { get; } = Logger.GetLogger(typeof(MainLogic));

        private static IServiceProvider _provider { get; set; }
        private static CommandService _commands { get; set; }
        private static DiscordSocketClient _client { get; set; }

        public async Task MainAsync()
        {
            // ServiceProviderインスタンス生成
            _provider = CreateProvider();

            // コマンド生成
            _commands = new CommandService();

            // client生成
            _client = new DiscordSocketClient();

            // コールバックをセット
            Callback.SetCallbacks(_provider, _commands, _client);


            // コマンド追加
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);

            // Botにログイン
            await _client.LoginAsync(TokenType.Bot, EntryPoint._config.token);

            // 常駐処理開始
            await _client.StartAsync();

            // タスクを常駐
            await Task.Delay(-1);
        }

        static IServiceProvider CreateProvider()
        {
            var collection = new ServiceCollection();
            return collection.BuildServiceProvider();
        }
    }
}
