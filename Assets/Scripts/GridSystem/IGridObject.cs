namespace GridSystem
{
    public interface IGridObject
    {
        int X { get; }
        int Y { get; }
        
        void Set(int x, int y);
    }
}