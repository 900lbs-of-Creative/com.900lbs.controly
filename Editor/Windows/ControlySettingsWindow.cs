using UnityEditor;
using UnityEngine;

namespace NineHundredLbs.Controly.Editor
{
    public class ControlySettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Controly")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ControlySettingsWindow), false, "Controly");
        }

        private static ControlySettings Settings => ControlySettings.Instance;
        private bool m_NeedsSave = false;
        private bool m_NeedsToUpdateScriptDefineSymbols = false;

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            if (EditorApplication.isCompiling)
                GUILayout.Label("Compiling...");
            DrawGeneralPluginBoxDoozyEngine();
            EditorGUILayout.Space();
            DrawGeneralPluginBoxEnhancedScroller();
            EditorGUILayout.EndVertical();

            if (m_NeedsToUpdateScriptDefineSymbols)
            {
                m_NeedsToUpdateScriptDefineSymbols = false;
                DefineSymbolsProcessor.UpdateScriptingDefineSymbols();
            }
        }

        private void OnDestroy()
        {
            if (!m_NeedsSave)
                return;

            ControlySettings.Instance.AssetDatabaseSaveAssetsNeeded = true;
            ControlySettings.Instance.SaveAndRefreshAssetDatabase();
        }

        private void DrawGeneralPluginBoxDoozyEngine()
        {
            bool usePlugin = Settings.UseDoozyEngine;
            EditorGUI.BeginChangeCheck();
            usePlugin = DrawGeneralPluginBox(
                "Doozy UI",
                Settings.DoozyEngineDetected,
                usePlugin);
            if (EditorGUI.EndChangeCheck())
            {
                Settings.UseDoozyEngine = usePlugin;
                if (!usePlugin)
                    Settings.UseEnhancedScroller = false;
            }
        }

        private void DrawGeneralPluginBoxEnhancedScroller()
        {
            bool usePlugin = Settings.UseEnhancedScroller;
            EditorGUI.BeginChangeCheck();
            usePlugin = DrawGeneralPluginBox(
                "EnhancedScroller",
                Settings.EnhancedScrollerDetected,
                usePlugin);
            if (EditorGUI.EndChangeCheck())
                Settings.UseEnhancedScroller = usePlugin;
        }

        private bool DrawGeneralPluginBox(string pluginName, bool hasPlugin, bool usePlugin)
        {
            bool value = usePlugin;
            GUILayout.BeginHorizontal();
            {
                GUI.enabled = !EditorApplication.isCompiling;
                GUILayout.BeginVertical();
                {
                    GUILayout.Label(pluginName, new GUIStyle(EditorStyles.boldLabel));
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();

                if (!hasPlugin)
                    GUI.enabled = false;

                Color GUIColor = GUI.color;
                GUI.backgroundColor = usePlugin ? Color.green : Color.red;
                if (GUILayout.Button(!hasPlugin ? "Not Installed" : usePlugin ? "Enabled" : "Disabled", new GUIStyle(EditorStyles.toolbarButton)))
                {
                    ControlySettings.Instance.UndoRecord(usePlugin ? "Disable Plugin" : "Enable Plugin");
                    ControlySettings.Instance.SetDirty(false);
                    m_NeedsSave = true;
                    m_NeedsToUpdateScriptDefineSymbols = true;
                    value = !value;
                }
                GUI.color = GUIColor;
            }
            GUILayout.EndHorizontal();
            GUI.enabled = true;
            return value;
        }
    }
}
