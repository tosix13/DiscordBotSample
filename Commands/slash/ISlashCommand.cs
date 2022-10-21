using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands.slash
{
    public interface ISlashCommand
    {
        public string _cmdName { get; set; }
        public Callback.SlashCommandRegion _region { get; set; }
        SlashCommandBuilder CommandBuild();
        Task CommandAction(SocketSlashCommand iCommand);
    }
}
