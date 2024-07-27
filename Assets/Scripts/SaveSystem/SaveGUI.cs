using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace SaveSystem
{
    public class SaveGUI : MonoBehaviour
    {
        [Inject] private SaveManager m_SaveManager;
        
        [Button] private void Save() => m_SaveManager.Save();
        [Button] private void Load() => m_SaveManager.Load();
    }
}