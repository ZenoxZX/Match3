using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public interface IGridTile
    {
        int Width { get; }
        int Height { get; }
        IGridObject this[int x, int y] { get; }
        IGridObject Get(int x, int y);
        void Set(int x, int y, IGridObject gridObject);
        void Swap(int x1, int y1, int x2, int y2);
        void Swap(IGridObject a, IGridObject b);
        bool IsInside(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
        
        IEnumerable<IGridObject> GetNeighbors(int x, int y)
        {
            if (IsInside(x - 1, y)) yield return Get(x - 1, y);
            if (IsInside(x + 1, y)) yield return Get(x + 1, y);
            if (IsInside(x, y - 1)) yield return Get(x, y - 1);
            if (IsInside(x, y + 1)) yield return Get(x, y + 1);
        }
        
        IGridObject GetNeighbor(int x, int y, int directionX, int directionY)
        {
            if (directionX == 0 && directionY == 0)
                throw new System.ArgumentException("Direction cannot be (0, 0)");
            
            directionX = Mathf.Clamp(directionX, -1, 1);
            directionY = Mathf.Clamp(directionY, -1, 1);
            return IsInside(x + directionX, y + directionY) ? Get(x + directionX, y + directionY) : null;
        }
    }
}