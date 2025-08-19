using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class VoxelBrushUI : SwitchButtonImageUI
{
    public VoxelBrushType Brush;
    public event Action<VoxelBrushType> OnClick;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (Clicked)
        {
            OnClick?.Invoke(Brush);
        }else
        {
            OnClick?.Invoke(VoxelBrushType.None);
        }
    }
}
