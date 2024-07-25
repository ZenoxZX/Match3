using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Extensions
{
    public static class UIExtensions
    {
        private static PointerEventData s_EventDataCurrentPosition;
        private static List<RaycastResult> s_Results;

        public static bool IsOverUI
        {
            get
            {
                s_EventDataCurrentPosition = new(EventSystem.current) { position = Input.mousePosition };
                s_Results = new();
                EventSystem.current.RaycastAll(s_EventDataCurrentPosition, s_Results);
                return s_Results.Count > 0;
            }
        }
    }
}