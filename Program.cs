using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PokemonPocket
{
    class Program
    {
        // Trainer session ID
        public static int trainerId = 0;

        // Helper function for text input
        static string input(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        // Helper function to clear the terminal
        static void Clear() 
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
        }

        static void Main(string[] args)
        {
            //PokemonMaster list for checking pokemon evolution availability.    
            List<PokemonMaster> pokemonMasters = new List<PokemonMaster>(){
                new PokemonMaster("Pikachu", 2, "Raichu"),
                new PokemonMaster("Eevee", 3, "Flareon"),
                new PokemonMaster("Charmander", 1, "Charmeleon")
            };

            //Use "Environment.Exit(0);" if you want to implement an exit of the console program
            //Start your assignment 1 requirements below.
            Clear();
            Console.Title = "PokemonPocket"; 
            while (true)
            {
                List<Trainer> trainers = new List<Trainer>();
                using (var context = new PokemonDataContext())
                {
                    trainers = context.Trainers.ToList();
                }
                Console.WriteLine("**************************************************");
                Console.WriteLine("Welcome to PokemonPocket. Please choose a trainer.");
                Console.WriteLine("**************************************************");
                int i = 1;
                foreach (var trainer in trainers)
                {
                    Console.WriteLine("(" + i + "). " + trainer.Name);
                    i++;
                }
                if (trainers.Count == 0)
                {
                    Console.WriteLine("No trainers in database.");
                }
                Console.WriteLine("(n). Create New Trainer");
                Console.WriteLine("(q). Quit PokemonPocket");
                String choice = input("Please only enter options listed: ");

                switch (choice.ToLower())
                {
                    case "q":
                        // Quit the application
                        Environment.Exit(0);
                        break;
                    case "n":
                        // Create a new trainer
                        createTrainer();
                        break;
                    default:
                        // Selected user or invalid option
                        try
                        {
                            if (int.Parse(choice) <= trainers.Count)
                            {
                                trainerId = trainers[int.Parse(choice) - 1].TrainerId;
                                optionsTrainer();
                            } else
                            {
                                throw new Exception();
                            }
                        } catch (Exception) {
                            Console.WriteLine("The choice entered is invalid. Please try again.");
                        }
                        break;
                }
            }

            void createTrainer()
            {
                Clear();
                string name = input("Enter your trainer name: ");

                using (var context = new PokemonDataContext())
                {
                    Trainer trainer = new Trainer { Name = name };
                    context.Trainers.Add(trainer);
                    context.SaveChanges();
                }
                Console.WriteLine("Successfully created trainer");
            }

            void optionsTrainer()
            {
                bool back = false;
                Clear();
                while (true)
                {
                    Trainer trainer;
                    using (var context = new PokemonDataContext())
                    {
                        trainer = context.Trainers.Find(trainerId);
                    }

                    if (trainer == null)
                    {
                        Clear();
                        break;
                    }

                    Console.WriteLine("******************************");
                    Console.WriteLine("Trainer: " + trainer.Name + " | Trainer Menu");
                    Console.WriteLine("******************************");
                    Console.WriteLine("(1). View " + trainer.Name + "'s Pokemon");
                    Console.WriteLine("(2). Edit Trainer Name");
                    Console.WriteLine("(3). Delete Trainer");
                    Console.WriteLine("(b). Main Menu");
                    String choice = input("Please only enter options listed: ");

                    switch (choice.ToLower())
                    {
                        case "1":
                            // View pokemon menu
                            pokemonMenu();
                            break;
                        case "2":
                            // Edit trainer name
                            editTrainer();
                            break;
                        case "3":
                            // Delete trainer
                            deleteTrainer();
                            break;
                        case "b":
                            // Back to main menu
                            back = true;
                            break;
                        default:
                            // Invalid option
                            Console.WriteLine("The choice entered is invalid. Please try again.");
                            break;
                    }

                    if (back)
                    {
                        Clear();
                        break;
                    }
                }
            }

            void editTrainer()
            {
                Clear();
                Trainer trainer;
                using (var context = new PokemonDataContext())
                {
                    trainer = context.Trainers.Find(trainerId);
                    Console.WriteLine("Current trainer name: " + trainer.Name);
                    trainer.Name = input("Enter new trainer name: ");
                    context.SaveChanges();
                }
            }

            void deleteTrainer()
            {
                // Had a shit load of trouble getting it done in one context so I am just going to use 2 contexts. 
                // I know its not the most efficent method but I can't really be bother to fix.
                Clear();
                Trainer trainer;
                using (var context = new PokemonDataContext())
                {
                    trainer = context.Trainers.Find(trainerId);
                    Console.WriteLine("Delete trainer '" + trainer.Name + "'? You cannot undo this action!");
                }

                Console.WriteLine("(y). Yes");
                Console.WriteLine("(*). No (Any Key)");
                string choice = input("Choose an option: ");

                if (choice == "y")
                {
                    using (var context = new PokemonDataContext())
                    {
                        var pokemons = context.Pokemons.Where(p => p.Trainer.TrainerId == trainerId);
                        foreach (Pokemon p in pokemons)
                        {
                            context.Pokemons.Remove(p);
                        }
                        context.Trainers.Remove(new Trainer { TrainerId = trainerId });
                        context.SaveChanges();
                    }
                }
            }

            void pokemonMenu()
            {
                bool back = false;
                Clear();
                while (true)
                {
                    Trainer trainer;
                    using (var context = new PokemonDataContext())
                    {
                        trainer = context.Trainers.Find(trainerId);
                    }
                    Console.WriteLine("******************************");
                    Console.WriteLine(trainer.Name + "'s Pokemon Pocket");
                    Console.WriteLine("******************************");
                    Console.WriteLine("(1). Add pokemon to my pocket");
                    Console.WriteLine("(2). List pokemon(s) in my pocket");
                    Console.WriteLine("(3). Check if I can evolve pokemon");
                    Console.WriteLine("(4). Evolve pokemon");
                    String choice = input("Please only enter options [1, 2, 3, 4] or 'b' to logout: ");

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
                        case "b":
                            // Logout to user menu
                            back = true;
                            break;
                        default:
                            // Invalid option
                            Console.WriteLine("The choice entered is invalid. Please try again.");
                            break;
                    }

                    if (back)
                    {
                        Clear();
                        break;
                    }
                }
            }

            void createPokemon()
            {
                Clear();
                string name = "";
                int hp = 0;
                int exp = 0;
                Pokemon p;
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
                                p = new Pikachu("Pikachu", Hp: hp, Exp: exp);
                                break;
                            case "charmander":
                                p = new Charmander("Charmander", Hp: hp, Exp: exp);
                                break;
                            case "eevee":
                                p = new Eevee("Eevee", Hp: hp, Exp: exp);
                                break;
                            default: throw new Exception("Pokemon name is not valid.");

                        }

                        using (var context = new PokemonDataContext())
                        {
                            context.Trainers.Find(trainerId).Pokemons.Add(p);
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

            void listPokemon()
            {
                Clear();
                using (var context = new PokemonDataContext())
                {
                    var list = context.Trainers.Include(t => t.Pokemons).FirstOrDefault(x => x.TrainerId == trainerId).Pokemons.OrderByDescending(p => p.Hp);
                    foreach (Pokemon p in list)
                    {
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

            void checkPokemon()
            {
                Clear();
                List<Pokemon> list = new List<Pokemon>();
                int evolveCount = 0;
                using (var context = new PokemonDataContext())
                {
                    list = context.Trainers.Include(t => t.Pokemons).FirstOrDefault(x => x.TrainerId == trainerId).Pokemons.ToList();
                }

                foreach (string unique in list.Select(pokemon => pokemon.Name).Distinct())
                {
                    Pokemon p = list.Where(pokemon => pokemon.Name == unique).First();
                    int count = list.Where(pokemon => pokemon.Name == unique).Count();
                    PokemonMaster checkEvolve = pokemonMasters.SingleOrDefault(s => s.Name == p.Name);
                    if (checkEvolve != null)
                    {
                        if (count >= checkEvolve.NoToEvolve)
                        {
                            evolveCount++;
                            Console.WriteLine(p.Name + " -> " + checkEvolve.EvolveTo);
                        }
                    }
                }
                if (evolveCount == 0)
                {
                    Console.WriteLine("No pokemon to evolve yet...");
                }
            }

            void evolvePokemon()
            {
                Clear();
                List<Pokemon> list = new List<Pokemon>();
                int evolveCount = 0;

                using (var context = new PokemonDataContext())
                {
                    list = context.Trainers.Include(t => t.Pokemons).FirstOrDefault(x => x.TrainerId == trainerId).Pokemons.ToList();
                    foreach (string unique in list.Select(pokemon => pokemon.Name).Distinct())
                    {
                        Pokemon p = list.Where(pokemon => pokemon.Name == unique).First();
                        int count = list.Where(pokemon => pokemon.Name == unique).Count();
                        PokemonMaster checkEvolve = pokemonMasters.SingleOrDefault(s => s.Name == p.Name);
                        if (checkEvolve != null)
                        {
                            if (count >= checkEvolve.NoToEvolve)
                            {
                                int delete_count = checkEvolve.NoToEvolve - 1;
                                string delete_name = p.Name;

                                evolveCount++;
                                Console.WriteLine("Evolving " + p.Name + " into " + checkEvolve.EvolveTo);
                                p.Name = checkEvolve.EvolveTo;
                                p.Hp = 100;
                                p.Exp = 0;
                                context.SaveChanges();

                                for (int i = 0; i < delete_count; i++)
                                {
                                    context.Trainers.Find(trainerId).Pokemons.Remove(context.Trainers.Find(trainerId).Pokemons.Where(pokemon => pokemon.Name == unique).First());
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                }

                if (evolveCount > 0)
                {
                    listPokemon();
                }
                else
                {
                    Console.WriteLine("No pokemon to evolve yet...");
                }
            }
        }

        
    }
}
