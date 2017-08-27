using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace KPokeBot {
    public class Program {

        static DiscordClient discord;
        static CommandsNextModule commands;

        static void Main(string[] args) {
            System.Diagnostics.Debug.WriteLine(System.IO.Directory.GetCurrentDirectory().ToString());
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(String[] args) {

            Pokedex.Init();
            Trainer.Init();

            // Set up the client connection
            discord = new DiscordClient(new DiscordConfig {
                Token = "MzUwODU5NTcxNDY5ODc3MjU4.DIKLBg.5BXCMFiPYKUNuFkp_wwLB8f2IeY",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            // Set up the command handler
            commands = discord.UseCommandsNext(new CommandsNextConfiguration {
                StringPrefix = "pk."
            });
            commands.RegisterCommands<CommandManager>();

            //discord.MessageCreated += async e =>
            //{
            //    if (e.Message.Content.ToLower().StartsWith("ping"))
            //        await e.Message.RespondAsync("pong!");
            //};

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
