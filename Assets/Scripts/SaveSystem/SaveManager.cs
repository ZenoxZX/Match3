using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Zenject;

namespace SaveSystem
{
    [UsedImplicitly]
    public class SaveManager : IInitializable, IDisposable
    {
        [Inject] private IDataService m_DataService;
        
        private readonly List<IDataPersist> m_DataPersistList = new();
        

        public void Save()
        {
            foreach (IDataPersist dataPersist in m_DataPersistList)
                m_DataService.Save(dataPersist.GetData(), dataPersist);
            
            foreach (IDataPersist dataPersist in FindAllMonoDataPersists())
                m_DataService.Save(dataPersist.GetData(), dataPersist);

            m_DataService.Write();
        }
        
        public void Load()
        {
            foreach (IDataPersist dataPersist in m_DataPersistList)
                dataPersist.SetData(m_DataService.Load(dataPersist));
            
            foreach (IDataPersist dataPersist in FindAllMonoDataPersists())
                dataPersist.SetData(m_DataService.Load(dataPersist));
        }
        
        public void Register(IDataPersist dataPersist) => m_DataPersistList.Add(dataPersist);
        public void Unregister(IDataPersist dataPersist) => m_DataPersistList.Remove(dataPersist);

        void IInitializable.Initialize() => m_DataService.Read();

        void IDisposable.Dispose() => m_DataService.Write();
        
        private static IEnumerable<IDataPersist> FindAllMonoDataPersists() =>
            SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(go => go.GetComponents<IDataPersist>());
    }
}