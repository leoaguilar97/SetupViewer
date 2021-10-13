using UnityEngine;

namespace Assets.Scripts
{
    static class Logger
    {
        public static void Log(string message) {
            Debug.Log(message);
        }

        public static void Log(object message) {
            Log(message.ToString());
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        public static void LogError(object message)
        {
            Log(message.ToString());
        }
    }
}
