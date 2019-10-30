using System.IO;
using UnityEditor;
using UnityEngine;

namespace NineHundredLbs.Controly.Editor
{
    public class ProcessorsSettings : ScriptableObject
    {
        private static string AssetPath { get => Path.Combine(ControlyPath.EDITOR_INTERNAL_PATH , "ProcessorsSettings.asset"); }

        private static ProcessorsSettings m_Instance;

        public static ProcessorsSettings Instance
        {
            get
            {
                if (m_Instance != null)
                    return m_Instance;
                m_Instance = AssetDatabase.LoadAssetAtPath<ProcessorsSettings>(AssetPath);

                if (m_Instance == null)
                    throw new System.Exception("Unable to find ProcessorsSettings asset at " + AssetPath);

                ControlyEditorUtils.SetDirty(m_Instance, true);
                return m_Instance;
            }
        }

        public bool RunDefineSymbolsProcessor = true;

        /// <summary>
        /// [Editor Only] Marks target object as dirty. (Only suitable for non-scene objects)
        /// </summary>
        /// <param name="saveAssets"> Write all unsaved asset changed to disk? </param>
        public void SetDirty(bool saveAssets) => ControlyEditorUtils.SetDirty(this, saveAssets);
    }
}
