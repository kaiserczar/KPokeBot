using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPokeBot {
    public class Pokedex {

        public static int NumberOfPokemon = 151; // 802 real
        public static Dictionary<int, string> Pokemon;
        public static List<int> Legendaries = new List<int>(5) { 144, 145, 146, 150, 151 };

        public static void Init() {
            Pokemon = new Dictionary<int, string>();

            foreach (string line in System.IO.File.ReadLines("Pokedex.txt")) {
                string[] split = line.Split(',');
                Pokemon.Add(int.Parse(split[0]), split[1]);
            }

        }

        // Returns whether it was a success or failure to catch.
        public static bool GetNewPokemon(Random r, out int pokeNum) {
            pokeNum = r.Next(NumberOfPokemon) + 1;
            bool retVal = true;

            foreach (int i in Legendaries) {
                if (pokeNum == i & r.Next(4) != 0) {
                    retVal = false;
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
