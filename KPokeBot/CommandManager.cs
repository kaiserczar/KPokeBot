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

        private static Random rand = new Random();

        [Command("catch")]
        public async Task Catch(CommandContext cc) {

            // Limited to original 151, not to NumberOfPokemon.
            int pokeNum;
            string pName;

            //PokemonSpecies p = await DataFetcher.GetApiObject<PokemonSpecies>(pokeNum);

            while(!Pokedex.GetNewPokemon(rand, out pokeNum)) {
                pName = Pokedex.Pokemon[pokeNum];
                await cc.RespondAsync($"You almost caught a " + Tools.CapFirst(pName) + " but it got away because you suck!");
            }
            pName = Pokedex.Pokemon[pokeNum];

            Trainer t = Trainer.GetActiveTrainer(cc.User.Id);
            t.AddPokemon(pokeNum);

            Uri url = new Uri("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + pokeNum.ToString() + ".png");
            await cc.RespondAsync($"{cc.User.Mention} has caught a **" + Tools.CapFirst(pName) + "**!\r\n" + url.ToString() + "\r\nYou now have " + t.NumOwned + " pokemon.");

            Trainer.SaveAll();
        }

        [Command("list")]
        public async Task List(CommandContext cc) {
            StringBuilder sb = new StringBuilder();
            Trainer t = Trainer.GetActiveTrainer(cc.User.Id);
            sb.AppendLine("**" + cc.User.Username + "'s Pokemon:** Captured **" + t.NumOwned + "** Pokemon.");
            foreach (int i in t.pokemonInventory.Keys) {
                sb.AppendLine(Pokedex.Pokemon[i] + " (" + i.ToString() + ") x" + t.pokemonInventory[i].ToString());
            }

            await cc.RespondAsync(sb.ToString());
        }
    }
}
