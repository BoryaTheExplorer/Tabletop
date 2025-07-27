using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    
    public int MapSizeInChunks = 6;
    public int ChunkSize = 16;
    public int ChunkHeight = 100;
    public int WaterThreshold = 50;
    public float NoiseScale = .003f;

    public Dictionary<Vector3Int, ChunkData> ChunkDataDictionary = new Dictionary<Vector3Int, ChunkData>();
    public Dictionary<Vector3Int, ChunkRenderer> ChunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>();

    public void GenerateMap()
    {
        ChunkDataDictionary.Clear();
        foreach (ChunkRenderer chunk in ChunkDictionary.Values)
        {
            Destroy(chunk.gameObject);
        }

        ChunkDictionary.Clear();

        for (int x = 0; x < MapSizeInChunks; x++)
        {
            for (int z = 0; z < MapSizeInChunks; z++)
            {
                ChunkData data = new ChunkData(ChunkSize, ChunkHeight, this, new Vector3Int(x * ChunkSize, 0, z * ChunkSize));
                GenerateVoxels(data);
                ChunkDataDictionary.Add(data.WorldPosition, data);
            }
        }

        foreach (ChunkData data in ChunkDataDictionary.Values)
        {
            MeshData meshData = Chunk.GetChunkMeshData(data);
            GameObject chunkObject = Instantiate(_chunkPrefab, data.WorldPosition, Quaternion.identity);
            ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
            ChunkDictionary.Add(data.WorldPosition, chunkRenderer);
            chunkRenderer.InitChunk(data);
            chunkRenderer.UpdateChunk(meshData);
        }
    }

    private void GenerateVoxels(ChunkData data)
    {
        for (int x = 0; x < data.ChunkSize; x++)
        {
            for (int z = 0; z < data.ChunkSize; z++)
            {
                float noiseValue = Mathf.PerlinNoise((data.WorldPosition.x + x) * NoiseScale, (data.WorldPosition.z + z) * NoiseScale);
                int groundPosition = Mathf.RoundToInt(noiseValue * 25);

                for (int y = 0; y < data.ChunkHeight; y++)
                {
                    VoxelType voxelType = VoxelType.Dirt;
                    if (y > groundPosition)
                    if (y > groundPosition)
                        voxelType = VoxelType.Air;
                    if (y == groundPosition)
                        voxelType = VoxelType.Grass;

                    Chunk.SetVoxel(data, new Vector3Int(x, y, z), voxelType);
                }
            }
        }
    }

    public void ReconstructModifiedChunk(ChunkData data, Vector3Int voxelPosition)
    {
        ChunkRenderer containerChunk;
        MeshData meshData;
        ChunkDictionary.TryGetValue(data.WorldPosition, out containerChunk);

        if (containerChunk == null)
            return;

        meshData = Chunk.GetChunkMeshData(data);

        containerChunk.InitChunk(data);
        containerChunk.UpdateChunk(meshData);

        if (voxelPosition.x == 0)
        {
            ChunkDictionary.TryGetValue(data.WorldPosition + new Vector3Int(-1, 0, 0) * ChunkSize, out containerChunk);

            if (containerChunk != null)
            {
                meshData = Chunk.GetChunkMeshData(containerChunk.ChunkData);
                containerChunk.UpdateChunk(meshData);
            }
        }
        if (voxelPosition.x >= 15)
        {
            ChunkDictionary.TryGetValue(data.WorldPosition + new Vector3Int(1, 0, 0) * ChunkSize, out containerChunk);

            if (containerChunk != null)
            {
                meshData = Chunk.GetChunkMeshData(containerChunk.ChunkData);
                containerChunk.UpdateChunk(meshData);
            }
        }
        if (voxelPosition.z == 0)
        {
            ChunkDictionary.TryGetValue(data.WorldPosition + new Vector3Int(0, 0, -1) * ChunkSize, out containerChunk);

            if (containerChunk != null)
            {
                meshData = Chunk.GetChunkMeshData(containerChunk.ChunkData);
                containerChunk.UpdateChunk(meshData);
            }
        }
        if (voxelPosition.z >= 15)
        {
            ChunkDictionary.TryGetValue(data.WorldPosition + new Vector3Int(0, 0, 1) * ChunkSize, out containerChunk);

            if (containerChunk != null)
            {
                meshData = Chunk.GetChunkMeshData(containerChunk.ChunkData);
                containerChunk.UpdateChunk(meshData);
            }
        }
    }
    public ChunkData GetChunkDataFromWorldCoordinates(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / ChunkSize) * ChunkSize;
        int z = Mathf.FloorToInt(position.z / ChunkSize) * ChunkSize;

        if (x < 0 || z < 0 || x > (MapSizeInChunks - 1) * ChunkSize || z > (MapSizeInChunks - 1) * ChunkSize)
            return null;

        ChunkData containerChunk = null;
        ChunkDataDictionary.TryGetValue(new Vector3Int(x, 0, z), out containerChunk);

        return containerChunk;
    }
    public VoxelType GetVoxelFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.ChunkPositionFromVoxelCoordinates(this, x, y, z);
        ChunkData containerChunk = null;

        ChunkDataDictionary.TryGetValue(pos, out containerChunk);

        if (containerChunk == null)
            return VoxelType.Nothing;

        Vector3Int voxelInChunkCoordinates = Chunk.GetVoxelCoordinatesInChunkSystem(containerChunk, new Vector3Int(x, y, z));

        return Chunk.GetVoxelFromChunkCoordinates(containerChunk, voxelInChunkCoordinates.x,
                                                                  voxelInChunkCoordinates.y,
                                                                  voxelInChunkCoordinates.z);
    }
}
