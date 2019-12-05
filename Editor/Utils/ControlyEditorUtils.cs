using UnityEditor;
using UnityEngine;

namespace NineHundredLbs.Controly.Editor
{
    public static class ControlyEditorUtils
    {
        /// <summary>
        /// [Editor Only] Marks target object as dirty. (Only suitable for non-scene objects).
        /// </summary>
        /// <param name="target">The object to mark as dirty.</param>
        /// <param name="saveAssets">Whether to write all unsaved asset changes to disk.</param>
        public static void SetDirty(Object target, bool saveAssets)
        {
            if (target == null)
                return;

            SetDirty(target);

            if (saveAssets)
                SaveAssets();
            else
                ControlySettings.Instance.AssetDatabaseSaveAssetsNeeded = true;
        }

        /// <summary>
        /// [Editor Only] Marks target object as dirty. (Only suitable for non-scene objects).
        /// </summary>
        /// <param name="target">The object to mark as dirty.</param>
        public static void SetDirty(Object target)
        {
            if (target == null)
                return;

            EditorUtility.SetDirty(target);
            ControlySettings.Instance.AssetDatabaseSaveAssetsNeeded = true;
        }

        public static void SaveAssets()
        {
            ControlySettings.Instance.AssetDatabaseSaveAssetsNeeded = false;
            AssetDatabase.SaveAssets();
        }
    }
}

