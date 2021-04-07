using System;
using System.IO;

class ChunkPlainFileLogger : ILogger
{
    private string directory;
    private string filePath;
    private int numOfLines;
    private int maxNumOfLines;

    public ChunkPlainFileLogger(string directory, int maxNumOfLines)
    {
        this.directory = directory;
        this.maxNumOfLines = maxNumOfLines;
    }

    public void Log(string message)
    {
        if (filePath == null || numOfLines == maxNumOfLines)
        {
            CreateFile();
            numOfLines = 0;
        }
        File.AppendAllLines(this.filePath, new string[] {message});
        numOfLines++;
    }

    public void LogError(string errorMessage)
    {
        errorMessage = "ERROR! " + errorMessage;
        if (filePath == null || numOfLines == maxNumOfLines)
        {
            CreateFile();
            numOfLines = 0;
        }
        File.AppendAllLines(this.filePath, new string[] {errorMessage});
        numOfLines++;
    }

    public void CreateFile()
    {
        this.filePath = directory + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.fff", System.Globalization.CultureInfo.InvariantCulture) + ".txt";
        File.Create(filePath).Close();
    }
}
