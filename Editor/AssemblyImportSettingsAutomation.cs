using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Compilation;

namespace NineHundredLbs.UIFramework.Editor
{
    [InitializeOnLoad]
    public static class AssemblyImportSettingsAutomation
    {
        /// <summary>
        /// Define Symbol for 900lbs UI Framework.
        /// </summary>
        public const string DEFINE_900LBSUIFRAMEWORK = "NINEHUNDREDLBS_UIFRAMEWORK";

        public const string RuntimeAssemblyDefinitionName = "900lbs.UIFramework";
        public const string DoozyRuntimeAssemblyDefinitionName = "Doozy.Engine";
        public const string EnhancedScrollerAssemblyDefinitionName = "EnhancedScroller";

        private static readonly StringBuilder StringBuilder = new StringBuilder();

        static AssemblyImportSettingsAutomation()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            Run();
        }

        private static void Run()
        {
            string runtimeAssemblyFilePath = CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(RuntimeAssemblyDefinitionName);
            try
            {
                UnityEditor.Compilation.Assembly doozyAssembly = null;
                string doozyAssemblyGUID = string.Empty;

                // Attempt to get the Doozy assembly.
                if (TryGetAssembly(DoozyRuntimeAssemblyDefinitionName, out doozyAssembly))
                    doozyAssemblyGUID = AssetDatabase.AssetPathToGUID(CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(doozyAssembly.name));
                else
                    throw new Exception("Unable to find a Doozy runtime assembly! Refer to the documentation in Packages/UI Framework/DOCUMENTATION to fix this issue.");

                // Attempt to get the EnhancedScroller assembly
                UnityEditor.Compilation.Assembly enhancedScrollerAssembly = null;
                string enhancedScrollerAssemblyGUID = string.Empty;
                if (TryGetAssembly(EnhancedScrollerAssemblyDefinitionName, out enhancedScrollerAssembly))
                    enhancedScrollerAssemblyGUID = AssetDatabase.AssetPathToGUID(CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(enhancedScrollerAssembly.name));
                else
                    throw new Exception("Unable to find an EnhancedScroller runtime assembly! Refer to the documentation in Packages/UI Framework/DOCUMENTATION to fix this issue.");

                File.WriteAllText(runtimeAssemblyFilePath, GetAssemblyDefinitionString(doozyAssemblyGUID, enhancedScrollerAssemblyGUID));
                AddGlobalDefine(DEFINE_900LBSUIFRAMEWORK);
            }
            catch (Exception e)
            {
                RemoveGlobalDefine(DEFINE_900LBSUIFRAMEWORK);
                UnityEngine.Debug.LogError(e.Message);
            }
        }

        private static string GetAssemblyDefinitionString(params string[] referencedAssemblyGUIDs)
        {
            StringBuilder.Length = 0;
            StringBuilder.Append("{\n");
            StringBuilder.Append("\"name\": " + "\"" + RuntimeAssemblyDefinitionName + "\",\n");
            StringBuilder.Append("\"references\": [\n");
            for (int i = 0; i < referencedAssemblyGUIDs.Length; i++)
            {
                StringBuilder.Append("\"GUID:" + referencedAssemblyGUIDs[i] + "\"");
                if (i != referencedAssemblyGUIDs.Length - 1)
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
            StringBuilder.Append("\"" + DEFINE_900LBSUIFRAMEWORK + "\"\n");
            StringBuilder.Append("],\n");
            StringBuilder.Append("\"versionDefines\": []\n");
            StringBuilder.Append("}");
            string assemblyDefinitionString = StringBuilder.ToString();
            StringBuilder.Length = 0;
            return assemblyDefinitionString;
        }

        private static bool TryGetAssembly(string assemblyName, out UnityEditor.Compilation.Assembly assembly)
        {
            foreach (var compiledAssembly in CompilationPipeline.GetAssemblies())
            {
                if (compiledAssembly.name == assemblyName)
                {
                    assembly = compiledAssembly;
                    return true;
                }
            }
            assembly = null;
            return false;
        }

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
    }
}
