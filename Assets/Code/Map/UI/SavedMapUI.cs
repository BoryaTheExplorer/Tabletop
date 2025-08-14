using TMPro;
using UnityEngine;

public class SavedMapUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    public string MapName {  get; private set; }
    public void Init(string mapName)
    {
        MapName = mapName;
        _name.text = MapName;
    }

    public void Load()
    {
        MapRegistry.LoadMapData(MapName);
        MapRegistry.NetworkMap.LoadFromRegistryClientRpc(MapName);
    }
    public void Remove()
    {
        MapRegistry.RemoveMapData(MapName);
        Destroy(gameObject);
    }
}
