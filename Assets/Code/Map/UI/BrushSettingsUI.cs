using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrushSettingsUI : MonoBehaviour
{
    [Header("Voxel Editor")]
    [SerializeField] private VoxelEditor _editor;

    [Header("Brush Settings Fields")]
    [SerializeField] private TMP_InputField _brushSizeInput;
    [SerializeField] private Slider _brushSizeSlider;

    [SerializeField] private TMP_Dropdown _brushTypeDropdown;

    [SerializeField] private TMP_Dropdown _voxelTypeDropdown;

    private void Start()
    {
        if (!_editor)
        {
            Debug.Log("Voxel Editor is missing.");
            return;
        }

        //Brush Size
        {
            if (_brushSizeInput)
            {
                _brushSizeInput.onValueChanged.AddListener(OnBrushSizeInputValueChanged);
                _brushSizeInput.SetTextWithoutNotify(_editor.BrushSize.ToString());
            }
            if (_brushSizeSlider)
            {
                _brushSizeSlider.onValueChanged.AddListener(OnBrushSizeSliderValueChanged);
                _brushSizeSlider.SetValueWithoutNotify(_editor.BrushSize);
            }
        }
        //Brush Type
        {
            string[] brushNames = Enum.GetNames(typeof(VoxelBrushType));
            List<string> brushOptions = brushNames.ToList();

            if ( _brushTypeDropdown)
            {
                _brushTypeDropdown.ClearOptions();
                _brushTypeDropdown.AddOptions(brushOptions);
                
                _brushTypeDropdown.onValueChanged.AddListener(OnBrushDropdownValueChanged);
                _brushTypeDropdown.SetValueWithoutNotify((int)_editor.BrushType);
            }
        }
        //VoxelToPaint
        {
            string[] voxelNames = Enum.GetNames(typeof(VoxelType));
            List<string> voxelOptions = voxelNames.ToList();

            if (_voxelTypeDropdown)
            {
                _voxelTypeDropdown.ClearOptions();
                _voxelTypeDropdown.AddOptions(voxelOptions);

                _voxelTypeDropdown.onValueChanged.AddListener(OnVoxelDropdownValueChanged);
                _voxelTypeDropdown.SetValueWithoutNotify((int)_editor.VoxelToPaint);
            }
        }
    }

    private void OnBrushSizeInputValueChanged(string input)
    {
        if (input.Length == 0)
            return;

        if (!int.TryParse(input, out int size))
            return;

        if (size == _editor.BrushSize)
            return;

        _editor.SetBrushSize(size);
        _brushSizeSlider.SetValueWithoutNotify(size);
    }
    private void OnBrushSizeSliderValueChanged(float value)
    {
        int size = (int)value;

        if (size == _editor.BrushSize)
            return;

        _editor.SetBrushSize(size);
        _brushSizeInput.SetTextWithoutNotify(size.ToString());
    }
    private void OnBrushDropdownValueChanged(int option)
    {
        VoxelBrushType brushType = (VoxelBrushType)option;

        if (_editor.BrushType == brushType)
            return;

        _editor.SetBrushType(brushType);
    }
    private void OnVoxelDropdownValueChanged(int option)
    {
        VoxelType voxel = (VoxelType)option;

        if (_editor.VoxelToPaint == voxel)
            return;

        _editor.SetVoxelToPaint(voxel);
    }
}
