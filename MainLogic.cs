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

        private IServiceProvider _provider { get; set; }
        private CommandService _commands { get; set; }
        private DiscordSocketClient _client { get; set; }

        public async Task MainAsync()
        {
            _logger.WriteLog(LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");

            // ServiceProviderインスタンス生成
            // _provider = new ServiceCollection().BuildServiceProvider();

            _provider = new ServiceCollection().AddSingleton(new AudioService()).BuildServiceProvider();

            // コマンド追加
            _commands = new CommandService();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);


            // client生成
            _client = new DiscordSocketClient();

            // コールバックをセット
            _client.MessageReceived += MessageRecieved;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
            _client.Log += Log;

            // Botにログイン
            await _client.LoginAsync(TokenType.Bot, EntryPoint._config.token);

            // 常駐処理開始
            await _client.StartAsync();

            // タスクを常駐
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            _logger.WriteLog(LogLevel.DEBUG, msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageRecieved(SocketMessage iMsgParam)
        {
            _logger.WriteLog(LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");

            var message = iMsgParam as SocketUserMessage;
            if (null == message)
            {
                return;
            }
            _logger.WriteLog(LogLevel.TRACE, $"{message.Channel.Name} {message.Author.Username}: {message}");

            int argPos = 0;

            // ユーザーからのコマンドメッセージを処理
            if (!message.Author.IsBot && isCommand(message, ref argPos))
            {
                var context = new CommandContext(_client, message);
                try
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _provider);
                    if (!result.IsSuccess)
                    {
                        await context.Channel.SendMessageAsync(result.ErrorReason);
                    }
                }
                catch (Exception e)
                {
                    _logger.WriteLog(LogLevel.ERROR, e.ToString());
                }
            }
        }

        /// <summary>
        /// ユーザーが会議に参加したときの処理
        /// </summary>
        /// <param name="iUser"></param>
        /// <returns></returns>
        private async Task UserJoined(SocketGuildUser iUser)
        {
            var chatchannnel = _client.GetChannel(EntryPoint._config.channelID) as SocketTextChannel;
            string msg = string.Format($"{iUser.Username}様、ようこそ！");
            await chatchannnel.SendMessageAsync(msg);
        }

        /// <summary>
        /// ユーザーが会議から抜けたときの処理
        /// </summary>
        /// <param name="iUser"></param>
        /// <returns></returns>
        private async Task UserLeft(SocketGuildUser iUser)
        {
            var chatchannnel = _client.GetChannel(EntryPoint._config.channelID) as SocketTextChannel;
            string msg = string.Format($"{iUser.Username}様さようなら、またのお越しをお待ちしております");
            await chatchannnel.SendMessageAsync(msg);
        }

        public bool isCommand(SocketUserMessage iMsg, ref int iArgpos)
        {
            bool retStatus = false;

            // !つきまたはbotへのメンション
            if ((iMsg.HasCharPrefix('!', ref iArgpos) || iMsg.HasMentionPrefix(_client.CurrentUser, ref iArgpos)))
            {
                retStatus = true;
            }
            return retStatus;
        }
    }
}
