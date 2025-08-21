using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayGrid : NetworkBehaviour
{
    [SerializeField] private PlayGrid _playGrid;
    private Dictionary<Vector3Int, int> _networkGrid = new Dictionary<Vector3Int, int>();
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
            {
                if (clientId == NetworkManager.Singleton.LocalClientId)
                    return;

                if (_playGrid.Grid.Count == 0)
                    return;

                foreach (var obj in _playGrid.Grid)
                {
                    SendGridObjectServerRpc(clientId, obj.Value.ID, obj.Key.x, obj.Key.y, obj.Key.z);
                }
            };
        }
    }

    [ServerRpc()]
    public void SendGridObjectServerRpc(ulong clientId, int gridObjectId, int x, int y, int z)
    {
        ClientRpcParams rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId },
            }
        };

        ReceiveGridObjectClientRpc(clientId, gridObjectId, x, y, z, rpcParams);
    }
    [ClientRpc()]
    public void ReceiveGridObjectClientRpc(ulong clientId, int gridObjectId, int x, int y, int z, ClientRpcParams rpcParams = default)
    {
        _playGrid.SpawnGridObject(gridObjectId, new Vector3(x, y, z)).Init(new Vector3Int(x, y, z));
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnGridObjectServerRpc(ulong clientId, int gridObjectId, int x, int y, int z)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
            return;

        _playGrid.SpawnGridObject(gridObjectId, new Vector3(x, y, z));
    }

}
