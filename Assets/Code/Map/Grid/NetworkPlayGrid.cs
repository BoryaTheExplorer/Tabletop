using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
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
        Vector3Int position = new Vector3Int(x, y, z);
        if (!_playGrid.Grid.ContainsKey(position))
            _playGrid.SpawnGridObject(gridObjectId, position).Init(position);
    }
    [ClientRpc()]
    public void ReceiveAndCheckGridObjectClientRpc(ulong clientId, int gridObjectId, int x, int y, int z)
    {
        Debug.Log($"Client Id: {clientId}, Local Client Id: {NetworkManager.Singleton.LocalClientId}");
        if (clientId == NetworkManager.Singleton.LocalClientId)
            return;
        Vector3Int position = new Vector3Int(x, y, z);
        _playGrid.SpawnGridObject(gridObjectId, position).Init(position);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnGridObjectServerRpc(ulong clientId, int gridObjectId, int x, int y, int z, ServerRpcParams rpcParams = default)
    {
        ReceiveAndCheckGridObjectClientRpc(clientId, gridObjectId, x, y, z);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveObjectServerRpc(int xOriginal, int yOriginal, int zOriginal, int xNew, int yNew, int zNew, ServerRpcParams rpcParams = default)
    {
        Vector3Int key = new Vector3Int(xOriginal, yOriginal, zOriginal);
        if (_playGrid.Grid.ContainsKey(key))
            MoveObjectClientRpc(xOriginal, yOriginal, zOriginal, xNew, yNew, zNew, rpcParams.Receive.SenderClientId);
    }

    [ClientRpc()]
    public void MoveObjectClientRpc(int xOriginal, int yOriginal, int zOriginal, int xNew, int yNew, int zNew, ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
            return;

        Vector3Int key = new Vector3Int(xOriginal, yOriginal, zOriginal);
        Vector3Int position = new Vector3Int(xNew, yNew, zNew);

        _playGrid.MoveGridObject(key, position);
    }
}
