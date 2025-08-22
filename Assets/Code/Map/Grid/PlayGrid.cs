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
    public void AddGridObject(Vector3Int position, GridObject gridObject)
    {
        if(_grid.ContainsKey(position))
        {
            Debug.LogWarning($"Grid cell '{position}' is busy and can't be overriden");
            return;
        }

        _grid.Add(position, gridObject);
    }
    public void MoveGridObject(GridObject gridObject, Vector3Int position)
    {
        if (!_grid.Remove(gridObject.GridPosition))
        {
            Debug.LogWarning($"Can't move {gridObject.name} at {gridObject.GridPosition}, since there is no corresponding object in the Dictionary");
            return;
        }

        gridObject.Move(position);
        AddGridObject(position, gridObject);
    }
    public void MoveGridObject(Vector3Int key, Vector3Int position)
    {
        if (!_grid.TryGetValue(key, out GridObject obj))
            return;

        obj.Move(position);
        _grid.Remove(key);
        AddGridObject(key, obj);
    }
    public GridObject SpawnGridObject(int id, Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        Vector3Int newPosition = new Vector3Int(x, y, z);
        
        GridObject obj = Instantiate(GridObjectDataManager.MiniaturesSO.GetGridObject(id), newPosition, Quaternion.identity);
        obj.Init(newPosition);

        AddGridObject(newPosition, obj);
        return obj;
    }
    
    public void RemoveGridObject(GridObject gridObject)
    {
        //I'll do it later...
    }
}
