using System;

interface ILogger
{
   void Log(string message);
   void LogError(string errorMessage);
}
