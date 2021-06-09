using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class Repository
{
    private SqliteConnection connection;

    public Repository(string databaseFilepath)
    {
        this.connection = new SqliteConnection($"Data Source = {databaseFilepath}");
    }

    public void CreateTable()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = 
        @"
            CREATE TABLE ceremonies (
	        Year INTEGER NOT NULL,
	        Ceremony INTEGER NOT NULL,
	        Award TEXT NOT NULL,
	        Winner INTEGER,
	        Name TEXT NOT NULL,
	        Film TEXT NOT NULL
        );";

        connection.Close();
    }

    public List<Oscar> GetAward(string award)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM ceremonies WHERE award = $award";
        command.Parameters.AddWithValue("$award", award);
        
        SqliteDataReader reader = command.ExecuteReader();
        
        List<Oscar> oscars = null;
        while (reader.Read())
        {
            Oscar oscar = new Oscar();
            oscar.year = int.Parse(reader.GetString(0));
            oscar.ceremony = int.Parse(reader.GetString(1));
            oscar.award = reader.GetString(2);
            oscar.winner = int.Parse(reader.GetString(3));
            oscar.name = reader.GetString(4);
            oscar.film = reader.GetString(5);
            oscars.Add(oscar);
        }
        reader.Close();
        connection.Close();

        return oscars;
    }

    public HashSet<Oscar> GetAll()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM ceremonies";
        
        SqliteDataReader reader = command.ExecuteReader();
        
        HashSet<Oscar> oscars = null;
        while (reader.Read())
        {
            Oscar oscar = new Oscar();
            oscar.year = int.Parse(reader.GetString(0));
            oscar.ceremony = int.Parse(reader.GetString(1));
            oscar.award = reader.GetString(2);
            oscar.winner = int.Parse(reader.GetString(3));
            oscar.name = reader.GetString(4);
            oscar.film = reader.GetString(5);
            oscars.Add(oscar);
        }
        reader.Close();
        connection.Close();

        return oscars;
    }
}