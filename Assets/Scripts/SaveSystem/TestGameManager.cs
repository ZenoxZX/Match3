using System;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaveSystem
{
    public class TestGameManager : MonoBehaviour, IDataPersist
    {
        public int Score;
        public int Level;
        public int Health;
        public int Mana;
        public int Experience;
        public int Gold;
        
        [field: SerializeField, PropertyOrder(-1)] public SGuid SGuid { get; private set; }
        public object GetData()
        {
            return new MyData
            {
                Name = gameObject.name,
                Score = Score,
                Level = Level,
                Health = Health,
                Mana = Mana,
                Experience = Experience,
                Gold = Gold
            };
        }

        public void SetData(object data)
        {
            JObject jObject = (JObject)data;
            MyData myData = jObject.ToObject<MyData>();
            
            Score = myData.Score;
            Level = myData.Level;
            Health = myData.Health;
            Mana = myData.Mana;
            Experience = myData.Experience;
            Gold = myData.Gold;
        }
        
        [Serializable]
        public class MyData
        {
            public string Name;
            public int Score;
            public int Level;
            public int Health;
            public int Mana;
            public int Experience;
            public int Gold;
        }
    }
}