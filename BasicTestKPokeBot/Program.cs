using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPokeBot;

namespace BasicTestKPokeBot {
    class Program {

        static int numRuns = 1000000;

        static void Main(string[] args) {
            DateTime start = DateTime.Now;
            Console.WriteLine("Started at " + start.ToShortTimeString());

            Random r = new Random(DateTime.Now.Millisecond);
            int legendariesSeen = 0;
            int legendariesCaught = 0;

            for (int i = 0; i<numRuns; i++) {
                bool caught = Pokedex.GetNewPokemon(r, out int pokeNum, updateConfig: false);
                if (Pokedex.IsLegendary(pokeNum)) {
                    legendariesSeen++;
                    if (caught) {
                        legendariesCaught++;
                    }
                }
            }

            Console.WriteLine("In " + numRuns + " pokemon... Results:");
            Console.WriteLine("     " + legendariesSeen + " legendaries seen (" + ((double)legendariesSeen / numRuns)*100 + "%); optimal 3.311%.");
            Console.WriteLine("     " + legendariesCaught + " legendaries caught (" + ((double)legendariesCaught / legendariesSeen) * 100 + "%); optimal 25.0%.");

            DateTime finish = DateTime.Now;
            Console.WriteLine("Finished at " + finish.ToShortTimeString());
            Console.WriteLine("Duration: " + finish.Subtract(start).TotalSeconds + " s");
            Console.ReadLine();
        }
    }
}
