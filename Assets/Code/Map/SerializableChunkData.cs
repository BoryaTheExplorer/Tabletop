using System;
using System.IO;
using UnityEngine;

[Serializable]
public struct SerializableChunkData
{
    public VoxelType[] Voxels;
    public Vector3Int WorldPosition;

    public SerializableChunkData(ChunkData chunkData)
    {
        Voxels = chunkData.Voxels;
        WorldPosition = chunkData.WorldPosition;
    }

    public ChunkData ToChunkData(Map mapRef)
    {
        ChunkData data = new ChunkData(mapRef.ChunkSize, mapRef.ChunkHeight, mapRef, WorldPosition);
        data.SetVoxels(Voxels);
        return data;
    }

    public byte[] SerializeToBytes(int chunkSize, int chunkHeight)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    writer.Write((int)Voxels[Chunk.GetIndexFromPosition(chunkSize, chunkHeight, x, y, z)]);
                }
            }
        }

        writer.Write(WorldPosition.x);
        writer.Write(WorldPosition.y);
        writer.Write(WorldPosition.z);

        writer.Flush();

        return ms.ToArray();
    }

    public static SerializableChunkData DeserializeFromBytes(byte[] data, int chunkSize, int chunkHeight)
    {
        var chunk = new SerializableChunkData();
        chunk.Voxels = new VoxelType[chunkSize * chunkSize * chunkHeight];

        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
    
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0;y < chunkHeight; y++)
            {
                for (int z = 0;z < chunkSize; z++)
                {
                    chunk.Voxels[Chunk.GetIndexFromPosition(chunkSize, chunkHeight, x, y, z)] = (VoxelType)reader.ReadInt32();
                }
            }
        }

        int cx = reader.ReadInt32();
        int cy = reader.ReadInt32();
        int cz = reader.ReadInt32();

        chunk.WorldPosition = new Vector3Int(cx, cy, cz);

        return chunk;
    }
}
