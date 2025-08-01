using System.Collections.Generic;
using UnityEngine;

public static class MapRegister
{
    public static Map Map { get; private set; }
    public static Dictionary<string, MapData> SavedMaps { get; private set; } = new();

    public static void SetMap(Map map)
    {
        if (Map == null)
            Map = map;
    }
    public static MapData GetMapData(string name)
    {
        MapData mapData;
        
        if (SavedMaps.TryGetValue(name, out mapData))
            Debug.Log($"Map '{name}' is missing in the Register.");

        return mapData;
    }
    public static void LoadMapData(string name)
    {
        MapData data = GetMapData(name);

        if (data == null)
            return;

        Map.LoadMapLayout(data);
    }
    public static bool RegisterMapData(MapData mapData)
    {
        if (mapData == null || mapData.ChunkDataDicitonary.Count == 0)
        {
            Debug.Log("Map Data is empty");
            return false;
        }

        if (SavedMaps.ContainsKey(mapData.Name))
        {
            Debug.Log($"Map with name '{mapData.Name}' is laready registered");
            return false;
        }
        
        SavedMaps.Add(mapData.Name, mapData);
        return true;
    }
    public static bool RemoveMapData(string name)
    {
        return SavedMaps.Remove(name);
    }
}
