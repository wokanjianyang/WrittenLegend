using System;
using Game;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build.Content;
using UnityEngine;
using Unity.Collections;

namespace ET
{
    public static class LeakDetectionControl
    {

        [MenuItem("MyProject/Jobs/Leak Detection")]
        private static void LeakDetection()
        {
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Enabled;
        }

        [MenuItem("MyProject/Jobs/Leak Detection With Stack Trace")]
        private static void LeakDetectionWithStackTrace()
        {
            NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
        }

        [MenuItem("MyProject/Jobs/No Leak Detection")]
        private static void NoLeakDetection()
        {
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }
    }
}
