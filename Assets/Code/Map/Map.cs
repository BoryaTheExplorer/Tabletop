using System;
using System.Collections.Generic;
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
                int groundPosition = Mathf.RoundToInt(noiseValue * ChunkHeight);

                for (int y = 0; y < 100; y++)
                {
                    VoxelType voxelType = VoxelType.Dirt;
                    if (y > groundPosition)
                        voxelType = VoxelType.Air;
                    if (y == groundPosition)
                        voxelType = VoxelType.Grass;

                    Chunk.SetVoxel(data, new Vector3Int(x, y, z), voxelType);
                }
            }
        }
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
