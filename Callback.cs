using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DiscordBot.NetCore.Commands.slash;

namespace DiscordBot.NetCore
{
    public class Callback
    {
        public enum SlashCommandRegion
        {
            global,
            guild
        }
        private static Logger _logger { get; } = Logger.GetLogger(typeof(Callback));
        private static IServiceProvider _provider { get; set; }
        private static DiscordSocketClient _client { get; set; }
        private static CommandService _commands { get; set; }
        public static List<ISlashCommand> CommandList = new List<ISlashCommand>();

        private Callback() { }

        public static void SetCallbacks(IServiceProvider iProvider, CommandService iCommands, DiscordSocketClient iClient)
        {
            _provider = iProvider;
            _client = iClient;
            _commands = iCommands;

            _client.Log += _Log;
            _client.MessageReceived += _MessageRecieved;
            _client.Ready += _Ready;
            _client.SlashCommandExecuted += _SlashCommandExecuted;
            _client.UserJoined += _UserJoined;
            // _client.UserLeft += _UserLeft;
        }

        private static Task _Log(LogMessage msg)
        {
            _logger.WriteLog(LogLevel.DEBUG, msg.ToString());
            return Task.CompletedTask;
        }

        public static async Task _Ready()
        {
            // 上限があるため廃止
            // var guild = _client.GetGuild(EntryPoint._config.guildID);
            // await SetSlashCommandCallback(guild);

            await Task.CompletedTask;
        }

        private static async Task _MessageRecieved(SocketMessage iMsgParam)
        {
            var msg = iMsgParam as SocketUserMessage;
            if (null == msg)
            {
                return;
            }
            _logger.WriteLog(LogLevel.TRACE, $"{msg.Channel.Name} {msg.Author.Username}: {msg}");

            int argPos = 0;

            // ユーザーからのコマンドメッセージを処理
            if (!msg.Author.IsBot)
            {
                // !コマンドの確認・実行
                if (isExclamationCommand(msg, ref argPos))
                {
                    await ExcecuteExclamationCommand(msg, argPos);
                }

                // /コマンドの場合はSlashCommandExecutedからコールバック
            }
        }

        private static async Task _SlashCommandExecuted(SocketSlashCommand iCommand)
        {
            string cmdName = iCommand.Data.Name;
            _logger.WriteLog(LogLevel.TRACE, $"Executed command: {cmdName}");

            ISlashCommand targetCmd = null;
            foreach (var cmd in CommandList)
            {
                if (cmd._cmdName == cmdName)
                {
                    targetCmd = cmd;
                    break;
                }
            }

            if (null == targetCmd)
            {
                throw new Exception("そんなコマンドないよ");
            }

            await targetCmd.CommandAction(iCommand);
        }


        /// <summary>
        /// ユーザーが会議に参加したときの処理
        /// </summary>
        /// <param name="iUser"></param>
        /// <returns></returns>
        private static async Task _UserJoined(SocketGuildUser iUser)
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
        private static async Task _UserLeft(SocketGuildUser iUser)
        {
            var chatchannnel = _client.GetChannel(EntryPoint._config.channelID) as SocketTextChannel;
            string msg = string.Format($"{iUser.Username}様さようなら、またのお越しをお待ちしております");
            await chatchannnel.SendMessageAsync(msg);
        }


        private static void SetCommandList()
        {
            _logger.WriteLog(LogLevel.TRACE, $"Start Method: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            // CommandList.Add(new PingCmd());
        }

        private static async Task SetSlashCommandCallback(SocketGuild iGuild)
        {
            SetCommandList();

            SlashCommandBuilder builtCmd;
            foreach (var cmd in CommandList)
            {
                builtCmd = cmd.CommandBuild();
                try
                {
                    switch (cmd._region)
                    {
                        case SlashCommandRegion.global:
                            await _client.CreateGlobalApplicationCommandAsync(builtCmd.Build());
                            break;
                        case SlashCommandRegion.guild:
                            await iGuild.CreateApplicationCommandAsync(builtCmd.Build());
                            break;
                        default:
                            break;
                    }
                }
                catch (HttpException exception)
                {
                    _logger.WriteLog(LogLevel.ERROR, JsonConvert.SerializeObject(exception.Errors, Formatting.Indented));
                }
            }
        }

        private static async Task ExcecuteExclamationCommand(SocketUserMessage iMsg, int iArgpos)
        {
            var context = new SocketCommandContext(_client, iMsg);
            try
            {
                var result = await _commands.ExecuteAsync(context, iArgpos, services: null);
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

        public static bool isExclamationCommand(SocketUserMessage iMsg, ref int iArgpos)
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
