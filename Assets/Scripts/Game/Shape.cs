using System.Collections.Generic;
using GridSystem;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Shape : MonoBehaviour, IGridObject
    {
        private const string k_Path = "Prefabs/Shapes/";
        [SerializeField] private Type m_Type = Type.Rect;

        private GameObject m_Model;
        private int m_X = -1;
        private int m_Y = -1;
        
        public Type MyType => m_Type;
        public int X => m_X;
        public int Y => m_Y;
        
        public void Set(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }
        
        public void SetType(Type type)
        {
            if (m_Model == null)
            {
                Destroy(m_Model);
            }

            m_Model = Instantiate(Load(type).gameObject, transform);
            m_Type = type;
            gameObject.name = type.ToString();
        }

        public override string ToString()
        {
            return $"({m_X}, {m_Y}) \n {m_Type}";
        }

        #region Load

        public static GameObject Load(Type type) => Resources.Load<GameObject>(k_Path + type);

        public static IEnumerable<GameObject> LoadAll() => Resources.LoadAll<GameObject>(k_Path);

        #endregion
        
        public enum Type
        {
            Rect,
            Circle,
            Triangle,
            Hexagon,
            Star,
            Diamond,
            Heart,
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<Shape>
        {
            
        }
    }
}