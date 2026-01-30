using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GGJ2026
{
    public static class GlobalLogger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogInfo(string message)
        {
            Debug.Log(message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogException(Exception e)
        {
            Debug.LogException(e);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogEditor(string s)
        {
            Debug.Log(s);
        }
    }
}