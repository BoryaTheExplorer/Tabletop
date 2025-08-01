using System.Collections.Generic;
using UnityEngine;

public class MapLoadMenuUI : MonoBehaviour
{
    [SerializeField] private SavedMapUI _mapSavePrefab;
    [SerializeField] private GameObject _content;
    private List<SavedMapUI> _currentSaves = new();

    public void OnEnable()
    {
        Clear();

        List<SavedMapUI> savedMaps = new List<SavedMapUI>();
        SavedMapUI ui;

        foreach (string name in MapRegister.SavedMaps.Keys)
        {
            ui = Instantiate(_mapSavePrefab, _content.transform);
            ui.Init(name);
            savedMaps.Add(ui);
        }

    }
    private void Clear()
    {
        foreach (var item in _currentSaves)
        {
            Destroy(item.gameObject);
        }

        _currentSaves.Clear();
    }
}
