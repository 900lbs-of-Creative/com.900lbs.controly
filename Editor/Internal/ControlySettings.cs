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
                    throw new Exception("Unable to find ControlySettings asset at " + AssetPath);

                return m_Instance;
            }
        }

        public bool DoozyEngineDetected;
        public bool EnhancedScrollerDetected;
        public bool AssetDatabaseSaveAssetsNeeded;
        public bool AssetDatabaseRefreshNeeded;

        /// <summary>
        /// Executed if a Refresh was in order (due to the creation of assets) or a SaveAssets needed,
        /// but NOT performed.
        /// </summary>
        public void SaveAndRefreshAssetDatabase()
        {
            if (AssetDatabaseRefreshNeeded)
            {
                AssetDatabaseSaveAssetsNeeded = false;
                AssetDatabaseRefreshNeeded = false;
                SetDirty(true);
                AssetDatabase.Refresh();
                return;
            }

            if (AssetDatabaseSaveAssetsNeeded)
            {
                AssetDatabaseSaveAssetsNeeded = false;
                SetDirty(true);
            }
        }

        /// <summary>
        /// [Editor Only] Marks target object as dirty. (Only suitable for non-scene objects).
        /// </summary>
        /// <param name="saveAssets">Whether to write all unsaved asset changes to disk.</param>
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
