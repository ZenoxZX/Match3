#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Data.Editor
{
    public static class ProjectSettingsShortcutEditor
    {
        [MenuItem("Match3/Project Settings", priority = 999)]
        public static void ShowProjectSettings()
        {
            const string path = "Assets/Resources/ProjectSettings.asset";
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
        }
    }
}

#endif
