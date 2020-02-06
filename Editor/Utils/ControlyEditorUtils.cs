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

        /// <summary>
        /// [Editor Only] Writes all unsaved asset changes to the disk.
        /// </summary>
        public static void SaveAssets()
        {
            ControlySettings.Instance.AssetDatabaseSaveAssetsNeeded = false;
            AssetDatabase.SaveAssets();
        }

        /// <summary> [Editor Only] Records any changes done on the object after the RecordObject function </summary>
        /// <param name="objectToUndo"> The reference to the object that you will be modifying </param>
        /// <param name="undoMessage"> The title of the action to appear in the undo history (i.e. visible in the undo menu) </param>
        /// <param name="saveAssets"> Write all unsaved asset changes to disk? </param>
        public static void UndoRecordObject(Object objectToUndo, string undoMessage, bool saveAssets)
        {
            if (objectToUndo == null) return;
            UndoRecordObject(objectToUndo, undoMessage);
            if (saveAssets) SaveAssets();
        }

        /// <summary> [Editor Only] Records any changes done on the object after the RecordObject function </summary>
        /// <param name="objectToUndo"> The reference to the object that you will be modifying </param>
        /// <param name="undoMessage"> The title of the action to appear in the undo history (i.e. visible in the undo menu) </param>
        public static void UndoRecordObject(Object objectToUndo, string undoMessage)
        {
            if (objectToUndo == null) return;
            Undo.RecordObject(objectToUndo, undoMessage);
        }
    }
}

