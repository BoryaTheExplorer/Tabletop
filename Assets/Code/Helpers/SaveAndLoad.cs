using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class SaveAndLoad
{
    private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
    {
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
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        Debug.LogWarning($"No Save file found: {fileName}");
        return default;
    }

    public static bool SaveFileExists(string folderPath, string fileName)
    {
        return File.Exists(GetFilePath(folderPath, fileName));
    }
}
