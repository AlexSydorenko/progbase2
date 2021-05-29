using System;
using System.IO;
using System.Collections.Generic;

namespace control_work_2
{
    class Program
    {
        static string datasetFilepath = "/home/alex/projects/progbase2/control work 2/Video Games Sales (CSV) - Full.csv";
        
        static void Main(string[] args)
        {
            string databaseFilepath = "/home/alex/projects/progbase2/control work 2/games";
            GameRepository gameRepo = new GameRepository(databaseFilepath);

            while (true)
            {
                Console.Write("Enter command: ");
                string command = Console.ReadLine();

                if (command == "import games")
                {
                    ImportAllGames(gameRepo, databaseFilepath);
                }
                else if (command == "import platforms")
                {
                    ImportAllPlatforms(gameRepo, databaseFilepath);
                }
                else if (command == "all games")
                {
                    List<Game> games = GetAllGames(gameRepo);
                    foreach (Game game in games)
                    {
                        Console.WriteLine(game.ToString());
                    }
                }
                else if (command == "all platforms")
                {
                    List<Platform> platforms = GetAllPlatforms(gameRepo);
                    foreach (Platform platform in platforms)
                    {
                        Console.WriteLine(platform.ToString());
                    }
                }
                else if (command == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Unknown command!");
                }
            }
        }

        static Game CreateGame(string[] data)
        {
            return new Game() {
                id = int.Parse(data[0]),
                name = data[1],
                year = int.Parse(data[3])
            };
        }

        static Platform CreatePlatform(string[] data)
        {
            return new Platform() {
                id = int.Parse(data[0]),
                name = data[2]
            };
        }

        static void ImportAllGames(GameRepository gameRepo, string datasetFilepath)
        {
            StreamReader sr = new StreamReader(datasetFilepath);
            while (true)
            {
                string s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                string[] data = s.Split(',');
                Game game = CreateGame(data);
                gameRepo.InsertGame(game);
            }
        }

        static void ImportAllPlatforms(GameRepository gameRepo, string datasetFilepath)
        {
            StreamReader sr = new StreamReader(datasetFilepath);
            while (true)
            {
                string s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                string[] data = s.Split(',');
                Platform platform = CreatePlatform(data);
                gameRepo.InsertPlatform(platform);
            }
        }

        static List<Game> GetAllGames(GameRepository gameRepo)
        {
            return gameRepo.GetAllGames();
        }

        static List<Platform> GetAllPlatforms(GameRepository gameRepo)
        {
            return gameRepo.GetAllPlatforms();
        }
    }
}
