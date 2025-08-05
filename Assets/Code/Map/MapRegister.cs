using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class MapRegister
{
    public static Map Map { get; private set; }
    public static NetworkMap NetworkMap { get; set; }
    public static Dictionary<string, MapData> SavedMaps { get; private set; } = new();

    public static void Init(Map map)
    {
        if (Map != null)
            return;

        Map = map;
        if (NetworkManager.Singleton.IsServer)
            LoadAllMaps();
    }
    public static void LoadAllMaps()
    {
        List<SerializableMapData> maps = SaveAndLoad.LoadAll<SerializableMapData>("Maps");

        if (maps.Count == 0)
            return;

        MapData data;
        foreach (SerializableMapData mapData in maps)
        {
            data = mapData.ToMapData(Map);
            SavedMaps.Add(data.Name, data);
        }
    }
    public static MapData GetMapData(string name)
    {
        MapData mapData;
        
        if (!SavedMaps.TryGetValue(name, out mapData))
            Debug.Log($"Map '{name}' is missing in the Register.");

        return new MapData(mapData);
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
        
        List<SerializableChunkData> data = new();
        foreach (var chunkData in mapData.ChunkDataDicitonary.Values)
        {
            data.Add(new SerializableChunkData()
            {
                Voxels = chunkData.Voxels,
                WorldPosition = chunkData.WorldPosition,
            });
        }

        SaveAndLoad.Save<SerializableMapData>(new SerializableMapData()
        {
            Data = data,
            Name = mapData.Name
        }, 
        $"Maps", $"{mapData.Name}_MapData");
        return true;
    }
    public static bool RemoveMapData(string name)
    {
        return SavedMaps.Remove(name);
    }
}
