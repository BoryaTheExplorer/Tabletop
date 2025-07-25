using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class NetworkGrid : NetworkBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private int _gridSize = 50;
    private GameObject[,] _tiles; 
    [HideInInspector] public NetworkVariable<TileDataArray> TileDataArray = new NetworkVariable<TileDataArray>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            TileDataArray.Value = new TileDataArray
            {
                TileData = new TileData[_gridSize * _gridSize],
                GridSize = _gridSize
            };
        
        TileDataArray.OnValueChanged += UpdateGrid;
        _tiles = new GameObject[_gridSize, _gridSize];

        StartCoroutine(WaitAndChange());
        
        _ = BuildGrid();
    }

    public void UpdateGrid(TileDataArray previousValue, TileDataArray newValue)
    {
        Debug.Log("Updated");
        _ = BuildGrid();
    }

    private IEnumerator WaitAndChange()
    {
        yield return new WaitForSeconds(5f);

        if (IsServer)
        {
            TileDataArray array = new TileDataArray
            {
                TileData = new TileData[_gridSize * _gridSize],
                GridSize = _gridSize
            };

            array.TileData[0] = new TileData
            {
                Id = 1,
                Height = 1
            };
            array.TileData[3] = new TileData
            {
                Id = 1,
                Height = 3
            };
            TileDataArray.Value = array;
        }
    }

    private async Task BuildGrid()
    {
        var data = TileDataArray.Value;
        await Awaitable.NextFrameAsync();
        
        for (int i = 0; i < _gridSize; i++)
        {
            for (int j  = 0; j < _gridSize; j++)
            {
                if (data.TileData[j * _gridSize + i].Id == 0) continue;
                if (_tiles[i, j] != null)
                {
                    Destroy(_tiles[i, j]);
                    _tiles[i, j] = null;
                }

                _tiles[i, j] = Instantiate(_tilePrefab, new Vector3(i, 0f, j), Quaternion.identity);
                _tiles[i, j].transform.localScale = new Vector3(1f, data.Get(i, j).Height, 1f);
            }
        }
    }
}
