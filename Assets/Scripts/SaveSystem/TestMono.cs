using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SaveSystem
{
    public class TestMono : MonoBehaviour, IDataPersist
    {
        [field: SerializeField] public SGuid SGuid { get; private set; }
        
        public object GetData()
        {
            Transform t = transform;
            
            return new MyData
            {
                Name = gameObject.name,
                Position = t.position,
                Rotation = t.rotation
            };
        }

        public void SetData(object data)
        {
            JObject jObject = (JObject)data;
            MyData myData = jObject.ToObject<MyData>();
            Transform t = transform;
            
            gameObject.name = myData.Name;
            t.position = myData.Position;
            t.rotation = myData.Rotation;
        }
        
        [Serializable]
        private class MyData
        {
            public string Name;
            public SerializableVector3 Position;
            public SerializableQuaternion Rotation;
        }
    }
}