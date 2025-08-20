using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MapConfigUI : MonoBehaviour
{
    [Header("Map")]
    [SerializeField] private Map _map;
    [SerializeField] private NetworkMap _networkMap;
    [Header("Perlin")]
    [SerializeField] private Toggle _usePerlin;
    [SerializeField] private Slider _perlinSlider;
    [SerializeField] private TMP_InputField _perlinInput;
    [Header("Height")]
    [SerializeField] private Slider _heightSlider;
    [SerializeField] private TMP_InputField _heightInput;
    [Header("Voxels")]
    [SerializeField] private VoxelSelectorUI _surfaceVoxelSelectorUI;
    [SerializeField] private VoxelItemUI _surfaceVoxelItemUI;
    [SerializeField] private VoxelSelectorUI _subsurfaceVoxelSelectorUI;
    [SerializeField] private VoxelItemUI _subsurfaceVoxelItemUI;

    private void Start()
    {
        Debug.Log("Hello!");
        if (!NetworkManager.Singleton.IsHost) 
            Destroy(gameObject);

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
            if (_surfaceVoxelSelectorUI)
            {
                _surfaceVoxelSelectorUI.OnValueChanged += _surfaceVoxelSelectorUI_OnValueChanged;
                _map.SetSurfaceVoxel(_surfaceVoxelSelectorUI.Voxel);
                _surfaceVoxelSelectorUI.gameObject.SetActive(false);

                _surfaceVoxelItemUI.OnClick += _surfaceVoxelItemUI_OnClick;
            }
            if (_subsurfaceVoxelSelectorUI)
            {
                _subsurfaceVoxelSelectorUI.OnValueChanged += _subsurfaceVoxelSelectorUI_OnValueChanged;
                _map.SetSubsurfaceVoxel(_subsurfaceVoxelSelectorUI.Voxel);
                _subsurfaceVoxelSelectorUI.gameObject.SetActive(false);

                _subsurfaceVoxelItemUI.OnClick += _subsurfaceVoxelItemUI_OnClick;
            }
        }
    }

    private void _subsurfaceVoxelItemUI_OnClick(VoxelType obj)
    {
        _subsurfaceVoxelSelectorUI.gameObject.SetActive(true);
    }

    private void _surfaceVoxelItemUI_OnClick(VoxelType obj)
    {
        _surfaceVoxelSelectorUI.gameObject.SetActive(true);
    }

    private void _subsurfaceVoxelSelectorUI_OnValueChanged(VoxelType obj)
    {
        _map.SetSubsurfaceVoxel(obj);

        _subsurfaceVoxelItemUI.Setup(obj);
        _subsurfaceVoxelItemUI.ResetColor();

        _subsurfaceVoxelSelectorUI.gameObject.SetActive(false);
    }

    private void _surfaceVoxelSelectorUI_OnValueChanged(VoxelType obj)
    {
        _map.SetSurfaceVoxel(obj);

        _surfaceVoxelItemUI.Setup(obj);
        _surfaceVoxelItemUI.ResetColor();

        _surfaceVoxelSelectorUI.gameObject.SetActive(false);
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
    public void Generate()
    {
        _map.GenerateMap();

    }
}
