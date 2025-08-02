using System.Collections.Generic;
using UnityEngine;

public class SerializableMapData
{
    public List<SerializableChunkData> Data = new();
    public string Name;

    public MapData ToMapData(Map mapRef)
    {
        Dictionary<Vector3Int, ChunkData> mapData = new();

        foreach (var data in Data)
        {
            mapData.Add(data.WorldPosition, data.ToChunkData(mapRef));
        }

        return new MapData(mapData, Name);
    }
}
