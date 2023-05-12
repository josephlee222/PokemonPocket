using Microsoft.EntityFrameworkCore;


namespace PokemonPocket
{
    public class PokemonDataContext : DbContext
    {
        static readonly string connectionString = "Server=localhost; User ID=pokemon; Password=pokemon; Database=pokemon";

        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
