using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class GameRepository
{
    private SqliteConnection connection;

    public GameRepository(string filepath)
    {
        this.connection = new SqliteConnection($"Data Source = {filepath}");
    }

    public long InsertGame(Game game)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO games (name, year)
            VALUES ($name, $year);

            SELECT last_insert_rowid();";

        command.Parameters.AddWithValue("$name", game.name);
        command.Parameters.AddWithValue("$group", game.year);

        long newID = (long)command.ExecuteScalar();
        
        connection.Close();
        
        if (newID == 0)
        {
            return 0;
        }
        return newID;
    }

    public long InsertPlatform(Platform platform)
    {
        connection.Open();
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO platforms (name)
            VALUES ($name);

            SELECT last_insert_rowid();";

        command.Parameters.AddWithValue("$name", platform.name);

        long newID = (long)command.ExecuteScalar();

        connection.Close();

        if (newID == 0)
        {
            return 0;
        }
        return newID;
    }

    public List<Game> GetAllGames()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM games";

        SqliteDataReader reader = command.ExecuteReader();
        List<Game> games = new List<Game>();

        while (reader.Read())
        {
            Game game = new Game();
            game.id = int.Parse(reader.GetString(0));
            game.name = reader.GetString(1);
            game.year = int.Parse(reader.GetString(2));
            games.Add(game);
        }
        reader.Close();
        connection.Close();

        return games;
    }

    public List<Platform> GetAllPlatforms()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM platforms";

        SqliteDataReader reader = command.ExecuteReader();
        List<Platform> platforms = new List<Platform>();

        while (reader.Read())
        {
            Platform platform = new Platform();
            platform.id = int.Parse(reader.GetString(0));
            platform.name = reader.GetString(1);
            platforms.Add(platform);
        }
        reader.Close();
        connection.Close();
        
        return platforms;
    }

    public void ClearAllGames()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM games";

        connection.Close();
    }

    public void ClearAllPlatforms()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM platforms";

        connection.Close();
    }
}
