using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPokeBot {
    public class Pokedex {

        public static Dictionary<int, string> Pokemon;

        public static void Init() {
            Pokemon = new Dictionary<int, string>();

            foreach (string line in System.IO.File.ReadLines("Pokedex.txt")) {
                string[] split = line.Split(',');
                Pokemon.Add(int.Parse(split[0]), split[1]);
            }

        }

    }
}
