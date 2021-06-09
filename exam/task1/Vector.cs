public class Vector
{
    public int x;
    public int y;
    public override string ToString()
    {
        return string.Format($"[{x}; {y}]");
    }
}