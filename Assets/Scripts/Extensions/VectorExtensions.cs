using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ToVector3X0Z(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }
        
        public static Vector3 AddX(this Vector3 vector3, float x)
        {
            return new Vector3(vector3.x + x, vector3.y, vector3.z);
        }
        
        public static Vector3 AddY(this Vector3 vector3, float y)
        {
            return new Vector3(vector3.x, vector3.y + y, vector3.z);
        }
        
        public static Vector3 AddZ(this Vector3 vector3, float z)
        {
            return new Vector3(vector3.x, vector3.y, vector3.z + z);
        }
        
        public static Vector3 RandomInsideUnitCircle(float radius = 1)
        {
            Vector2 randomPosition = Random.insideUnitCircle * radius;
            return randomPosition
                .ToVector3X0Z();

        }
    }
}