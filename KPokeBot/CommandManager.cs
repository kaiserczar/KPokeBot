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

        [Command("catch"), Description("Try to catch a new pokemon.")]
        public async Task Catch(CommandContext cc) {
            // Get the person who's catching.
            Trainer t = Trainer.GetActiveTrainer(cc.User.Id);

            //if (false) { 
            if (!Program.DEBUG & DateTime.Now.Subtract(t.lastCatch).TotalHours < 3) {
                await cc.RespondAsync($"Stop trying to catch too early you moron. \r\nLast catch at: " + t.lastCatch.ToString());
            } else {
                // Limited to original 151, not to NumberOfPokemon.
                int pokeNum;
                string pName;

                //PokemonSpecies p = await DataFetcher.GetApiObject<PokemonSpecies>(pokeNum);

                while (!Pokedex.GetNewPokemon(rand, out pokeNum)) {
                    pName = Pokedex.Pokemon[pokeNum];
                    await cc.RespondAsync($"You almost caught a " + Tools.CapFirst(pName) + " but it got away because you suck!");
                }
                pName = Pokedex.Pokemon[pokeNum];

                t.AddPokemon(pokeNum);

                Uri url = new Uri("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + pokeNum.ToString() + ".png");
                await cc.RespondAsync($"{cc.User.Mention} has caught a **" + Tools.CapFirst(pName) + "**! \r\nYou now have " + t.NumOwned + " pokemon.\r\nLast catch at: " + t.lastCatch.ToString() + "\r\n" + url.ToString());

                Trainer.SaveAll();
            }
        }

        [Command("list"), Description("List all the pokemon of another person.")]
        public async Task List(CommandContext cc) {
            StringBuilder sb = new StringBuilder();
            Trainer t = Trainer.GetActiveTrainer(cc.User.Id);
            sb.AppendLine("**" + cc.User.Username + "'s Pokemon:** Captured **" + t.NumOwned + "** Pokemon.");
            foreach (int i in t.pokemonInventory.Keys) {
                sb.AppendLine(Pokedex.Pokemon[i] + " (" + i.ToString() + ") x" + t.pokemonInventory[i].ToString().PadRight(100,' '));
            }

            var pages = Program.interactivity.GeneratePagesInEmbeds(sb.ToString());

            //await cc.RespondAsync(sb.ToString());
            await Program.interactivity.SendPaginatedMessage(cc.Channel, cc.User, pages, TimeSpan.FromMinutes(5), DSharpPlus.Interactivity.TimeoutBehaviour.Delete);
        }

        //[Command("list"), Description("List all the pokemon of another person.")]
        //public async Task List(CommandContext cc,[Description("Whose pokedex you want to look at.")] string username) {
        //    StringBuilder sb = new StringBuilder();
        //    Trainer t = Trainer.GetActiveTrainer(cc.User.Id);
        //    sb.AppendLine("**" + cc.User.Username + "'s Pokemon:** Captured **" + t.NumOwned + "** Pokemon.");
        //    foreach (int i in t.pokemonInventory.Keys) {
        //        sb.AppendLine(Pokedex.Pokemon[i] + " (" + i.ToString() + ") x" + t.pokemonInventory[i].ToString());
        //    }

        //    await cc.RespondAsync(sb.ToString());
        //}


    }
}
