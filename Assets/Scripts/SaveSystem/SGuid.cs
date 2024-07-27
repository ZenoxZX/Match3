using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaveSystem
{
    [Serializable, HideLabel, PropertyOrder(-1)]
    public class SGuid
    {
        [ReadOnly, LabelText("GUID"), CustomContextMenu("Change GUID", nameof(ChangeGuid))]
        [SerializeField] 
        private string m_GuidString = Guid.NewGuid().ToString();

        public string GuidString => m_GuidString;
        
        private void ChangeGuid() => m_GuidString = Guid.NewGuid().ToString();
    }
}