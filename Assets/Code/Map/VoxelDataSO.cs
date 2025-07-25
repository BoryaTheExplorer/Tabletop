using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoxelDataSO", menuName = "Scriptable Objects/VoxelDataSO")]
public class VoxelDataSO : ScriptableObject
{
    public float TextureSizeX;
    public float TextureSizeY;
    public List<TextureData> TextureDataList;
}
