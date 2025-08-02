using UnityEngine;

public class ChunkData
{
    public VoxelType[] Voxels {  get; private set; }
    
    public int ChunkSize { get; private set; } = 16;
    public int ChunkHeight { get; private set; } = 50;
    
    public Vector3Int WorldPosition { get; private set; }

    public Map MapReference { get; private set; }

    public bool Modified { get; set; } = false;
    public ChunkData(int chunkSize, int chunkHeight, Map map, Vector3Int worldPosition)
    {
        ChunkSize = chunkSize;
        ChunkHeight = chunkHeight;
        MapReference = map;
        WorldPosition = worldPosition;

        InitChunkVoxelData();
    }

    public void InitChunkVoxelData()
    {
        Voxels = new VoxelType[ChunkSize * ChunkSize * ChunkHeight];
    }
    public void SetVoxels(VoxelType[] voxels)
    {
        Voxels = voxels;
    }
}
