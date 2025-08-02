using UnityEngine;

public class SerializableChunkData
{
    public VoxelType[] Voxels;
    public int ChunkSize;
    public int ChunkHeight;
    public Vector3Int WorldPosition;

    public ChunkData ToChunkData(Map mapRef)
    {
        ChunkData data = new ChunkData(ChunkSize, ChunkHeight, mapRef, WorldPosition);
        data.SetVoxels(Voxels);
        return data;
    }
}
