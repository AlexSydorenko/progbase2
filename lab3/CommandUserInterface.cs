using System;
using System.Text;

class CommandUserInterface
{
    public void ProcessSets(ILogger logger)
    {
        ISetInt setA = new ArraySetInt();
        ISetInt setB = new ArraySetInt();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("Command list:");
        Console.ResetColor();
        Console.WriteLine("{set} add {value}");
        Console.WriteLine("{set} contains {value}");
        Console.WriteLine("{set} remove {value}");
        Console.WriteLine("{set} clear");
        Console.WriteLine("{set} log");
        Console.WriteLine("{set} count");
        Console.WriteLine("{set} read {file}");
        Console.WriteLine("{set} write {file}");
        Console.WriteLine("SetEquals");
        Console.WriteLine("UnionWith");
        Console.WriteLine("exit");
        Console.WriteLine();
        
        string command;
        do
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Enter command: ");
            Console.ResetColor();
            command = Console.ReadLine();
            if (command.Contains("add"))
            {
                ProcessAdd(command, setA, setB, logger);
            }
            else if (command.Contains("contains"))
            {
                ProcessContains(command, setA, setB, logger);
            }
            else if (command.Contains("remove"))
            {
                ProcessRemove(command, setA, setB, logger);
            }
            else if (command.Contains("clear"))
            {
                ProcessClear(command, setA, setB, logger);
            }
            else if (command.Contains("log"))
            {
                ProcessLog(command, setA, setB, logger);
            }
            else if (command.Contains("count"))
            {
                ProcessCount(command, setA, setB, logger);
            }
            else if (command.Contains("read"))
            {
                ISetInt set = ProcessRead(command, setA, setB, logger);
                if (set != null)
                {
                    if (command.StartsWith("a"))
                    {
                        setA = set;
                    }
                    else
                    {
                        setB = set;
                    }
                }
            }
            else if (command.Contains("write"))
            {
                ProcessWrite(command, setA, setB, logger);
            }
            else if (command == "SetEquals")
            {
                ProcessSetEquals(setA, setB, logger);
            }
            else if (command == "UnionWith")
            {
                ProcessUnionWith(setA, setB, logger);
            }
            else if (command != "exit")
            {
                logger.LogError($"Incorrect command: `{command}`!");
            }
        }
        while (command != "exit");
    }

    static void ProcessAdd(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 3)
        {
            logger.LogError("Incorrect number of command args!");
            return;
        }
        if (subcommands[1] != "add")
        {
            logger.LogError($"2nd arg should be `add`, but not `{subcommands[1]}`!");
            return;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is non-existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return;
        }
        int parsedValue = 0;
        if (int.TryParse(subcommands[2], out parsedValue))
        {
            if (subcommands[0] == "a")
            {
                bool result = a.Add(parsedValue);
                if (result)
                {
                    logger.Log($"Number `{parsedValue}` was successfully added to the set `a`!");
                }
                else
                {
                    logger.Log($"Number `{parsedValue}` wasn't added to the set `a`!");
                }
            }
            else
            {
                bool result = b.Add(parsedValue);
                if (result)
                {
                    logger.Log($"Number `{parsedValue}` was successfully added to the set `b`!");
                }
                else
                {
                    logger.Log($"Number `{parsedValue}` wasn't added to the set `b`!");
                }
            }
        }
        else
        {
            logger.LogError("3rd argument shuld be an integer!");
            return;
        }
    }

    static void ProcessContains(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 3)
        {
            logger.LogError("Incorrect number of command args!");
            return;
        }
        if (subcommands[1] != "contains")
        {
            logger.LogError($"2nd arg should be `contains`, but not `{subcommands[1]}`!");
            return;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is not existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return;
        }
        int parsedValue = 0;
        if (int.TryParse(subcommands[2], out parsedValue))
        {
            if (subcommands[0] == "a")
            {
                bool result = a.Contains(parsedValue);
                if (result)
                {
                    logger.Log($"Set `a` contains number `{parsedValue}`!");
                }
                else
                {
                    logger.Log($"Set `a` doesn't contain number `{parsedValue}`!");
                }
            }
            else
            {
                bool result = b.Contains(parsedValue);
                if (result)
                {
                    logger.Log($"Set `b` contains number `{parsedValue}`!");
                }
                else
                {
                    logger.Log($"Set `b` doesn't contain number `{parsedValue}`!");
                }
            }
        }
        else
        {
            logger.LogError("3rd argument shuld be an integer!");
            return;
        }
    }

    static void ProcessRemove(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 3)
        {
            logger.LogError("Incorrect number of command args!");
            return;
        }
        if (subcommands[1] != "remove")
        {
            logger.LogError($"2nd arg should be `remove`, but not `{subcommands[1]}`!");
            return;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is not existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return;
        }
        int parsedValue = 0;
        if (int.TryParse(subcommands[2], out parsedValue))
        {
            if (subcommands[0] == "a")
            {
                bool result = a.Remove(parsedValue);
                if (result)
                {
                    logger.Log($"Number `{parsedValue}` was successfully removed from the set `a`!");
                }
                else
                {
                    logger.Log($"Number `{parsedValue}` wasn't removed from the set `a`! No such number in the set!");
                }
            }
            else
            {
                bool result = b.Remove(parsedValue);
                if (result)
                {
                    logger.Log($"Number `{parsedValue}` was successfully removed from the set `b`!");
                }
                else
                {
                    logger.Log($"Number `{parsedValue}` wasn't removed from the set `b`! No such number in the set!");
                }
            }
        }
        else
        {
            logger.LogError("3rd arg shuld be an integer!");
            return;
        }
    }

    static void ProcessClear(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 2)
        {
            logger.LogError("Incorrect number of command args!");
            return;
        }
        if (subcommands[1] != "clear")
        {
            logger.LogError($"2nd arg should be `clear`, but not `{subcommands[1]}`!");
            return;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is not existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return;
        }
        if (subcommands[0] == "a")
        {
            a.Clear();
            logger.Log("Set `a` cleared!");
        }
        else
        {
            b.Clear();
            logger.Log("Set `b` cleared!");
        }
    }

    static void ProcessLog(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 2)
        {
            logger.LogError("Incorrect number of command args!");
            return;
        }
        if (subcommands[1] != "log")
        {
            logger.LogError($"2nd arg should be `log`, but not `{subcommands[1]}`!");
            return;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is not existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return;
        }
        if (subcommands[0] == "a")
        {
            int[] setA_array = new int[a.GetCount()];
            a.CopyTo(setA_array);
            if (setA_array.Length == 0)
            {
                logger.Log("Set `a` is empty!");
                return;
            }
            StringBuilder setA = new StringBuilder();
            foreach (int item in setA_array)
            {
                setA.Append(item + " ");
            }
            logger.Log("Set `a`: " + setA.ToString());
        }
        else
        {
            int[] setB_array = new int[b.GetCount()];
            b.CopyTo(setB_array);
            if (setB_array.Length == 0)
            {
                logger.Log("Set `b` is empty!");
                return;
            }
            StringBuilder setB = new StringBuilder();
            foreach (int item in setB_array)
            {
                setB.Append(item + " ");
            }
            logger.Log("Set `b`: " + setB.ToString());
        }
    }

    static void ProcessCount(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 2)
        {
            logger.LogError("Incorrect number of command args!");
            return;
        }
        if (subcommands[1] != "count")
        {
            logger.LogError($"2nd arg should be `count`, but not `{subcommands[1]}`!");
            return;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is not existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return;
        }
        if (subcommands[0] == "a")
        {
            logger.Log("Number of values in set `a`: " + a.GetCount().ToString());
        }
        else
        {
            logger.Log("Number of values in set `b`: " + b.GetCount().ToString());
        }
    }

    static ISetInt ProcessRead(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 3)
        {
            logger.LogError("Incorrect number of command args!");
            return null;
        }
        if (subcommands[1] != "read")
        {
            logger.LogError($"2nd arg should be `read`, but not `{subcommands[1]}`!");
            return null;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is not existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return null;
        }
        SetsStorage ss = new SetsStorage();
        ISetInt set = ss.ReadSet(subcommands[2]);
        if (set == null)
        {
            logger.LogError("Incorrect file path or there are mistakes in the file!");
            return null;
        }
        logger.Log($"All values from the file `{subcommands[2]}` were successfully added into set `{subcommands[0]}`!");
        return set;
    }

    static void ProcessWrite(string command, ISetInt a, ISetInt b, ILogger logger)
    {
        string[] subcommands = command.Split(" ");
        if (subcommands.Length != 3)
        {
            logger.LogError("Incorrect number of command args!");
            return;
        }
        if (subcommands[1] != "write")
        {
            logger.LogError($"2nd arg should be `write`, but not `{subcommands[1]}`!");
            return;
        }
        if (subcommands[0] != "a" && subcommands[0] != "b")
        {
            logger.LogError($"It is not existing set: `{subcommands[0]}`! Enter `a` or `b`!");
            return;
        }
        if (subcommands[0] == "a")
        {
            SetsStorage ss = new SetsStorage();
            ss.WriteSet(subcommands[2], a);
            logger.Log($"Set `a` was written to file `{subcommands[2]}`!");
        }
        else
        {
            SetsStorage ss = new SetsStorage();
            ss.WriteSet(subcommands[2], b);
            logger.Log($"Set `b` was written to file `{subcommands[2]}`!");
        }
    }

    static void ProcessSetEquals(ISetInt a, ISetInt b, ILogger logger)
    {
        bool result = a.SetEquals(b);
        if (result)
        {
            logger.Log("Sets are equal!");
        }
        else
        {
            logger.Log("Sets aren't equal!");
        }
    }

    static void ProcessUnionWith(ISetInt a, ISetInt b, ILogger logger)
    {
        a.UnionWith((ArraySetInt)b);
        logger.Log("Sets were successfully united!");
    }
}
