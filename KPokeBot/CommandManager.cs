﻿using System;
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
                    Program.SaveConfig();
                }
                pName = Pokedex.Pokemon[pokeNum];
                if (Pokedex.IsLegendary(pokeNum)) Program.SaveConfig();

                t.AddPokemon(pokeNum);

                Uri url = new Uri("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + pokeNum.ToString() + ".png");
                await cc.RespondAsync($"{cc.User.Mention} has caught a **" + Tools.CapFirst(pName) + "**! \r\nYou now have " + t.pokemonInventory.Count + " unique pokemon and " + t.NumOwned + " total pokemon.\r\nLast catch at: " + t.lastCatch.ToString() + "\r\n" + url.ToString());

                Trainer.SaveAll();
            }
        }

        [Command("list"), Description("List all the pokemon you own.")]
        public async Task List(CommandContext cc) {
            StringBuilder sb = new StringBuilder();
            Trainer t = Trainer.GetActiveTrainer(cc.User.Id);
            sb.AppendLine("**" + cc.User.Username + "'s Pokemon:** Captured **" + t.pokemonInventory.Count + "** unique Pokemon and **" + t.NumOwned + "** total Pokemon.");
            foreach (int i in t.pokemonInventory.Keys) {
                if (Pokedex.IsLegendary(i)) {
                    sb.AppendLine("**" + Pokedex.Pokemon[i] + "** (" + i.ToString() + ") x" + t.pokemonInventory[i].ToString().PadRight(120, ' '));
                } else {
                    sb.AppendLine(Pokedex.Pokemon[i] + " (" + i.ToString() + ") x" + t.pokemonInventory[i].ToString().PadRight(120,' '));
                }
            }

            var pages = Program.interactivity.GeneratePagesInEmbeds(sb.ToString());

            //await cc.RespondAsync(sb.ToString());
            await Program.interactivity.SendPaginatedMessage(cc.Channel, cc.User, pages, TimeSpan.FromMinutes(5), DSharpPlus.Interactivity.TimeoutBehaviour.Delete);
        }

        [Command("legendary"), Description("Lists legendary sightings.")]
        public async Task Legendary(CommandContext cc) {
            await cc.RespondAsync("There have been " + Program.jConfig.NumLegendariesSeen + " legendary sightings and " + Program.jConfig.NumLegendariesCaught + " caught.");
        }


        private static DateTime lastGlobalDex = DateTime.Now.Subtract(new TimeSpan(2, 0, 0));
        [Command("globaldex"), Description("Lists the total pokemon caught in the universe.")]
        public async Task GlobalDex(CommandContext cc) {
            List<SortedDictionary<int, int>> lists = new List<SortedDictionary<int, int>>();
            foreach (Trainer t in Trainer.Trainers) {
                lists.Add(t.pokemonInventory);
            }
            SortedDictionary<int, int> totalPokemon = Tools.DictionaryCombine(lists);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("**Global Pokemon:** Captured **" + totalPokemon.Count + "** unique Pokemon and **" + totalPokemon.Values.Sum() + "** total Pokemon.");
            foreach (int i in totalPokemon.Keys) {
                if (Pokedex.IsLegendary(i)) {
                    sb.AppendLine("**" + Pokedex.Pokemon[i] + "** (" + i.ToString() + ") x" + totalPokemon[i].ToString().PadRight(120, ' '));
                } else {
                    sb.AppendLine(Pokedex.Pokemon[i] + " (" + i.ToString() + ") x" + totalPokemon[i].ToString().PadRight(120, ' '));
                }
            }

            var pages = Program.interactivity.GeneratePagesInEmbeds(sb.ToString());
            
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
