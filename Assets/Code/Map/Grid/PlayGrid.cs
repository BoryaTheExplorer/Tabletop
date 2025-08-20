using System.Collections.Generic;
using UnityEngine;

public class PlayGrid : MonoBehaviour
{
    public static PlayGrid Instance { get; private set; }
    private Dictionary<Vector3Int, GridObject> _grid = new Dictionary<Vector3Int, GridObject>();
    public Dictionary<Vector3Int, GridObject> Grid { get { return _grid; } }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void AddGridObject(GridObject gridObject, Vector3Int position)
    {
        if(!_grid.ContainsKey(position))
            _grid.Add(position, gridObject);
    }
    public void SpawnGridObject(int id, Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        Vector3Int newPosition = new Vector3Int(x, y, z);
        
        GridObject obj = Instantiate(GridObjectDataManager.MiniaturesSO.GetGridObject(id), newPosition, Quaternion.identity);
        AddGridObject(obj, newPosition);
    }
    
    public void RemoveGridObject(GridObject gridObject)
    {

    }
}
