using System;
using Game;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class BuildHelper
    {


        [InitializeOnLoadMethod]
        public static void ReGenerateProjectFiles()
        {
            if (Unity.CodeEditor.CodeEditor.CurrentEditor.GetType().Name== "RiderScriptEditor")
            {
                FieldInfo generator = Unity.CodeEditor.CodeEditor.CurrentEditor.GetType().GetField("m_ProjectGeneration", BindingFlags.Static | BindingFlags.NonPublic);
                var syncMethod = generator.FieldType.GetMethod("Sync");
                syncMethod.Invoke(generator.GetValue(Unity.CodeEditor.CodeEditor.CurrentEditor), null);
            }
            else
            {
                Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
            }
            
            Debug.Log("ReGenerateProjectFiles finished.");
        }

        
#if ENABLE_CODES
        [MenuItem("ET/ChangeDefine/Remove ENABLE_CODES")]
        public static void RemoveEnableCodes()
        {
            EnableCodes(false);
        }
#else
        //[MenuItem("ET/ChangeDefine/Add ENABLE_CODES")]
        public static void AddEnableCodes()
        {
            EnableCodes(true);
        }
#endif
        private static void EnableCodes(bool enable)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var ss = defines.Split(';').ToList();
            if (enable)
            {
                if (ss.Contains("ENABLE_CODES"))
                {
                    return;
                }
                ss.Add("ENABLE_CODES");
            }
            else
            {
                if (!ss.Contains("ENABLE_CODES"))
                {
                    return;
                }
                ss.Remove("ENABLE_CODES");
            }
            defines = string.Join(";", ss);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            AssetDatabase.SaveAssets();
        }
        
#if ENABLE_VIEW
        [MenuItem("ET/ChangeDefine/Remove ENABLE_VIEW")]
        public static void RemoveEnableView()
        {
            EnableView(false);
        }
#else
        //[MenuItem("ET/ChangeDefine/Add ENABLE_VIEW")]
        public static void AddEnableView()
        {
            EnableView(true);
        }
#endif
        private static void EnableView(bool enable)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var ss = defines.Split(';').ToList();
            if (enable)
            {
                if (ss.Contains("ENABLE_VIEW"))
                {
                    return;
                }
                ss.Add("ENABLE_VIEW");
            }
            else
            {
                if (!ss.Contains("ENABLE_VIEW"))
                {
                    return;
                }
                ss.Remove("ENABLE_VIEW");
            }
            
            defines = string.Join(";", ss);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("开发工具/生成测试包32位")]
        public static void BuildDebug()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            
            var opa = BuildOptions.CompressWithLz4HC | BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler | BuildOptions.EnableDeepProfilingSupport;

            BuildHelper.Build(BuildType.Release,PlatformType.Android, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, opa, true, true, true);
        }

        [MenuItem("开发工具/生成正式包64位")]
        public static void BuildRelease()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

            var opa = BuildOptions.CompressWithLz4HC;

            BuildHelper.Build(BuildType.Release,PlatformType.Android, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, opa, true, true, true);
        }

        public static void Build(BuildType buildType,PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, bool clearFolder)
        {
            
            try
            {
                BuildTarget buildTarget = BuildTarget.StandaloneWindows;
                PlayerSettings.bundleVersion = string.Join(".", PlayerSettings.Android.bundleVersionCode.ToString().PadLeft(3,'0').ToCharArray());
                string programName = $"全职传奇.{buildType.ToString()}.{PlayerSettings.bundleVersion}";
                string exeName = programName;
                switch (type)
                {
                    case PlatformType.Windows:
                        buildTarget = BuildTarget.StandaloneWindows64;
                        exeName += ".exe";
                        break;
                    case PlatformType.Android:
                        buildTarget = BuildTarget.Android;
                        exeName += ".apk";
                        break;
                    case PlatformType.IOS:
                        buildTarget = BuildTarget.iOS;
                        break;
                    case PlatformType.MacOS:
                        buildTarget = BuildTarget.StandaloneOSX;
                        break;
                    
                    case PlatformType.Linux:
                        buildTarget = BuildTarget.StandaloneLinux64;
                        break;
                }

                string fold = $"../{buildType.ToString()}/{type}/StreamingAssets/";

                if (clearFolder && Directory.Exists(fold))
                {
                    Directory.Delete(fold, true);
                }
                Directory.CreateDirectory(fold);

                UnityEngine.Debug.Log("start build assetbundle");
                BuildPipeline.BuildAssetBundles(fold, buildAssetBundleOptions, buildTarget);

                UnityEngine.Debug.Log("finish build assetbundle");

                if (isContainAB)
                {
                    FileHelper.CleanDirectory("Assets/StreamingAssets/");
                    FileHelper.CopyDirectory(fold, "Assets/StreamingAssets/");
                }

                if (isBuildExe)
                {
                    PlayerSettings.Android.bundleVersionCode++;

                    AssetDatabase.Refresh();
                    string[] levels = {
                        "Assets/Scenes/Init.unity",
                    };
                    UnityEngine.Debug.Log("start build exe");
                    BuildPipeline.BuildPlayer(levels, $"../{buildType.ToString()}/{exeName}", buildTarget, buildOptions);
                    UnityEngine.Debug.Log("finish build exe");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
