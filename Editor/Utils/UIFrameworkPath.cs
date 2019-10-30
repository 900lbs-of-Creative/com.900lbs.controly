using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace NineHundredLbs.UIFramework.Editor
{
    public static class UIFrameworkPath
    {
        private const string PACKAGES_PATH = "Packages/";

        private const string UI_FRAMEWORK = "com.900lbs.uiframework";
        private const string EDITOR = "Editor";
        private const string RUNTIME = "Runtime";
        private const string INTERNAL = "Internal";
        private const string VIEW = "VIEW";
        private const string SCROLLER = "SCROLLER";

        public static string UI_FRAMEWORK_PATH = BasePath;
        public static string EDITOR_PATH = Path.Combine(UI_FRAMEWORK_PATH, EDITOR);
        public static string EDITOR_INTERNAL_PATH = Path.Combine(EDITOR_PATH, INTERNAL);

        public static string RUNTIME_PATH = Path.Combine(UI_FRAMEWORK_PATH, RUNTIME);
        public static string RUNTIME_VIEW_PATH = Path.Combine(RUNTIME_PATH, VIEW);
        public static string RUNTIME_VIEW_SCROLLER_PATH = Path.Combine(RUNTIME_VIEW_PATH, SCROLLER);

        private static string m_BasePath;

        public static string BasePath
        {
            get
            {
                if (!string.IsNullOrEmpty(m_BasePath))
                    return m_BasePath;

                DirectoryInfo baseDirectory = new DirectoryInfo(Path.Combine(PACKAGES_PATH, UI_FRAMEWORK));
                Debug.Assert(baseDirectory != null, "baseDirectory != null");
                Assert.AreEqual(UI_FRAMEWORK, baseDirectory.Name);
                string baseDirectoryPath = baseDirectory.ToString().Replace('\\', '/');
                m_BasePath = baseDirectoryPath;
                return m_BasePath;
            }
        }
    }
}
