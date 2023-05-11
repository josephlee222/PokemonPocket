using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonPocket
{
    class Program
    {
        static string input(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        static void Clear() 
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
        }

        static void Main(string[] args)
        {
            //PokemonMaster list for checking pokemon evolution availability.    
            /*List<PokemonMaster> pokemonMasters = new List<PokemonMaster>(){
                new PokemonMaster("Pikachu", 2, "Raichu"),
                new PokemonMaster("Eevee", 3, "Flareon"),
                new PokemonMaster("Charmander", 1, "Charmeleon")
            };*/

            //Use "Environment.Exit(0);" if you want to implement an exit of the console program
            //Start your assignment 1 requirements below.
            Clear();
            while (true)
            {
                Console.WriteLine("******************************");
                Console.WriteLine("Welcome to Pokemon Pocket App");
                Console.WriteLine("******************************");
                Console.WriteLine("(1). Add pokemon to my pocket");
                Console.WriteLine("(2). List pokemon(s) in my pocket");
                Console.WriteLine("(3). Check if I can evolve pokemon");
                Console.WriteLine("(4). Evolve pokemon");
                String choice = input("Please only enter options [1, 2, 3, 4] or 'q' to quit the program: ");

                switch (choice.ToLower())
                {
                    case "1":
                        // Create Pokemon
                        createPokemon();
                        break;
                    case "2":
                        // List pokemons
                        listPokemon();
                        break;
                    case "3":
                        // Evolve checker
                        checkPokemon();
                        break;
                    case "4":
                        // Evolve pokemon
                        evolvePokemon();
                        break;
                    case "q":
                        // Quit the application
                        Environment.Exit(0);
                        break;
                    default:
                        // Invalid option
                        Console.WriteLine("The choice entered is invalid. Please try again.");
                        break;
                }
            }
        }

        static void createPokemon()
        {
            Clear();
            string name = "";
            int hp = 0;
            int exp = 0;
            Pokemon p = new Pokemon();
            while (true)
            {
                try
                {
                    name = input("Enter pokemon name: ");
                    hp = int.Parse(input("Enter Pokemon HP: "));
                    exp = int.Parse(input("Enter Pokemon EXP points: "));

                    switch (name.ToLower())
                    {
                        case "pikachu":
                            p = new Pikachu(_Hp: hp, _Exp: exp);
                            break;
                        case "charmander":
                            p = new Charmander(_Hp: hp, _Exp: exp);
                            break;
                        case "eevee":
                            p = new Eevee(_Hp: hp, _Exp: exp);
                            break;
                        default: throw new Exception("Pokemon name is not valid.");

                    }

                    using (var context = new PokemonDataContext())
                    {
                        context.Pokemons.Add(p);
                        context.SaveChanges();
                    }
                    Console.WriteLine("Pokemon successfully added to database");
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("HP and EXP values should be numeric");
                    continue;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
        }

        static void listPokemon()
        {
            Clear();
            using (var context = new PokemonDataContext())
            {
                var list = context.Pokemons.OrderByDescending(p => p.Hp);
                foreach (Pokemon p in list) {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Name: " + p.Name);
                    Console.WriteLine("HP: " + p.Hp);
                    Console.WriteLine("EXP: " + p.Exp);
                    Console.WriteLine("Skill: " + p.Skill);
                    Console.WriteLine("--------------------------");
                }

                if (list.Count() == 0) 
                {
                    Console.WriteLine("You do not have any pokemon yet...");   
                }
            }
        }

        static void checkPokemon()
        {
            Clear();
            List<Pokemon> list = new List<Pokemon>();
            int evolveCount = 0;
            using (var context = new PokemonDataContext())
            {
                list = context.Pokemons.ToList();
            }

            foreach (string unique in list.Select(pokemon => pokemon.Name).Distinct())
            {
                Pokemon p = list.Where(pokemon => pokemon.Name == unique).First();
                int count = list.Where(pokemon => pokemon.Name == unique).Count();
                if (count >= p.NoToEvolve & p.EvolveTo != "")
                {
                    evolveCount++;
                    Console.WriteLine(p.Name + " -> " + p.EvolveTo);
                }
            }
            if (evolveCount == 0)
            {
                Console.WriteLine("No pokemon to evolve yet...");
            }
        }

        static void evolvePokemon()
        {
            Clear();
            List<Pokemon> list = new List<Pokemon>();
            int evolveCount = 0;

            using (var context = new PokemonDataContext())
            {
                list = context.Pokemons.ToList();
                foreach (string unique in list.Select(pokemon => pokemon.Name).Distinct())
                {
                    Pokemon p = context.Pokemons.Where(pokemon => pokemon.Name == unique).First();
                    int count = context.Pokemons.Where(pokemon => pokemon.Name == unique).Count();
                    
                    if (count >= p.NoToEvolve & p.EvolveTo != "")
                    {
                        int delete_count = p.NoToEvolve - 1;
                        string delete_name = p.Name;

                        evolveCount++;
                        Console.WriteLine("Evolving " + p.Name + " into " + p.EvolveTo);
                        p.Name = p.EvolveTo;
                        p.Hp = 100;
                        p.Exp = 0;
                        p.EvolveTo = "";
                        context.SaveChanges();

                        context.Pokemons.RemoveRange(context.Pokemons.Where(pokemon => pokemon.Name == unique).Take(delete_count));
                        context.SaveChanges();
                    }
                }
            }

            if (evolveCount > 0)
            {
                listPokemon();
            } else
            {
                Console.WriteLine("No pokemon to evolve yet...");
            }
        }
    }
}
