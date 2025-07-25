using System.Collections.Generic;
using UnityEngine;

public class VoxelDataManager : MonoBehaviour
{
    public static float TextureOffset = .001f;

    public static float TileSizeX;
    public static float TileSizeY;

    public static Dictionary<VoxelType, TextureData> VoxelTextureDataDictionary = new Dictionary<VoxelType, TextureData>();

    public VoxelDataSO VoxelData;

    private void Awake()
    {
        foreach (TextureData item in VoxelData.TextureDataList)
        {
            if (!VoxelTextureDataDictionary.ContainsKey(item.VoxelType))
            {
                VoxelTextureDataDictionary.Add(item.VoxelType, item);
            }
        }

        TileSizeX = VoxelData.TextureSizeX;
        TileSizeY = VoxelData.TextureSizeY;
    }
}
