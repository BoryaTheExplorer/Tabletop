using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoxelSelectorUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private VoxelItemUI _voxelUIPrefab;

    private List<VoxelItemUI> _voxels = new List<VoxelItemUI>();

    public VoxelType Voxel;
    public event Action<VoxelType> OnValueChanged;

    private void Start()
    {
        VoxelItemUI voxelItemUI;
        foreach (var voxel in VoxelDataManager.VoxelTextureDataDictionary)
        {
            if (voxel.Key == VoxelType.Air || voxel.Key == VoxelType.Nothing)
                continue;

            voxelItemUI = Instantiate(_voxelUIPrefab, _content);
            voxelItemUI.OnClick += VoxelItemUI_OnClick;
            voxelItemUI.Voxel = voxel.Key;
            voxelItemUI.Setup(voxel.Value.Side.x, voxel.Value.Side.y);

            _voxels.Add(voxelItemUI);
        }
    }

    private void VoxelItemUI_OnClick(VoxelType obj)
    {
        Voxel = obj;
        OnValueChanged?.Invoke(obj);

        List<VoxelItemUI> selected = _voxels.Where(q => q.Voxel != obj && q.Clicked == true).ToList();
        foreach (var voxel in selected)
        {
            voxel.ResetColor();
        }
    }
}
