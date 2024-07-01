using System;
using Game;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build.Content;
using UnityEngine;

namespace ET
{
    public static class BuildHelper
    {


        [InitializeOnLoadMethod]
        public static void ReGenerateProjectFiles()
        {
            if (Unity.CodeEditor.CodeEditor.CurrentEditor.GetType().Name == "RiderScriptEditor")
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
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            PlayerSettings.applicationIdentifier = "com.fulljoblegend.android"; //com.lengend.test
            PlayerSettings.Android.useCustomKeystore = false;
            var opa = BuildOptions.CompressWithLz4HC | BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler | BuildOptions.EnableDeepProfilingSupport;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            BuildHelper.Build(BuildType.Release, PlatformType.Android, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, opa, true, true, true, "测试版");
        }

        [MenuItem("开发工具/生成正式包Taptap版")]
        public static void BuildRelease()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.applicationIdentifier = "com.fulljoblegend.android";
            PlayerSettings.Android.useCustomKeystore = true;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, "IS_TAPTAP"); //ODIN_INSPECTOR;EASY_MOBILE;EASY_MOBILE_PRO;

            var opa = BuildOptions.CompressWithLz4HC;

            BuildHelper.Build(BuildType.Release, PlatformType.Android, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, opa, true, true, true, "Taptap版", true);
        }
        [MenuItem("开发工具/生成正式包QQ版")]
        public static void BuildReleaseQQ()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.applicationIdentifier = "com.fulljoblegend.android";
            PlayerSettings.Android.useCustomKeystore = true;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            var opa = BuildOptions.CompressWithLz4HC;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, "");

            BuildHelper.Build(BuildType.Release, PlatformType.Android, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, opa, true, true, true, "QQ版", true);
        }


        [MenuItem("开发工具/生成正式包Taptap版和兼容版")]
        public static void BuildReleaseAll()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.applicationIdentifier = "com.fulljoblegend.android";
            PlayerSettings.Android.useCustomKeystore = true;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            var opa = BuildOptions.CompressWithLz4HC;

            var buildSuccess = BuildHelper.Build(BuildType.Release, PlatformType.Android, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, opa, true, true, true, "Taptap版", false);

            if (buildSuccess)
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
                PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25;
                PlayerSettings.applicationIdentifier = "com.fulljoblegend.android";
                PlayerSettings.Android.useCustomKeystore = true;
                EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
                opa = BuildOptions.CompressWithLz4HC;

                BuildHelper.Build(BuildType.Release, PlatformType.Android, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, opa, true, true, true, "兼容版");
            }
        }
        public static bool Build(BuildType buildType, PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, bool clearFolder, string ext = "", bool isAddVersionNum = true)
        {
            var ret = false;
            try
            {
                string name = "全职龙城";

                //if (buildType == BuildType.Debug)
                //{
                //    name += "-测试";
                //}

                PlayerSettings.productName = name;

                BuildTarget buildTarget = BuildTarget.StandaloneWindows;
                string programName = $"{name}.{PlayerSettings.bundleVersion}";
                if (!string.IsNullOrEmpty(ext))
                {
                    programName += $".{ext}";
                }
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

                    AssetDatabase.Refresh();
                    string[] levels = {
                        "Assets/Scenes/Init.unity",
                    };
                    UnityEngine.Debug.Log("start build exe");
                    BuildPipeline.BuildPlayer(levels, $"../{buildType.ToString()}/{exeName}", buildTarget, buildOptions);

                    string sds = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                    Debug.Log("sds:" + sds);
                    //PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, "");
                    Debug.Log("reset sds:" + sds);

                    UnityEngine.Debug.Log("finish build exe");
                    ret = true;
                    if (buildType == BuildType.Release && isAddVersionNum)
                    {
                        //PlayerSettings.Android.bundleVersionCode++;
                        //PlayerSettings.bundleVersion = string.Join(".", PlayerSettings.Android.bundleVersionCode.ToString().PadLeft(3, '0').ToCharArray());
                    }

                }

            }
            catch (Exception e)
            {
                ret = false;
                UnityEngine.Debug.LogError(e);
            }

            return ret;
        }
    }
}
