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
            //Scorbunny evolves to Raboot at level 2 and Raboot evolves to Cinderace at level 3.
            List<PokemonMaster> pokemonMasters = new List<PokemonMaster>(){
                new PokemonMaster("Pikachu", 2, "Raichu"),
                new PokemonMaster("Eevee", 3, "Flareon"),
                new PokemonMaster("Charmander", 1, "Charmeleon"),
                new PokemonMaster("Riolu", 2, "Lucario"),
                new PokemonMaster("Scorbunny", 2, "Raboot"),
                new PokemonMaster("Raboot", 3, "Cinderace")
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
                String choice = input("Enter an option: ");

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
                            Clear();
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
                    int tradeCount = 0;
                    using (var context = new PokemonDataContext())
                    {
                        trainer = context.Trainers.Find(trainerId);
                        // Check trade requests count
                        tradeCount = context.Trades.Where(t => t.ToTrainer == trainerId).Count();
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
                    Console.WriteLine("(4). View Trade Requests (" + tradeCount + ")");
                    Console.WriteLine("(5). Create Trade Request");
                    Console.WriteLine("(b). Main Menu");
                    String choice = input("Enter an option: ");

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
                        case "4":
                            // Evolve pokemon
                            checkTrade();
                            break;
                        case "5":
                            // Evolve pokemon
                            createTrade();
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
                        List<Trade> trades = context.Trades.Where(t => t.FromTrainer == trainerId || t.ToTrainer == trainerId).ToList();
                        context.Trades.RemoveRange(trades);
                        context.Trainers.Remove(new Trainer { TrainerId = trainerId });
                        context.SaveChanges();
                    }
                } else
                {
                    Clear();
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
                    String choice = input("Please only enter options [1 - 4] or 'b' to go back: ");

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
                                p = new Pikachu(Hp: hp, Exp: exp);
                                break;
                            case "charmander":
                                p = new Charmander(Hp: hp, Exp: exp);
                                break;
                            case "eevee":
                                p = new Eevee(Hp: hp, Exp: exp);
                                break;
                            case "riolu":
                                p = new Riolu(Hp: hp, Exp: exp);
                                break;
                            case "zeraora":
                                p = new Zeraora(Hp: hp, Exp: exp);
                                break;
                            case "pachirisu":
                                p = new Pachirisu(Hp: hp, Exp: exp);
                                break;
                            case "scorbunny":
                                p = new Scorbunny(Hp: hp, Exp: exp);
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

            void checkTrade()
            {
                int tradeId = 0;
                Trade trade = new Trade(0,0,0,0);
                Clear();
                using (var context = new PokemonDataContext())
                {
                    List<Trade> list = context.Trades.Where(t => t.ToTrainer == trainerId).ToList();
                    Console.WriteLine("Incoming Trade Requests:");
                    foreach (Trade t in list)
                    {
                        string from = context.Trainers.Find(t.FromTrainer).Name;
                        string to = context.Trainers.Find(t.ToTrainer).Name;
                      
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Trade ID: " + t.TradeId);
                        Console.WriteLine("From: " + from);
                        Console.WriteLine("To: " + to);
                        Console.WriteLine("--------------------------");
                    }

                    if (list.Count() == 0)
                    {
                        Clear();
                        Console.WriteLine("You do not have any trade requests yet...");
                        return;
                    }

                    while (true)
                    {
                        try
                        {
                            tradeId = int.Parse(input("Enter trade ID to view its details: "));
                            trade = context.Trades.Find(tradeId);
                            if (trade == null)
                            {
                                throw new Exception();
                            }
                            break;
                        } catch (Exception)
                        {
                            Console.WriteLine("Trade ID is not valid");
                            continue;
                        }
                    }

                    // Display trade details
                    Clear();
                    Console.WriteLine("Trade Details: ");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Trade ID: " + trade.TradeId);
                    Console.WriteLine("From: " + context.Trainers.Find(trade.FromTrainer).Name);
                    Console.WriteLine("To: " + context.Trainers.Find(trade.ToTrainer).Name);
                    Console.WriteLine("--------------------------");
                    
                    Pokemon to_p = context.Pokemons.Find(trade.ToPokemon);
                    Pokemon from_p = context.Pokemons.Find(trade.FromPokemon);
                    Console.WriteLine("Pokemon to Give:");
                    Console.WriteLine("Name: " + to_p.Name);
                    Console.WriteLine("HP: " + to_p.Hp);
                    Console.WriteLine("EXP: " + to_p.Exp);
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Pokemon to Receive:");
                    Console.WriteLine("Name: " + from_p.Name);
                    Console.WriteLine("HP: " + from_p.Hp);
                    Console.WriteLine("EXP: " + from_p.Exp);
                    Console.WriteLine("--------------------------");

                    Console.WriteLine("1. Accept");
                    Console.WriteLine("2. Reject");
                    Console.WriteLine("3. Back");
                    try
                    {
                        int choice = int.Parse(input("Enter choice: "));
                        switch (choice)
                        {
                            case 1:
                                // Accept trade, swap pokemon and delete trade
                                // change foreign key to trainer id
                                to_p.Trainer = context.Trainers.Find(trade.FromTrainer);
                                from_p.Trainer = context.Trainers.Find(trade.ToTrainer);
                                context.SaveChanges();
                                context.Trades.Remove(trade);
                                context.SaveChanges();
                                Clear();
                                Console.WriteLine("Trade accepted!");
                                break;
                            case 2:
                                // Delete trade
                                context.Trades.Remove(trade);
                                context.SaveChanges();
                                Clear();
                                Console.WriteLine("Trade rejected!");
                                break;
                            case 3:
                                Clear();
                                return;
                            default:
                                throw new Exception();
                        }
                    } catch (Exception)
                    {
                        Console.WriteLine("Invalid choice");
                        return;
                    }
                }   
            }

            void createTrade()
            {
                Clear();
                int to_p = 0;
                int from_p = 0;
                int to_t = 0;
                int from_t = 0;

                // Set from_t to current trainer
                using (var context = new PokemonDataContext())
                {
                    from_t = context.Trainers.Find(trainerId).TrainerId;

                    // Check if there are any trainers to trade with
                    if (context.Trainers.Count() == 1)
                    {
                        Console.WriteLine("There are no other trainers to trade with...");
                        return;
                    }

                    // Check if the current trainer has any pokemon
                    if (context.Trainers.Include(t => t.Pokemons).FirstOrDefault(t => t.TrainerId == trainerId).Pokemons.Count() == 0)
                    {
                        Console.WriteLine("You do not have any pokemon to trade...");
                        return;
                    }
                }

                // List all trainers to trade with
                Console.WriteLine("Trainers available to trade with:");
                using (var context = new PokemonDataContext())
                {
                    var list = context.Trainers.Where(t => t.TrainerId != trainerId).ToList();
                    foreach (Trainer t in list)
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Trainer ID: " + t.TrainerId);
                        Console.WriteLine("Trainer Name: " + t.Name);
                        Console.WriteLine("--------------------------");
                    }

                    // Set to_t to selected trainer
                    while (true)
                    {
                        try
                        {
                            int to_t_id = int.Parse(input("Enter Trainer ID to trade with: "));
                            // Check if the selected trainer is the current trainer

                            if (to_t_id == trainerId)
                            {
                                Console.WriteLine("You cannot trade with yourself");
                                continue;
                            }

                            to_t = context.Trainers.Find(to_t_id).TrainerId;
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Trainer ID is not valid");
                            continue;
                        }
                    }

                    // Check if the selected trainer has any pokemon
                    if (context.Trainers.Include(t => t.Pokemons).FirstOrDefault(t => t.TrainerId == to_t).Pokemons.Count() == 0)
                    {
                        Clear();
                        Console.WriteLine("The selected trainer does not have any pokemon to trade...");
                        return;
                    }
                }
                
                // List current trainer's pokemon to trade with ID
                Clear();
                Console.WriteLine("My Pokemon available to trade:");
                using (var context = new PokemonDataContext())
                {
                    // List all pokemon
                    var list = context.Trainers.Include(t => t.Pokemons).FirstOrDefault(x => x.TrainerId == trainerId).Pokemons.ToList();
                    foreach (Pokemon p in list)
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Pokemon ID: " + p.PokemonId);
                        Console.WriteLine("Name: " + p.Name);
                        Console.WriteLine("HP: " + p.Hp);
                        Console.WriteLine("EXP: " + p.Exp);
                        Console.WriteLine("Skill: " + p.Skill);
                        Console.WriteLine("--------------------------");
                    }

                    // Set from_p to selected pokemon
                    if (list.Count() == 0)
                    {
                        Console.WriteLine("You do not have any pokemon yet...");
                    }
                    else
                    {
                        while (true)
                        {
                            try
                            {
                                int from_p_id = int.Parse(input("Enter Pokemon ID to trade: "));
                                from_p = context.Trainers.Find(trainerId).Pokemons.ToList().Where(p => p.PokemonId == from_p_id).FirstOrDefault().PokemonId;
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Pokemon ID is not valid");
                                continue;
                            }
                        }
                    }
                }

                // List selected trainer's pokemon to trade with ID
                Clear();
                Console.WriteLine("Select the other person's Pokemon to trade with:");
                using (var context = new PokemonDataContext())
                {
                    var list = context.Trainers.Include(t => t.Pokemons).FirstOrDefault(x => x.TrainerId == to_t).Pokemons.ToList();
                    foreach (Pokemon p in list)
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Pokemon ID: " + p.PokemonId);
                        Console.WriteLine("Name: " + p.Name);
                        Console.WriteLine("HP: " + p.Hp);
                        Console.WriteLine("EXP: " + p.Exp);
                        Console.WriteLine("Skill: " + p.Skill);
                        Console.WriteLine("--------------------------");
                    }

                    // Set to_p to selected pokemon
                    if (list.Count() == 0)
                    {
                        Console.WriteLine("The selected user does not have any pokemons yet...");
                    }
                    else
                    {
                        while (true)
                        {
                            try
                            {
                                int to_p_id = int.Parse(input("Enter Pokemon ID to trade: "));
                                to_p = context.Trainers.Find(to_t).Pokemons.ToList().Where(p => p.PokemonId == to_p_id).FirstOrDefault().PokemonId;
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Pokemon ID is not valid");
                                continue;
                            }
                        }
                    }
                }

                // check if pokemon is already in trade
                using (var context = new PokemonDataContext())
                {
                    var list = context.Trades.ToList();
                    foreach (Trade t in list)
                    {
                        if (t.FromPokemon == from_p || t.ToPokemon == to_p || t.FromPokemon == to_p || t.ToPokemon == from_p)
                        {
                            Clear();
                            Console.WriteLine("Pokemon selected is already in another trade request");
                            return;
                        }
                    }
                }   
                
                using (var context = new PokemonDataContext())
                {
                    Trade trade = new Trade(to_t, from_t, to_p, from_p);    
                    context.Trades.Add(trade);
                    context.SaveChanges();
                }

                Clear();
                Console.WriteLine("Trade request sent!");
            }
        }

        
    }
}
