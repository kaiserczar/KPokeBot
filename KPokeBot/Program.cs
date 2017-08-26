using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace KPokeBot {
    class Program {

        static DiscordClient discord;

        static void Main(string[] args) {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(String[] args) {
            discord = new DiscordClient(new DiscordConfig {
                Token = "MzUwODU5NTcxNDY5ODc3MjU4.DIKLBg.5BXCMFiPYKUNuFkp_wwLB8f2IeY",
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("pong!");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
