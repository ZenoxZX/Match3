using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Game;

namespace Data
{
    [CreateAssetMenu(menuName = EnVar.GameName + "/" + nameof(LevelData), fileName = nameof(LevelData) + "0001")]
    public class LevelData : SerializedScriptableObject
    {
        private const string k_Path = "Levels/";
        
        [SerializeField] private int m_Width;
        [SerializeField] private int m_Height;
        [OdinSerialize, DictionaryDrawerSettings(IsReadOnly = true)] private Dictionary<Shape.Type, bool> m_Shapes;

        #region Lifecycle

        private void Reset()
        {
            m_Width = 8;
            m_Height = 8;
            m_Shapes = new();
            
            foreach (Shape.Type shape in Enum.GetValues(typeof(Shape.Type)))
            {
                m_Shapes.Add(shape, false);
            }
            
            m_Shapes[Shape.Type.Rect] = true;
            m_Shapes[Shape.Type.Circle] = true;
            m_Shapes[Shape.Type.Triangle] = true;
            

        }

        #endregion


        #region Load

        public static LevelData Load(string name) => Resources.Load<LevelData>(k_Path + name);

        public static IEnumerable<LevelData> LoadAll() => Resources.LoadAll<LevelData>(k_Path);

        #endregion
    }
}