using System;

class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    public void LogError(string errorMessage)
    {
        Console.WriteLine("ERROR! " + errorMessage);
    }
}
