using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public Dictionary<Vector3Int, ChunkData> ChunkDataDicitonary { get; private set; } = new();
    public string Name { get; private set; }

    public MapData(Dictionary<Vector3Int, ChunkData> chunkDataDicitonary, string name)
    {
        ChunkDataDicitonary = chunkDataDicitonary;
        Name = name;
    }
    public MapData(MapData mapData)
    {
        ChunkDataDicitonary = new Dictionary<Vector3Int, ChunkData>(mapData.ChunkDataDicitonary);
        Name = mapData.Name;
    }
}
