using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;

namespace KPokeBot {
    public class Program {

        public static bool DEBUG;

        public static DiscordClient discord;
        public static CommandsNextModule commands;
        public static InteractivityModule interactivity;

        static void Main(string[] args) {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(String[] args) {

            // Load config
            string json = "";
            using (var fs = System.IO.File.OpenRead("config.json"))
            using (var sr = new System.IO.StreamReader(fs, new System.Text.UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            ConfigJson jConfig = JsonConvert.DeserializeObject<ConfigJson>(json);

            DEBUG = jConfig.DEBUG;

            Pokedex.Init();
            Trainer.Init();

            DiscordConfig dConfig;

            // Set up the client connection
            if (DEBUG) {
                dConfig = new DiscordConfig {
                    Token = jConfig.TokenTest,
                    TokenType = TokenType.Bot,
                    UseInternalLogHandler = true,
                    LogLevel = LogLevel.Debug
                };
            } else {
                dConfig = new DiscordConfig {
                    Token = jConfig.TokenReal,
                    TokenType = TokenType.Bot,
                    UseInternalLogHandler = true,
                    LogLevel = LogLevel.Debug
                };
            }
            discord = new DiscordClient(dConfig);

            interactivity = discord.UseInteractivity();

            // Set up the command handler
            if (DEBUG) {
                commands = discord.UseCommandsNext(new CommandsNextConfiguration {
                    StringPrefix = jConfig.PrefixTest + "."
                });
                commands.RegisterCommands<CommandManager>();
            } else {
                commands = discord.UseCommandsNext(new CommandsNextConfiguration {
                    StringPrefix = jConfig.PrefixReal + "."
                });
                commands.RegisterCommands<CommandManager>();
            }

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        public struct ConfigJson {

            [JsonProperty("DEBUG")]
            public bool DEBUG { get; private set; }

            [JsonProperty("tokenReal")]
            public string TokenReal { get; private set; }

            [JsonProperty("tokenTest")]
            public string TokenTest { get; private set; }

            [JsonProperty("prefixReal")]
            public string PrefixReal { get; private set; }

            [JsonProperty("prefixTest")]
            public string PrefixTest { get; private set; }
        }
    }
}
