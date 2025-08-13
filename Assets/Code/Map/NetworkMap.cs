using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMap : NetworkBehaviour
{
    [SerializeField] private Map _map;
    public override void OnNetworkSpawn()
    {
        MapRegistry.Init(_map);
        MapRegistry.NetworkMap = this;
    }
    public void SendEditVoxelRPC(NetworkVoxelData data, ulong clientId)
    {
        RequestEditVoxelServerRpc(data, clientId);
    }
    public void SendUpdateMapRpc(ulong clientId)
    {
        RequestUpdateMapServerRpc(clientId);
    }
    [ServerRpc(RequireOwnership = false)]
    public void RequestEditVoxelServerRpc(NetworkVoxelData data, ulong clientId)
    {
        EditVoxelClientRpc(data, clientId);
    }
    [ServerRpc(RequireOwnership = false)]
    public void RequestUpdateMapServerRpc(ulong clientId)
    {
        UpdateMapClientRpc(clientId);
    }

    [ClientRpc()]
    public void EditVoxelClientRpc(NetworkVoxelData data, ulong clientId)
    {
        if (NetworkManager.LocalClientId == clientId)
            return;

        if (_map == null) 
            return;

        ChunkData chunkData;
        _map.ChunkDataDictionary.TryGetValue(data.ChunkPos.ToVector3Int(), out chunkData);

        if (chunkData == null)
            return;

        Vector3Int voxelPosition = data.VoxelPos.ToVector3Int();

        if (Chunk.SetVoxel(chunkData, voxelPosition, data.Voxel))
            chunkData.Modified = true;
    }
    [ClientRpc()]
    public void LoadFromRegisterClientRpc(string mapKey)
    {
        if (IsServer)
            return;

        MapRegistry.LoadMapData(mapKey);
    }

    [ClientRpc()]
    public void UpdateMapClientRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
            return;

        if (_map == null)
            return;

        _map.UpdateModifiedChunks();
    }
}
