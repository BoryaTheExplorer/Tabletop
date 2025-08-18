using UnityEngine;

public static class VoxelHelper
{
    private static Direction[] directions =
    {
        Direction.up,
        Direction.down, 
        Direction.left, 
        Direction.right,
        Direction.forward,
        Direction.backward
    };

    public static MeshData GetMeshData(ChunkData chunk, int x, int y, int z, MeshData meshData, VoxelType voxelType)
    {
        if (voxelType == VoxelType.Air || voxelType == VoxelType.Nothing)
            return meshData;
        
        foreach (Direction direction in directions)
        {
            Vector3Int neighbourVoxelCoordinates = new Vector3Int(x, y, z) + direction.GetVector();
            VoxelType neighbourType = Chunk.GetVoxelFromChunkCoordinates(chunk, neighbourVoxelCoordinates.x,
                                                                                neighbourVoxelCoordinates.y,
                                                                                neighbourVoxelCoordinates.z);

            if (/*neighbourType != VoxelType.Nothing && */!VoxelDataManager.VoxelTextureDataDictionary[neighbourType].IsSolid)
            {
                if (voxelType == VoxelType.Water)
                {
                    meshData.SubMesh = GetFaceDataIn(direction, chunk, x, y, z, meshData.SubMesh, voxelType);
                }
                else
                {
                    meshData = GetFaceDataIn(direction, chunk, x, y, z, meshData, voxelType);
                }
            }
        }
        return meshData;
    }
    public static MeshData GetFaceDataIn(Direction direction, ChunkData chunk, int x, int y, int z, MeshData meshData, VoxelType voxelType)
    {
        GetFaceVertices(direction, x, y, z, meshData, voxelType);
        meshData.AddQuadTriangles(VoxelDataManager.VoxelTextureDataDictionary[voxelType].GeneratesCollider);
        meshData.UVs.AddRange(FaceUVs(direction, voxelType));

        return meshData;
    }

    public static void GetFaceVertices(Direction direction, int x, int y, int z, MeshData data, VoxelType voxelType)
    {
        bool generatesCollider = VoxelDataManager.VoxelTextureDataDictionary[voxelType].GeneratesCollider;

        switch (direction)
        {
            case Direction.up:
                data.AddVertex(new Vector3(x - .5f, y + .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y + .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y + .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y + .5f, z - .5f), generatesCollider);
                break;
            case Direction.down:
                data.AddVertex(new Vector3(x - .5f, y - .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y - .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y - .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y - .5f, z + .5f), generatesCollider);
                break;
            case Direction.left:
                data.AddVertex(new Vector3(x - .5f, y - .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y + .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y + .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y - .5f, z - .5f), generatesCollider);
                break;
            case Direction.right:
                data.AddVertex(new Vector3(x + .5f, y - .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y + .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y + .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y - .5f, z + .5f), generatesCollider);
                break;
            case Direction.forward:
                data.AddVertex(new Vector3(x + .5f, y - .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y + .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y + .5f, z + .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y - .5f, z + .5f), generatesCollider);
                break;
            case Direction.backward:
                data.AddVertex(new Vector3(x - .5f, y - .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x - .5f, y + .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y + .5f, z - .5f), generatesCollider);
                data.AddVertex(new Vector3(x + .5f, y - .5f, z - .5f), generatesCollider);
                break;
            default:
                break;
        }
    }

    public static Vector2Int TexturePosition(Direction direction, VoxelType voxelType)
    {
        return direction switch
        {
            Direction.up => VoxelDataManager.VoxelTextureDataDictionary[voxelType].Up,
            Direction.down => VoxelDataManager.VoxelTextureDataDictionary[voxelType].Down,
            _ => VoxelDataManager.VoxelTextureDataDictionary[voxelType].Side
        };
    }

    public static Vector2[] FaceUVs(Direction direction, VoxelType voxelType)
    {
        Vector2[] uvs = new Vector2[4];
        Vector2Int tilePos = TexturePosition(direction, voxelType);

        uvs[0] = new Vector2(VoxelDataManager.TileSizeX * tilePos.x + VoxelDataManager.TileSizeX - VoxelDataManager.TextureOffset,
                             VoxelDataManager.TileSizeY * tilePos.y + VoxelDataManager.TextureOffset);

        uvs[1] = new Vector2(VoxelDataManager.TileSizeX * tilePos.x + VoxelDataManager.TileSizeX - VoxelDataManager.TextureOffset,
                             VoxelDataManager.TileSizeY * tilePos.y + VoxelDataManager.TileSizeY - VoxelDataManager.TextureOffset);

        uvs[2] = new Vector2(VoxelDataManager.TileSizeX * tilePos.x + VoxelDataManager.TextureOffset,
                             VoxelDataManager.TileSizeY * tilePos.y + VoxelDataManager.TileSizeY - VoxelDataManager.TextureOffset);
        
        uvs[3] = new Vector2(VoxelDataManager.TileSizeX * tilePos.x + VoxelDataManager.TextureOffset,
                             VoxelDataManager.TileSizeY * tilePos.y + VoxelDataManager.TextureOffset);

        return uvs;
    }
}
