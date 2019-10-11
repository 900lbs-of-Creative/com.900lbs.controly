using System;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;

[InitializeOnLoad]
public static class AssemblyImportSettingsAutomation
{
    public static readonly string RuntimeAssemblyDefinitionName = "900lbs.UIFramework";
    public static readonly string DoozyRuntimeAssemblyDefinitionName = "Doozy.Engine";
    public static readonly string EnhancedScrollerAssemblyDefinitionName = "EnhancedScroller";

    static AssemblyImportSettingsAutomation()
    {
        string runtimeAssemblyFilePath = CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(RuntimeAssemblyDefinitionName);
        try
        {
            using (FileStream fileStream = new FileStream(runtimeAssemblyFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                Assembly doozyAssembly = null;
                string doozyAssemblyGUID = string.Empty;
                if (TryGetAssembly(DoozyRuntimeAssemblyDefinitionName, out doozyAssembly))
                {
                    doozyAssemblyGUID = AssetDatabase.AssetPathToGUID(CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(doozyAssembly.name));
                }
                else
                    throw new Exception("Unable to find a Doozy runtime assembly! Refer to the documentation in Packages/UI Framework/DOCUMENTATION to fix this issue.");

                Assembly enhancedScrollerAssembly = null;
                string enhancedScrollerAssemblyGUID = string.Empty;
                if (TryGetAssembly(EnhancedScrollerAssemblyDefinitionName, out enhancedScrollerAssembly))
                {
                    enhancedScrollerAssemblyGUID = AssetDatabase.AssetPathToGUID(CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(enhancedScrollerAssembly.name));
                }
                else
                    throw new Exception("Unable to find an EnhancedScroller runtime assembly! Refer to the documentation in Packages/UI Framework/DOCUMENTATION to fix this issue.");

                StreamWriter streamWriter = new StreamWriter(fileStream);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("{\n");
                stringBuilder.Append("\"name\": " + "\"" + RuntimeAssemblyDefinitionName + "\",\n");
                stringBuilder.Append("\"references\": [\n");
                stringBuilder.Append("GUID:" + doozyAssemblyGUID + ",\n");
                stringBuilder.Append("GUID:" + enhancedScrollerAssemblyGUID + "\n");
                stringBuilder.Append("],\n");
                stringBuilder.Append("\"includePlatforms\": [],\n");
                stringBuilder.Append("\"excludePlatforms\": [],\n");
                stringBuilder.Append("\"allowUnsafeCode\": false,\n");
                stringBuilder.Append("\"overrideReferences\": false,\n");
                stringBuilder.Append("\"precompiledReferences\": [],\n");
                stringBuilder.Append("\"autoReferenced\": true,\n");
                stringBuilder.Append("\"defineConstraints\": [\n");
                stringBuilder.Append("\"dUI_MANAGER\",\n");
                stringBuilder.Append("\"ODIN_INSPECTOR\"\n");
                stringBuilder.Append("],\n");
                stringBuilder.Append("\"versionDefines\": []\n");
                stringBuilder.Append("}");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }
    }

    private static bool TryGetAssembly(string assemblyName, out Assembly assembly)
    {
        foreach (Assembly compiledAssembly in CompilationPipeline.GetAssemblies())
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
}
