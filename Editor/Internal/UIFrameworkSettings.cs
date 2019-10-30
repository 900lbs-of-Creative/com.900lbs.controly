using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NineHundredLbs.UIFramework.Editor
{
    [Serializable]
    public class UIFrameworkSettings : ScriptableObject
    {
        private static string AssetPath { get => Path.Combine(UIFrameworkPath.EDITOR_INTERNAL_PATH, "UIFrameworkSettings.asset"); }

        private static UIFrameworkSettings m_Instance;
        public static UIFrameworkSettings Instance
        {
            get
            {
                if (m_Instance != null)
                    return m_Instance;
                m_Instance = AssetDatabase.LoadAssetAtPath<UIFrameworkSettings>(AssetPath);

                if (m_Instance == null)
                    throw new System.Exception("Unable to find UIFrameworkSettings asset at " + AssetPath);

                UIFrameworkEditorUtils.SetDirty(m_Instance, true);
                return m_Instance;
            }
        }

        public bool DoozyEngineDetected;
        public bool EnhancedScrollerDetected;

        public void SetDirty(bool saveAssets) 
        { 
            UIFrameworkEditorUtils.SetDirty(this, saveAssets); 
        }
    }

    public class UIFrameworkSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/UI Framework")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UIFrameworkSettingsWindow), false, "UI Framework");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Run define symbols processor"))
            {
                ProcessorsSettings.Instance.RunDefineSymbolsProcessor = true;
                DefineSymbolsProcessor.ExecuteProcessor();
            }
        }
    }
}
