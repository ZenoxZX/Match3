using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = EnVar.GameName + "/" + nameof(ProjectSettings), fileName = nameof(ProjectSettings))]
    public class ProjectSettings : SerializedScriptableObject
    {
        private const string k_GeneralSettings = "General Settings";
        
        public InputHandler.Settings InputHandlerSettings => m_InputHandlerSettings;
        [SerializeField, FoldoutGroup(k_GeneralSettings)] private InputHandler.Settings m_InputHandlerSettings;
    }
}