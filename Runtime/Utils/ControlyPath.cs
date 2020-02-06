using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace NineHundredLbs.Controly.Utils
{
    [Serializable]
    public class ControlyPath : ScriptableObject
    {
        private const string ASSETS_PATH = "Assets/";

        private const string CONTROLY = "Controly";
        private const string EDITOR = "Editor";
        private const string RUNTIME = "Runtime";
        private const string INTERNAL = "Internal";
        private const string UI = "UI";
        private const string VIEW = "View";
        private const string SCROLLER = "Scroller";

        public static string CONTROLY_PATH = BasePath;
        public static string EDITOR_PATH = Path.Combine(CONTROLY_PATH, EDITOR);
        public static string EDITOR_INTERNAL_PATH = Path.Combine(EDITOR_PATH, INTERNAL);

        public static string RUNTIME_PATH = Path.Combine(CONTROLY_PATH, RUNTIME);
        public static string RUNTIME_UI_PATH = Path.Combine(RUNTIME_PATH, UI);
        public static string RUNTIME_UI_VIEW_PATH = Path.Combine(RUNTIME_UI_PATH, VIEW);
        public static string RUNTIME_UI_VIEW_SCROLLER_PATH = Path.Combine(RUNTIME_UI_VIEW_PATH, SCROLLER);

        public static string ASMDEF_CONTROLY = string.Join(".", "900lbs", CONTROLY);
        public static string ASMDEF_CONTROLY_UI = string.Join(".", ASMDEF_CONTROLY, UI);
        public static string ASMDEF_CONTROLY_UI_VIEW_SCROLLER = string.Join(".", ASMDEF_CONTROLY_UI, VIEW, SCROLLER);

        private static string m_BasePath;
        public static string BasePath
        {
            get
            {
#if UNITY_EDITOR
                if (!string.IsNullOrEmpty(m_BasePath))
                    return m_BasePath;

                var scriptableObjectInstance = CreateInstance<ControlyPath>();
                UnityEditor.MonoScript scriptAsset = UnityEditor.MonoScript.FromScriptableObject(scriptableObjectInstance);
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(scriptAsset);
                DestroyImmediate(scriptableObjectInstance);

                FileInfo fileInfo = new FileInfo(assetPath);
                Debug.Assert(fileInfo.Directory != null, "fileInfo.Directory != null");
                Debug.Assert(fileInfo.Directory.Parent != null, "fileInfo.Directory.Parent != null");

                // Currently, this script instance exists in Controly/Editor/Utils
                // so we need to step up twice in directories to get to the root folder.
                DirectoryInfo baseDirectory = fileInfo.Directory.Parent.Parent;
                Debug.Assert(baseDirectory != null, "baseDir != null");
                Assert.AreEqual(CONTROLY, baseDirectory.Name);

                string baseDirectoryPath = baseDirectory.ToString().Replace('\\', '/');
                int index = baseDirectoryPath.LastIndexOf(ASSETS_PATH, StringComparison.Ordinal);
                Assert.IsTrue(index >= 0);
                baseDirectoryPath = baseDirectoryPath.Substring(index);
                m_BasePath = baseDirectoryPath;
                return m_BasePath;
#else
                return "";
#endif
            }
        }
    }
}
