using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mesh;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;

    public bool UsePerlin { get; private set; } = true;
    public float NoiseScale { get; private set; } = .003f;
    public int FloorHeight { get; private set; } = 25;
    public VoxelType SurfaceVoxel { get; private set; } = VoxelType.Grass;
    public VoxelType SubsurfaceVoxel { get; private set; } = VoxelType.Dirt;
    
    public int MapSizeInChunks = 3;
    public int ChunkSize = 16;
    public int ChunkHeight = 100;
    public int WaterThreshold = 50;

    public Dictionary<Vector3Int, ChunkData> ChunkDataDictionary = new Dictionary<Vector3Int, ChunkData>();
    public Dictionary<Vector3Int, ChunkRenderer> ChunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>();

    //SYNC USING CUSTOM MESSAGE
    public void LoadMapLayout(MapData map)
    {
        ChunkDataDictionary.Clear();
        ChunkDataDictionary = map.ChunkDataDicitonary;

        foreach (var chunk in ChunkDictionary.Values) 
        {
            Destroy(chunk.gameObject);
        }
        ChunkDictionary.Clear();

        BuildChunks();
    }

    public void BuildChunks()
    {
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
        BuildChunks();

        Debug.Log("Chunks: " + ChunkDataDictionary.Keys.Count);
        foreach (Vector3Int pos in ChunkDataDictionary.Keys)
            Debug.Log(pos.ToString());
    }

    private void GenerateVoxels(ChunkData data)
    {
        for (int x = 0; x < data.ChunkSize; x++)
        {
            for (int z = 0; z < data.ChunkSize; z++)
            {
                int groundPosition = FloorHeight;// 

                if (UsePerlin)
                {
                    float noiseValue = Mathf.PerlinNoise((data.WorldPosition.x + x) * NoiseScale, (data.WorldPosition.z + z) * NoiseScale);
                    groundPosition = Mathf.RoundToInt(noiseValue * FloorHeight);
                }
                for (int y = 0; y < data.ChunkHeight; y++)
                {
                    VoxelType voxelType = SubsurfaceVoxel;
                    if (y > groundPosition)
                        voxelType = VoxelType.Air;
                    if (y == groundPosition)
                        voxelType = SurfaceVoxel;

                    Chunk.SetVoxel(data, new Vector3Int(x, y, z), voxelType);
                }
            }
        }
    }
    public void UpdateModifiedChunks()
    {
        ChunkRenderer containerChunk;
        MeshData meshData;
        Vector3Int position;
        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.forward,
            Vector3Int.back
        };
     
        foreach (var chunkData in ChunkDataDictionary.Values)
        {
            if (!chunkData.Modified) continue;

            meshData = Chunk.GetChunkMeshData(chunkData);
            position = chunkData.WorldPosition;
            
            containerChunk = ChunkDictionary[position];

            containerChunk.InitChunk(chunkData);
            containerChunk.UpdateChunk(meshData);

            chunkData.Modified = false;

            UpdateNeighbourChunks(position, directions);
        }
    }

    private void UpdateNeighbourChunks(Vector3Int origin, Vector3Int[] directions)
    {
        ChunkRenderer containerChunk;
        MeshData meshData;
        foreach (Vector3Int direction in directions)
        {
            ChunkDictionary.TryGetValue(origin + direction * ChunkSize, out containerChunk);
            
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

        ChunkData containerChunk;
        ChunkDataDictionary.TryGetValue(new Vector3Int(x, 0, z), out containerChunk);

        return containerChunk;
    }

    public VoxelType GetVoxelFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.ChunkPositionFromVoxelCoordinates(this, x, y, z);
        ChunkData containerChunk;

        ChunkDataDictionary.TryGetValue(pos, out containerChunk);

        if (containerChunk == null)
            return VoxelType.Nothing;

        Vector3Int voxelInChunkCoordinates = Chunk.GetVoxelCoordinatesInChunkSystem(containerChunk, new Vector3Int(x, y, z));

        return Chunk.GetVoxelFromChunkCoordinates(containerChunk, voxelInChunkCoordinates.x,
                                                                  voxelInChunkCoordinates.y,
                                                                  voxelInChunkCoordinates.z);
    }

    public void SetSurfaceVoxel(VoxelType voxel)
    {
        SurfaceVoxel = voxel;
    }
    public void SetSubsurfaceVoxel(VoxelType voxel)
    {
        SubsurfaceVoxel = voxel;
    }
    public void SetUsePerlin(bool use)
    {
        UsePerlin = use;
    }
    public void SetPerlinScale(float perlin)
    {
        NoiseScale = perlin;
    }
    public void SetFloorHeight(int floorHeight)
    {
        FloorHeight = floorHeight;
    }
}
