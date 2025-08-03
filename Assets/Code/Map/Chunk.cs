using System;
using UnityEngine;

public static class Chunk
{
    public static Vector3Int GetPositionFromIndex(ChunkData data, int index)
    {
        int x = index % data.ChunkSize;
        int y = (index / data.ChunkSize) % data.ChunkHeight;
        int z = index / (data.ChunkSize * data.ChunkHeight);

        return new Vector3Int(x, y, z);
    }

    private static bool InRange(ChunkData data, int index)
    {
        if (index < 0 || index >= data.ChunkSize)
            return false;

        return true;
    }
    private static bool InRangeHeight(ChunkData data, int index)
    {
        if (index < 0 || index >= data.ChunkHeight)
            return false;

        return true;
    }
    public static VoxelType GetVoxelFromChunkCoordinates(ChunkData data, int x, int y, int z)
    {
        if (InRange(data, x) && InRange(data, z) && InRangeHeight(data, y))
        {
            int index = GetIndexFromPosition(data, x, y, z);
            return data.Voxels[index];
        }

        return data.MapReference.GetVoxelFromChunkCoordinates(data, data.WorldPosition.x + x,
                                                                    data.WorldPosition.y + y,
                                                                    data.WorldPosition.z + z);
    }
    public static bool SetVoxel(ChunkData data, Vector3Int localPosition, VoxelType voxel)
    {
        if (!InRange(data, localPosition.x) || !InRange(data, localPosition.z) || !InRangeHeight(data, localPosition.y))
        {
            Debug.Log("Failed to Set a Voxel");
            return false;
        }

        int index = GetIndexFromPosition(data, localPosition.x, localPosition.y, localPosition.z);
        data.Voxels[index] = voxel;
        return true;
    }

    public static int GetIndexFromPosition(ChunkData data, int x, int y, int z)
    {
        return x + data.ChunkSize * y + data.ChunkSize * data.ChunkHeight * z;
    }
    public static int GetIndexFromPosition(int chunkSize, int chunkHeight, int x, int y, int z)
    {
        return x + chunkSize * y + chunkSize * chunkHeight * z;
    }

    public static Vector3Int GetVoxelCoordinatesInChunkSystem(ChunkData data, Vector3Int pos)
    {
        return new Vector3Int(pos.x - data.WorldPosition.x, pos.y - data.WorldPosition.y, pos.z - data.WorldPosition.z);
    }

    public static void LoopThroughVoxels(ChunkData chunkData, Action<int, int , int> action)
    {
        for (int i = 0; i < chunkData.Voxels.Length; i++)
        {
            Vector3Int position = GetPositionFromIndex(chunkData, i);
            action(position.x, position.y, position.z);
        }
    }
    public static MeshData GetChunkMeshData(ChunkData data)
    {
        MeshData meshData = new MeshData(true);

        LoopThroughVoxels(data, (x, y, z) => meshData = VoxelHelper.GetMeshData(data, x, y, z, meshData, data.Voxels[GetIndexFromPosition(data, x, y, z)]));

        return meshData;
    }

    public static Vector3Int ChunkPositionFromVoxelCoordinates(Map map, int x, int y, int z)
    {
        return new Vector3Int
        {
            x = Mathf.FloorToInt(x / (float)map.ChunkSize) * map.ChunkSize,
            y = Mathf.FloorToInt(y / (float)map.ChunkHeight) * map.ChunkHeight,
            z = Mathf.FloorToInt(z / (float)map.ChunkSize) * map.ChunkSize
        };
    }
}
