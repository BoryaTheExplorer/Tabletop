using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VoxelItemUI : SwitchButtonImageUI
{
    public VoxelType Voxel;
    public event Action<VoxelType> OnClick;

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(Voxel);

        if (!Clicked)
            base.OnPointerClick(eventData);
    }
    private void Setup(int x, int y)
    {
        float texX = x / 10f;
        float texY = y / 10f;

        RawImage raw = (RawImage)_target;

        Rect rect = raw.uvRect;
        rect.x = texX;
        rect.y = texY;
        
        raw.uvRect = rect;
    }
    public void Setup(VoxelType type)
    {
        Vector2Int uv = VoxelDataManager.VoxelTextureDataDictionary[type].Side;

        Setup(uv.x, uv.y);
        Voxel = type;
    }
}
