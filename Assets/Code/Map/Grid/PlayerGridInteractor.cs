using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGridInteractor : NetworkBehaviour
{
    [SerializeField] private NetworkPlayGrid _networkGrid;
    private GridObject _selectedObject;
    public override void OnNetworkSpawn()
    {
        int objToSpawn = 0;
        Vector3 position = PlayersData.Instance.SpawnPoints[(int)NetworkManager.Singleton.LocalClientId].position - Vector3.down;
        PlayGrid.Instance.SpawnGridObject(objToSpawn, position);
        _networkGrid.SpawnGridObjectServerRpc(NetworkManager.Singleton.LocalClientId, objToSpawn, (int)position.x, (int)position.y, (int)position.z);
    }

    private void Start()
    {
        GameInput.Instance.OnClickPerformed += GameInput_OnClickPerformed;
    }

    private void GameInput_OnClickPerformed()
    {
        Debug.Log("Firing Grid Search");

        if (!_selectedObject)
        {
            Debug.Log("First If");
            ScanAndSelectGridObject();
            return;
        }

        if (GridObjectScanner.Scan(out GridObject obj, out Vector3Int position))
        {
            Debug.Log("Second If");
            _selectedObject.Deselect();
            _selectedObject = obj;
            obj.Select();
        }
        else
        {
            Debug.Log("Second else");
            _selectedObject.Move(position);
        }
    }

    public bool ScanAndSelectGridObject()
    {
        if (!GridObjectScanner.Scan(out GridObject obj, out Vector3Int position))
            return false;

        _selectedObject = obj;
        obj.Select();

        return true;
    }
}
