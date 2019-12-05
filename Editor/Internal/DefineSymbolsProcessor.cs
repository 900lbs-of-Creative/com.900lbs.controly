using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace NineHundredLbs.Controly.Editor
{
    [InitializeOnLoad]
    public static class DefineSymbolsProcessor
    {
        /// <summary>
        /// Define Symbol for Controly
        /// </summary>
        private const string DEFINE_UIFRAMEWORK_UI = "CONTROLY_UI";

        /// <summary>
        /// Define Symbol for Controly EnhancedScroller module.
        /// </summary>
        private const string DEFINE_CONTROLY_SCROLLER = "CONTROLY_UI_SCROLLER";

        /// <summary>
        /// Namespace for Doozy Engine.
        /// </summary>
        private const string NAMESPACE_DOOZY_ENGINE = "Doozy.Engine";

        /// <summary>
        /// Namespace for EnhancedScroller.
        /// </summary>
        private const string NAMESPACE_ENHANCEDSCROLLER = "EnhancedUI";

        private static ControlySettings Settings => ControlySettings.Instance;
        private static List<Assembly> m_Assemblies = new List<Assembly>();
        private static bool m_SaveAssets;

        private static readonly StringBuilder StringBuilder = new StringBuilder();

        static DefineSymbolsProcessor()
        {
            ExecuteProcessor();
        }

        public static void ExecuteProcessor()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (!ProcessorsSettings.Instance.RunDefineSymbolsProcessor)
                return;

            UpdateAssemblies();
            UpdateInstalledPlugins();
            UpdateAssemblyDefinitions();
            if (m_SaveAssets)
                AssetDatabase.SaveAssets();
            UpdateScriptingDefineSymbols();

            ProcessorsSettings.Instance.RunDefineSymbolsProcessor = false;
            ProcessorsSettings.Instance.SetDirty(true);
        }

        private static void UpdateAssemblies()
        {
            m_Assemblies.Clear();
            m_Assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        }

        private static void UpdateInstalledPlugins()
        {
            bool saveAssets = false;

            // DoozyUI - Doozy.Engine
            bool hasDoozyEngine = NamespaceExists(NAMESPACE_DOOZY_ENGINE);
            if (Settings.DoozyEngineDetected != hasDoozyEngine)
            {
                Settings.DoozyEngineDetected = hasDoozyEngine;
                saveAssets = true;
            }

            // EnhancedScroller - EnhancedUI
            bool hasEnhancedScroller = NamespaceExists(NAMESPACE_ENHANCEDSCROLLER);
            if (Settings.EnhancedScrollerDetected != hasEnhancedScroller)
            {
                Settings.EnhancedScrollerDetected = hasEnhancedScroller;
                saveAssets = true;
            }

            if (!saveAssets)
                return;

            Settings.SetDirty(false);
            m_SaveAssets = true;
        }

        private static void UpdateAssemblyDefinitions()
        {
            if (Settings.DoozyEngineDetected)
            {
                string doozyAssemblyName;
                if (TryGetAssemblyByNamespace(NAMESPACE_DOOZY_ENGINE, out doozyAssemblyName))
                {
                    List<string> referencedAssemblies = new List<string>();
                    referencedAssemblies.Add(ControlyPath.ASMDEF_CONTROLY);
                    referencedAssemblies.Add(doozyAssemblyName);

                    List<string> defineConstraints = new List<string>();
                    defineConstraints.Add(DEFINE_UIFRAMEWORK_UI);

                    File.WriteAllText(Path.Combine(ControlyPath.RUNTIME_UI_PATH, ControlyPath.ASMDEF_CONTROLY_UI + ".asmdef"), 
                        GetAssemblyDefinitionString(ControlyPath.ASMDEF_CONTROLY_UI, referencedAssemblies, defineConstraints));
                }
            }

            if (Settings.EnhancedScrollerDetected)
            {
                string enhancedScrollerAssemblyName;
                if (TryGetAssemblyByNamespace(NAMESPACE_ENHANCEDSCROLLER, out enhancedScrollerAssemblyName))
                {
                    List<string> referencedAssemblies = new List<string>();
                    referencedAssemblies.Add(ControlyPath.ASMDEF_CONTROLY);
                    referencedAssemblies.Add(ControlyPath.ASMDEF_CONTROLY_UI);
                    referencedAssemblies.Add(enhancedScrollerAssemblyName);

                    List<string> defineConstraints = new List<string>();
                    defineConstraints.Add(DEFINE_CONTROLY_SCROLLER);

                    File.WriteAllText(Path.Combine(ControlyPath.RUNTIME_UI_VIEW_SCROLLER_PATH, ControlyPath.ASMDEF_CONTROLY_UI_VIEW_SCROLLER + ".asmdef"), 
                        GetAssemblyDefinitionString(ControlyPath.ASMDEF_CONTROLY_UI_VIEW_SCROLLER, referencedAssemblies, defineConstraints));
                }
            }
            m_SaveAssets = true;
        }

        private static void UpdateScriptingDefineSymbols()
        {
            if (Settings.DoozyEngineDetected)
                AddGlobalDefine(DEFINE_UIFRAMEWORK_UI);
            else
                RemoveGlobalDefine(DEFINE_UIFRAMEWORK_UI);

            if (Settings.EnhancedScrollerDetected)
                AddGlobalDefine(DEFINE_CONTROLY_SCROLLER);
            else
                RemoveGlobalDefine(DEFINE_CONTROLY_SCROLLER);
        }

        private static bool TryGetAssemblyByNamespace(string nameSpace, out string assemblyName)
        {
            assemblyName = "";
            foreach (Assembly assembly in m_Assemblies)
            {
                if (assembly == null)
                    continue;

                Type[] typesInAsm;
                try
                {
                    typesInAsm = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInAsm = ex.Types.Where(t => t != null).ToArray();
                }

                if (typesInAsm.Any(type => type.Namespace == nameSpace))
                {
                    assemblyName = assembly.GetName().Name;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// https://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx/
        /// </summary>
        /// <param name="target">Name of the target namespace.</param>
        /// <returns>Whether or not the <paramref name="nameSpace"/> exists in the currently loaded assemblies.</returns>
        private static bool NamespaceExists(string nameSpace)
        {
            if (m_Assemblies == null || m_Assemblies.Count == 0)
            {
                UpdateAssemblies();
                if (m_Assemblies == null || m_Assemblies.Count == 0)
                    return false;
            }

            foreach (Assembly assembly in m_Assemblies)
            {
                if (assembly == null) continue;
                Type[] typesInAsm;
                try
                {
                    typesInAsm = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInAsm = ex.Types.Where(t => t != null).ToArray();
                }

                if (typesInAsm.Any(type => type.Namespace == nameSpace))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a global define symbol for all <see cref="BuildTargetGroup"/>s with the given string <paramref name="id"/>.
        /// </summary>
        /// <remarks>
        /// All credit to the Doozy framework for this solution.
        /// </remarks>
        /// <param name="id">The string value to add as a global define symbol.</param>
        private static void AddGlobalDefine(string id)
        {
            bool flag = false;
            int num = 0;
            foreach (BuildTargetGroup buildTargetGroup in (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup)))
                if (IsValidBuildTargetGroup(buildTargetGroup))
                {
                    string defineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                    if (Array.IndexOf(defineSymbolsForGroup.Split(';'), id) != -1) continue;
                    flag = true;
                    ++num;
                    string defines = defineSymbolsForGroup + (defineSymbolsForGroup.Length > 0 ? ";" + id : id);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
                }

            if (!flag) return;
            UnityEngine.Debug.Log(string.Format("Added global define \"{0}\" to {1} BuildTargetGroups", id, num));
        }

        /// <summary>
        /// Removes the global define symbol for all <see cref="BuildTargetGroup"/>s with the given string <paramref name="id"/>.
        /// </summary>
        /// <remarks>
        /// All credit to the Doozy framework for this solution.
        /// </remarks>
        /// <param name="id">The string value to be removed.</param>
        private static void RemoveGlobalDefine(string id)
        {
            bool flag = false;
            int num = 0;
            foreach (BuildTargetGroup buildTargetGroup in (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup)))
                if (IsValidBuildTargetGroup(buildTargetGroup))
                {
                    string[] array = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
                    if (Array.IndexOf(array, id) == -1) continue;
                    flag = true;
                    ++num;
                    StringBuilder.Length = 0;
                    foreach (string t in array)
                        if (t != id)
                        {
                            if (StringBuilder.Length > 0) StringBuilder.Append(';');
                            StringBuilder.Append(t);
                        }

                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, StringBuilder.ToString());
                }

            StringBuilder.Length = 0;
            if (!flag) return;
            UnityEngine.Debug.Log(string.Format("Removed global define \"{0}\" from {1} BuildTargetGroups", id, num));
        }

        /// <summary>
        /// Checks if the given <paramref name="group"/> is valid.
        /// </summary>
        /// <param name="group">The <see cref="BuildTargetGroup"/> to be checked.</param>
        /// <remarks>
        /// All credit to the Doozy framework for this solution.
        /// </remarks>
        /// <returns>Whether or not the given <paramref name="group"/> is valid.</returns>
        private static bool IsValidBuildTargetGroup(BuildTargetGroup group)
        {
            if (group == BuildTargetGroup.Unknown) return false;
            Type unityEditorModuleManagerType = Type.GetType("UnityEditor.Modules.ModuleManager, UnityEditor.dll");
            if (unityEditorModuleManagerType == null) return true;
            MethodInfo method1 = unityEditorModuleManagerType.GetMethod("GetTargetStringFromBuildTargetGroup", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo method2 = typeof(PlayerSettings).GetMethod("GetPlatformName", BindingFlags.Static | BindingFlags.NonPublic);
            var parameters = new object[] { group };
            if (method1 == null) return true;
            string str1 = (string)method1.Invoke(null, parameters);
            if (method2 == null) return true;
            string str2 = (string)method2.Invoke(null, new object[] { group });
            if (string.IsNullOrEmpty(str1)) return !string.IsNullOrEmpty(str2);
            return true;
        }

        /// <summary>
        /// Constructs and returns a string to be written to an assembly definition file with the given 
        /// <paramref name="referencedAssemblies"/>.
        /// </summary>
        /// <param name="referencedAssemblies">The assemblies to include in the assembly definition as referenced.</param>
        /// <returns>A string representing an assembly definition.</returns>
        private static string GetAssemblyDefinitionString(string name, List<string> referencedAssemblies, List<string> defineConstraints)
        {
            StringBuilder.Length = 0;
            StringBuilder.Append("{\n");
            StringBuilder.Append("\"name\": " + "\"" + name + "\",\n");
            StringBuilder.Append("\"references\": [\n");
            for (int i = 0; i < referencedAssemblies.Count; i++)
            {
                StringBuilder.Append("\"" + referencedAssemblies[i] + "\"");
                if (i != referencedAssemblies.Count - 1)
                    StringBuilder.Append(",");
                StringBuilder.Append("\n");
            }
            StringBuilder.Append("],\n");
            StringBuilder.Append("\"includePlatforms\": [],\n");
            StringBuilder.Append("\"excludePlatforms\": [],\n");
            StringBuilder.Append("\"allowUnsafeCode\": false,\n");
            StringBuilder.Append("\"overrideReferences\": false,\n");
            StringBuilder.Append("\"precompiledReferences\": [],\n");
            StringBuilder.Append("\"autoReferenced\": true,\n");
            StringBuilder.Append("\"defineConstraints\": [\n");
            for (int i = 0; i < defineConstraints.Count; i++)
            {
                StringBuilder.Append("\"" + defineConstraints[i] + "\"");
                if (i != defineConstraints.Count - 1)
                    StringBuilder.Append(",");
                StringBuilder.Append("\n");
            }
            StringBuilder.Append("],\n");
            StringBuilder.Append("\"versionDefines\": []\n");
            StringBuilder.Append("}");
            string assemblyDefinitionString = StringBuilder.ToString();
            StringBuilder.Length = 0;
            return assemblyDefinitionString;
        }
    }
}
