using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPokeBot {
    public class Pokedex {

        public static int NumberOfPokemon = 251; // 802 real
        public static Dictionary<int, string> Pokemon;
        public static List<int> Legendaries = new List<int>(5) { 144, 145, 146, 150, 151, 243, 244, 245, 249, 250, 251 };

        public static void Init() {
            Pokemon = new Dictionary<int, string>();

            foreach (string line in System.IO.File.ReadLines("Pokedex.txt")) {
                string[] split = line.Split(',');
                Pokemon.Add(int.Parse(split[0]), split[1]);
            }

        }

        // Returns whether it was a success or failure to catch.
        public static bool GetNewPokemon(Random r, out int pokeNum, bool updateConfig = true) {
            pokeNum = r.Next(NumberOfPokemon) + 1;
            bool retVal = true;

            foreach (int i in Legendaries) {
                if (pokeNum == i) {
                    if (updateConfig) Program.jConfig.NumLegendariesSeen++;
                    if (r.Next(4) != 0) {
                        retVal = false;
                    } else {
                        if (updateConfig) Program.jConfig.NumLegendariesCaught++;
                    }
                }
            }

            return retVal;
        }

        public static bool IsLegendary(int num) {
            foreach (int i in Legendaries) {
                if (num == i) {
                    return true;
                }
            }

            return false;
        }

    }
}
