using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PokeAPI;

namespace KPokeBot {
    public class CommandManager {

        public static int NumberOfPokemon = 802;
        private static Random rand = new Random();

        [Command("catch")]
        public async Task Catch(CommandContext cc) {

            int pokeNum = rand.Next(802) + 1;

            PokemonSpecies p = await DataFetcher.GetApiObject<PokemonSpecies>(pokeNum);
            
            await cc.RespondAsync($"{cc.User.Mention} has caught a **" + Tools.CapFirst(p.Name) + "**!");
            Uri url = new Uri("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + pokeNum.ToString() + ".png");
            await cc.RespondAsync(url.ToString());
        }

    }
}
