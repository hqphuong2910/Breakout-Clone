using System;
using System.IO;
using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public static class SaveSystem
    {
        public static void Save<T>(T data, string fileName)
        {
            try
            {
                var json = JsonUtility.ToJson(data);
                var path = Path.Combine(Application.persistentDataPath, fileName);
                File.WriteAllText(path, json);

                AppLogger.Log(nameof(SaveSystem), $"File has been saved to {path}.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(nameof(SaveSystem), $"Error occured while saving {fileName}: {e.Message}.");
            }
        }

        public static T Load<T>(string fileName) where T : new()
        {
            try
            {
                var path = Path.Combine(Application.persistentDataPath, fileName);
                if (!File.Exists(path)) return new T();

                var json = File.ReadAllText(path);

                AppLogger.Log(nameof(SaveSystem), $"Loaded {fileName} successful.");

                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                AppLogger.LogError(nameof(SaveSystem), $"Error occured while loading {fileName}: {e.Message}.");
                return new T();
            }
        }
    }
}