using System;
using UnityEngine;

[Serializable]
public class TextureData
{
    public VoxelType VoxelType;
    
    public Vector2Int Up;
    public Vector2Int Down;
    public Vector2Int Side;

    public bool IsSolid = true;
    public bool GeneratesCollider = true;
}
