using System;

namespace PokemonPocket
{
    public class Pokemon
    {
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string EvolveTo { get; set; }
        public int NoToEvolve { get; set; }
        public int Hp { get; set; }
        public int Exp { get; set; }
        public string Skill { get; set; }
        public int Damage { get; set; }
    }
}
