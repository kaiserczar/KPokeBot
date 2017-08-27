using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPokeBot {
    public class Pokedex {

        public static Dictionary<int, string> Pokemon;
        public static List<int> Legendaries = new List<int>(5) { 144, 145, 146, 150, 151 };

        public static void Init() {
            Pokemon = new Dictionary<int, string>();

            foreach (string line in System.IO.File.ReadLines("Pokedex.txt")) {
                string[] split = line.Split(',');
                Pokemon.Add(int.Parse(split[0]), split[1]);
            }

        }

        public static int GetNewPokemon(Random r) {
            int retVal = r.Next(151) + 1;

            foreach (int i in Legendaries) {
                if (retVal == i & r.Next(4) == 0) {
                    retVal = GetNewPokemon(r);
                }
            }

            return retVal;
        }

    }
}
