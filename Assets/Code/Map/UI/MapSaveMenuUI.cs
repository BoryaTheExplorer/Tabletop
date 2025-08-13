using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MapSaveMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private TextMeshProUGUI _warning;

    public void Save()
    {
        string text = _nameField.text;

        if ( text.Length == 0)
        {
            _warning.gameObject.SetActive(true);
            return;
        }

        _warning.gameObject.SetActive(false);

        MapRegistry.RegisterMapData(new MapData(new Dictionary<Vector3Int, ChunkData>(MapRegistry.Map.ChunkDataDictionary), text));
        gameObject.SetActive(false);
    }
}
