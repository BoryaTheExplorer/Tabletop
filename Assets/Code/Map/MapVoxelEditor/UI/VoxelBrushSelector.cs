using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoxelBrushSelector : MonoBehaviour
{
    [SerializeField] private VoxelEditor _editor;
    [SerializeField] private GameObject _brushMaterials;

    [SerializeField] private GameObject _brushSize;
    [SerializeField] private SerializableDictionary<VoxelBrushType, VoxelBrushUI> _serializableBrushes;
    private Dictionary<VoxelBrushType, VoxelBrushUI> _brushes;
    
    private void Start()
    {
        _brushes = _serializableBrushes.GetDictionary();

        foreach (var brush in _brushes.Values )
        {
            brush.OnClick += Brush_OnClick;
        }
    }

    private void Brush_OnClick(VoxelBrushType obj)
    {
        switch (obj)
        {
            case VoxelBrushType.Single:
                _brushMaterials.SetActive(true);
                break;
            case VoxelBrushType.Sphere:
                _brushMaterials.SetActive(true);
                _brushSize.SetActive(true);
                break;
            default:
                foreach (var brush in _brushes.Values.Where(q => q.Clicked))
                    brush.ResetColor();
                _brushMaterials.SetActive(false);
                _brushSize.SetActive(false);
                break;
        }

        _editor.SetBrushType(obj);
    }

    private void BrushSphereOnClick()
    {

    }
    private void OnDestroy()
    {

    }
}
