﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPokeBot {
    public class Trainer {

        public ulong uid;
        public SortedDictionary<int, int> pokemonInventory;
        public static List<Trainer> Trainers;
        public DateTime lastCatch;

        public Trainer(ulong uid) {
            this.uid = uid;
            pokemonInventory = new SortedDictionary<int, int>();
            lastCatch = DateTime.Now.Subtract(new TimeSpan(50, 0, 0));
        }

        public int NumOwned {
            get {
                int owned = 0;
                foreach (int i in pokemonInventory.Keys) {
                    owned += pokemonInventory[i];
                }
                return owned;
            }
        }

        public void AddPokemon(int pokeNum) {
            if (pokemonInventory.ContainsKey(pokeNum)) {
                pokemonInventory[pokeNum]++;
            } else {
                pokemonInventory.Add(pokeNum, 1);
            }
            lastCatch = DateTime.Now;
        }

        public static void SaveAll() {
            StringBuilder sb = new StringBuilder();
            foreach(Trainer t in Trainers) {
                sb.AppendLine(t.uid.ToString() + " " + t.lastCatch.ToBinary().ToString() + " " + Tools.IntDictToString(t.pokemonInventory));
            }
            System.IO.File.WriteAllText("savedData.txt", sb.ToString());
        }

        public static void LoadAll() {
            if (System.IO.File.Exists("savedData.txt")) {
                foreach (string line in System.IO.File.ReadLines("savedData.txt")) {
                    string[] parsed = line.Split(' ');
                    ulong uid = ulong.Parse(parsed[0]);
                    string lastCatchString = parsed[1];
                    SortedDictionary<int, int> pokes = Tools.StringToIntDict(parsed[2]);
                    Trainer t = new Trainer(uid);
                    t.pokemonInventory = pokes;
                    t.lastCatch = DateTime.FromBinary(long.Parse(lastCatchString));
                    Trainers.Add(t);
                }
            }
        }

        public static void Init() {
            Trainer.Trainers = new List<Trainer>();
            Trainer.LoadAll();
        }

        public static Trainer GetActiveTrainer(ulong uid) {
            foreach (Trainer t in Trainers) {
                if (t.uid == uid) return t;
            }
            Trainer tNew = new Trainer(uid);
            Trainers.Add(tNew);
            return tNew;
        }
    }
}
