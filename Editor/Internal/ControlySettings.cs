using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NineHundredLbs.Controly.Editor
{
    [Serializable]
    public class ControlySettings : ScriptableObject
    {
        private static string AssetPath { get => Path.Combine(ControlyPath.EDITOR_INTERNAL_PATH, "ControlySettings.asset"); }

        private static ControlySettings m_Instance;
        public static ControlySettings Instance
        {
            get
            {
                if (m_Instance != null)
                    return m_Instance;
                m_Instance = AssetDatabase.LoadAssetAtPath<ControlySettings>(AssetPath);

                if (m_Instance == null)
                    throw new System.Exception("Unable to find ControlySettings asset at " + AssetPath);

                ControlyEditorUtils.SetDirty(m_Instance, true);
                return m_Instance;
            }
        }

        public bool DoozyEngineDetected;
        public bool EnhancedScrollerDetected;

        public void SetDirty(bool saveAssets) 
        { 
            ControlyEditorUtils.SetDirty(this, saveAssets); 
        }
    }

    public class ControlySettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Controly")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ControlySettingsWindow), false, "Controly");
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
