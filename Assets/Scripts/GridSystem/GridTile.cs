namespace GridSystem
{
    public sealed class GridTile<T> : IGridTile where T : IGridObject , new()
    {
        private readonly int m_Width;
        private readonly int m_Height;
        
        private readonly T[,] m_Grid;
        
        private GridTile() {}
        
        public GridTile(int width, int height)
        {
            m_Width = width;
            m_Height = height;
            m_Grid = new T[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    m_Grid[x, y] = new();
                    m_Grid[x, y].Set(x, y);
                }
            }
        }
        
        public int Width => m_Width;
        public int Height => m_Height;

        public IGridObject this[int x, int y] => m_Grid[x, y];

        public IGridObject Get(int x, int y) => m_Grid[x, y];

        public void Set(int x, int y, IGridObject gridObject) => m_Grid[x, y] = (T) gridObject;

        public void Swap(int x1, int y1, int x2, int y2)
        {
            m_Grid[x1, y1].Set(x2, y2);
            m_Grid[x2, y2].Set(x1, y1);
            
            (m_Grid[x1, y1], m_Grid[x2, y2]) = (m_Grid[x2, y2], m_Grid[x1, y1]);
        }

        public void Swap(IGridObject a, IGridObject b)
        {
            a.Set(b.X, b.Y);
            b.Set(a.X, a.Y);
            
            (m_Grid[a.X, a.Y], m_Grid[b.X, b.Y]) = (m_Grid[b.X, b.Y], m_Grid[a.X, a.Y]);
        }

      
    }
}