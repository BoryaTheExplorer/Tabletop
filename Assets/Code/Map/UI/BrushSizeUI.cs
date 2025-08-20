using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeUI : MonoBehaviour
{
    [Header("Voxel Editor")]
    [SerializeField] private VoxelEditor _editor;

    [Header("Brush Settings Fields")]
    [SerializeField] private TMP_InputField _brushSizeInput;
    [SerializeField] private Slider _brushSizeSlider;

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
}
