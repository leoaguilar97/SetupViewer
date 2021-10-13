using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    static class Logger
    {
        static readonly List<string> lines = new List<string>();
        static readonly string ERROR = "[ERROR ] {0} :::> {1}";
        static readonly string ACCION = "[ACCION] {0} :::> {1}";
        static string start = null;

        public static void CrearBitacora()
        {
            string destination = UnityEngine.Application.persistentDataPath + "/bitacora_201603029_201602797.txt";

            Log("Creando bitacora en " + destination);
            FileStream file;

            if (File.Exists(destination))
            {
                file = File.OpenWrite(destination);
            }
            else
            {
                file = File.Create(destination);
            }
            try
            {
                file.Seek(0, SeekOrigin.End);
                using StreamWriter sw = new StreamWriter(file);
                sw.WriteLine("* INICIO DE BITACORA EN :: " + start);
                foreach (string line in lines)
                {
                    sw.WriteLine(line);
                }

                sw.WriteLine("* FIN DE BITACORA EN :: " + DateTime.Now.ToString());
                sw.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("Error escribiendo bitacora");
                Debug.LogError(e.Message);
            }

            lines.Clear();
        }

        public static void Log(string message, bool showMessage = false) {

            if (start == null)
            {
                start = DateTime.Now.ToString();
            }
            Debug.Log(message);
            lines.Add(string.Format(ACCION, DateTime.Now, message));


            if (showMessage)
            {
                EditorUtility.DisplayDialog("Información", message, "¡Entendido!");
            }
        }

        public static void Log(object message) {
            Log(message.ToString());
        }

        public static void LogError(string message, bool showMessage = false)
        {
            if (start == null)
            {
                start = DateTime.Now.ToString();
            }
            Debug.LogError(message);
            lines.Add(string.Format(ERROR, DateTime.Now, message));

            if (showMessage)
            {
                EditorUtility.DisplayDialog("Error", message, "Ok");
            }
        }

        public static void LogError(object message)
        {
            Log(message.ToString());
        }
    }
}
