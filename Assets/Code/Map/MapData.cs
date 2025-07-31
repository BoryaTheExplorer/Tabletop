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
}
