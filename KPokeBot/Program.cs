using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;

namespace KPokeBot {
    public class Program {

        public static bool DEBUG = true;

        public static DiscordClient discord;
        public static CommandsNextModule commands;
        public static InteractivityModule interactivity;

        static void Main(string[] args) {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(String[] args) {

            Pokedex.Init();
            Trainer.Init();

            // Set up the client connection
            if (DEBUG) {
                discord = new DiscordClient(new DiscordConfig {
                    Token = "MzUxNDI1Njc1NDc2ODYwOTM4.DISaLg.U10iSWt9_c5QOClpPxY9wPz5QlY",
                    TokenType = TokenType.Bot,
                    UseInternalLogHandler = true,
                    LogLevel = LogLevel.Debug
                });
            } else {
                discord = new DiscordClient(new DiscordConfig {
                    Token = "MzUwODU5NTcxNDY5ODc3MjU4.DIKLBg.5BXCMFiPYKUNuFkp_wwLB8f2IeY",
                    TokenType = TokenType.Bot,
                    UseInternalLogHandler = true,
                    LogLevel = LogLevel.Debug
                });
            }

            interactivity = discord.UseInteractivity();

            // Set up the command handler
            if (DEBUG) {
                commands = discord.UseCommandsNext(new CommandsNextConfiguration {
                    StringPrefix = "kp."
                });
                commands.RegisterCommands<CommandManager>();
            } else {
                commands = discord.UseCommandsNext(new CommandsNextConfiguration {
                    StringPrefix = "pk."
                });
                commands.RegisterCommands<CommandManager>();
            }

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
