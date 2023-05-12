using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonPocket
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Pokemon> Pokemons { get; set; }

        public Trainer()
        {
            //Name = name;
            Pokemons = new List<Pokemon>();
        }
    }
}
