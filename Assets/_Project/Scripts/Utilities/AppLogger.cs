using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace _Project.Scripts.Utilities
{
    public static class AppLogger
    {
        public static bool LogEnabled { get; set; } = true;

        /// <summary>
        ///     <para>Logs a custom formatted message to the Unity console if log is enabled.</para>
        /// </summary>
        /// <param name="prefix">The prefix of the message.</param>
        /// <param name="message">The main log message.</param>
        /// <param name="prefixColor">The color of the prefix uses a HEX code.</param>
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEBUG")]
        public static void Log(string prefix, string message, string prefixColor = "#00FF00")
        {
            if (!LogEnabled) return;
            Debug.Log($"<color={prefixColor}><b>{prefix}</b></color>: {message}");
        }

        /// <summary>
        ///     <para>A variant of AppLogger.Log that logs a warning message to the console.</para>
        /// </summary>
        /// <param name="prefix">The prefix of the message.</param>
        /// <param name="message">The main log message.</param>
        /// <param name="prefixColor">The color of the prefix uses a HEX code.</param>
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEBUG")]
        public static void LogWarning(string prefix, string message, string prefixColor = "#FFCC00")
        {
            if (!LogEnabled) return;
            Debug.LogWarning($"<color={prefixColor}><b>{prefix}</b></color>: {message}");
        }

        /// <summary>
        ///     <para>A variant of AppLogger.Log that logs an error message to the console.</para>
        /// </summary>
        /// <param name="prefix">The prefix of the message.</param>
        /// <param name="message">The main log message.</param>
        /// <param name="prefixColor">The color of the prefix uses a HEX code.</param>
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEBUG")]
        public static void LogError(string prefix, string message, string prefixColor = "#FF0000")
        {
            Debug.LogError($"<color={prefixColor}><b>{prefix}</b></color>: {message}");
        }
    }
}