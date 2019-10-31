using System.IO;
using UnityEngine;

namespace NineHundredLbs.Controly.Editor
{
    public static class ControlyPath
    {
        private const string PACKAGES_PATH = "Packages/";
        private const string PACKAGE_NAME = "com.900lbs.controly";

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
                if (!string.IsNullOrEmpty(m_BasePath))
                    return m_BasePath;

                DirectoryInfo baseDirectory = new DirectoryInfo(Path.Combine(PACKAGES_PATH, PACKAGE_NAME));
                Debug.Assert(baseDirectory != null, "baseDirectory != null");
                string baseDirectoryPath = baseDirectory.ToString().Replace('\\', '/');
                m_BasePath = baseDirectoryPath;
                return m_BasePath;
            }
        }
    }
}
