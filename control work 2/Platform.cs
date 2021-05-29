public class Platform
{
    public int id;
    public string name;

    public override string ToString()
    {
        return string.Format($"[{id}] {name}");
    }
}
