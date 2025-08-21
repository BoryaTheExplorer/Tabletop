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
        if (!_selectedObject)
        {
            ScanAndSelectGridObject();
            return;
        }

        if (GridObjectScanner.Scan(out GridObject obj, out Vector3Int position))
        {
            _selectedObject.Deselect();
            _selectedObject = obj;
            obj.Select();
        }
        else
        {
            _selectedObject.Deselect();
            PlayGrid.Instance.MoveGridObject(_selectedObject, position);
            _selectedObject = null;
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
