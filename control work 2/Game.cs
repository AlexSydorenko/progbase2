public class Game
{
    public int id;
    public string name;
    public int year;

    public override string ToString()
    {
        return string.Format($"[{id}] {name} - {year}");
    }
}