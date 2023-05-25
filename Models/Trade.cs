using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonPocket
{
    public class Trade
    {
        public int TradeId { get; set; }
        public int ToTrainer { get; set; }
        public int FromTrainer { get; set; }
        public int ToPokemon { get; set; }
        public int FromPokemon { get; set; }

        public Trade(int ToTrainer, int FromTrainer, int ToPokemon, int FromPokemon)
        {
            // Set values for the properties
            this.ToTrainer = ToTrainer;
            this.FromTrainer = FromTrainer;
            this.ToPokemon = ToPokemon;
            this.FromPokemon = FromPokemon;
        }
    }
}
