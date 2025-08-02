using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveAndLoad
{
    private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Converters = new List<JsonConverter> { new Vector3IntConverter() },
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented
    };
    private static string GetFullFolderPath(string folderName)
    {
        return Path.Combine(Application.persistentDataPath, folderName);
    }
    private static string GetFilePath(string folderName, string fileName)
    {
        return Path.Combine(GetFullFolderPath(folderName), fileName + ".json");
    }

    public static void Save<T>(T data, string folderName, string fileName)
    {
        if (!Directory.Exists(GetFullFolderPath(folderName))) 
        {
            Directory.CreateDirectory(GetFullFolderPath(folderName));
        }

        string json = JsonConvert.SerializeObject(data, settings);
        File.WriteAllText(GetFilePath(folderName, fileName), json);
    }
    public static T Load<T>(string folderName, string fileName)
    {
        string path = GetFilePath(folderName, fileName);
        if (SaveFileExists(folderName, fileName))
        {
            string json = File.ReadAllText(path);
            T result = JsonConvert.DeserializeObject<T>(json, settings);
            return result;
        }

        Debug.LogWarning($"No Save file found: {fileName}");
        return default;
    }
    public static List<T> LoadAll<T>(string folderName)
    {
        string directory = GetFullFolderPath(folderName);
        if (!Directory.Exists(directory))
            return default;

        List<T> list = new List<T>();

        string[] paths = Directory.GetFiles(directory, "*.json");
        foreach (string path in paths)
        {
            try
            {
                string json = File.ReadAllText(path);
                T data = JsonConvert.DeserializeObject<T>(json, settings);
                list.Add(data);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        return list;
    }

    public static bool SaveFileExists(string folderPath, string fileName)
    {
        return File.Exists(GetFilePath(folderPath, fileName));
    }
}
