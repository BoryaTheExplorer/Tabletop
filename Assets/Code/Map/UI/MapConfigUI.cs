using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapConfigUI : MonoBehaviour
{
    [Header("Map")]
    [SerializeField] private Map _map;
    [Header("Config")]
    [SerializeField] private Toggle _usePerlin;
    [SerializeField] private Slider _perlinSlider;
    [SerializeField] private TMP_InputField _perlinInput;
    [SerializeField] private Slider _heightSlider;
    [SerializeField] private TMP_InputField _heightInput;
    [SerializeField] private TMP_Dropdown _surfaceVoxel;
    [SerializeField] private TMP_Dropdown _subsurfaceVoxel;

    private void Start()
    {
        //Perlin
        {
            if (_usePerlin)
                _usePerlin.onValueChanged.AddListener(OnUsePerlin);
            if (_perlinSlider)
            {
                _perlinSlider.onValueChanged.AddListener(OnPerlinSliderValueChanged);
                _perlinSlider.SetValueWithoutNotify(_map.NoiseScale);
            }
            if (_perlinInput)
            {
                _perlinInput.onValueChanged.AddListener(OnPerlinInputValueChanged);
                _perlinInput.SetTextWithoutNotify(_map.NoiseScale.ToString());
            }
                
        }
        //Height
        {
            if (_heightSlider)
            {
                _heightSlider.onValueChanged.AddListener(OnHeightSliderValueChanged);
                _heightSlider.SetValueWithoutNotify(_map.FloorHeight);
            }
            if (_heightInput)
            {
                _heightInput.onValueChanged.AddListener(OnHeightInputValueChanged);
                _heightInput.SetTextWithoutNotify(_map.FloorHeight.ToString());
            }
        }
        //Voxels
        {
            string[] names = Enum.GetNames(typeof(VoxelType));
            List<string> options = names.ToList();
            if (_surfaceVoxel)
            {
                _surfaceVoxel.onValueChanged.AddListener(OnSurfaceVoxelValueChanged);
                _surfaceVoxel.options.Clear();
                _surfaceVoxel.AddOptions(options);
                _surfaceVoxel.SetValueWithoutNotify((int)_map.SurfaceVoxel);
            }
                
            if (_subsurfaceVoxel)
            {
                _subsurfaceVoxel.onValueChanged.AddListener(OnSubsurfaceVoxelValueChanged);
                _subsurfaceVoxel.options.Clear();
                _subsurfaceVoxel.AddOptions(options);

                _subsurfaceVoxel.SetValueWithoutNotify((int)_map.SubsurfaceVoxel);
            }
        }
    }
    private void OnUsePerlin(bool value)
    {
        _map.SetUsePerlin(value);
    }
    private void OnPerlinSliderValueChanged(float value)
    {
        if (value == _map.NoiseScale)
            return;

        _map.SetPerlinScale(value);
        _perlinInput.SetTextWithoutNotify(value.ToString());
    }
    private void OnPerlinInputValueChanged(string value)
    {
        float perlin = float.Parse(value);

        if (perlin == _map.NoiseScale) 
            return;

        _map.SetPerlinScale(perlin);
        _perlinSlider.SetValueWithoutNotify(perlin);
    }

    private void OnHeightSliderValueChanged(float value)
    {
        if (value == _map.FloorHeight)
            return;

        _map.SetFloorHeight(Mathf.FloorToInt(value));
        _heightInput.SetTextWithoutNotify(value.ToString());
    }
    private void OnHeightInputValueChanged(string value)
    {
        int height = int.Parse(value);

        if (height == _map.FloorHeight)
            return;

        _map.SetFloorHeight(height);
        _heightSlider.SetValueWithoutNotify(height);
    }

    private void OnSurfaceVoxelValueChanged(int value)
    {
        VoxelType voxel = (VoxelType)value;

        if (voxel == _map.SurfaceVoxel)
            return;

        _map.SetSurfaceVoxel(voxel);
    }
    private void OnSubsurfaceVoxelValueChanged(int value)
    {
        VoxelType voxel = (VoxelType)value;

        if (voxel == _map.SubsurfaceVoxel)
            return;

        _map.SetSubsurfaceVoxel(voxel);
    }
}
