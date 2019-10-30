using UnityEditor;
using UnityEngine;

namespace NineHundredLbs.Controly.Editor
{
    public static class ControlyEditorUtils
    {
        public static void SetDirty(Object target, bool saveAssets)
        {
            if (target == null)
                return;
            SetDirty(target);
            if (saveAssets)
                SaveAssets();
        }

        public static void SetDirty(Object target)
        {
            if (target == null)
                return;
            EditorUtility.SetDirty(target);
        }

        public static void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }
    }
}

