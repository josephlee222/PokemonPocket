using System;

namespace PokemonPocket
{
    public class PokemonMaster
    {
        public string Name { get; set; }
        public int NoToEvolve { get; set; }
        public string EvolveTo { get; set; }

        public PokemonMaster(string name, int noToEvolve, string evolveTo)
        {
            this.Name = name;
            this.NoToEvolve = noToEvolve;
            this.EvolveTo = evolveTo;
        }
    }

    public class Pokemon
    {
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Exp { get; set; }
        public string Skill { get; set; }
        public int Damage { get; set; }
        public virtual Trainer Trainer { get; set; }

        public Pokemon(int hp, int exp)
        { 
            this.Hp = hp;
            this.Exp = exp;
        }
    }
}
