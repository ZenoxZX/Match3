using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace SaveSystem
{
    [UsedImplicitly]
    public class FileDataService : IDataService
    {
        private Dictionary<string, object> m_DataDictionary = new();
        [Inject] private ISerializer m_Serializer;
        
        private const string k_FileName = "data";
        private const string k_FileExtension = "json";
        private readonly string m_DataPath = Application.persistentDataPath;
        
        public void Write()
        {
            string path = GetFilePath();
            string data = m_Serializer.Serialize(m_DataDictionary);
            File.WriteAllText(path, data);
        }

        public void Read()
        {
            string path = GetFilePath();
            
            if (!File.Exists(path))
            {
                Debug.Log("No data file found.");
                return;
            }

            string data = File.ReadAllText(path);
            m_DataDictionary = m_Serializer.Deserialize<Dictionary<string, object>>(data);
        }

        public void Save(object data, IDataPersist dataPersist)
        {
            string guid = dataPersist.SGuid.GuidString;

            if (m_DataDictionary.ContainsKey(guid))
                m_DataDictionary[guid] = data;
            else
                m_DataDictionary.Add(guid, data);
        }

        public object Load(IDataPersist dataPersist)
        {
            string guid = dataPersist.SGuid.GuidString;
            return !m_DataDictionary.ContainsKey(guid) ? default : m_DataDictionary[guid];
        }

        public bool HasData(IDataPersist dataPersist)
        {
            string guid = dataPersist.SGuid.GuidString;
            return m_DataDictionary.ContainsKey(guid);
        }

        public void Delete(IDataPersist dataPersist)
        {
            string guid = dataPersist.SGuid.GuidString;
            m_DataDictionary.Remove(guid);
        }

        public void DeleteAll()
        {
            string filePath = GetFilePath();
            File.Delete(filePath);
            m_DataDictionary.Clear();
        }
        
        private string GetFilePath() 
            => Path.Combine(m_DataPath, string.Concat(k_FileName, ".", k_FileExtension));
    }
}