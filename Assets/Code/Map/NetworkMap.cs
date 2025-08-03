using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMap : NetworkBehaviour
{
    [SerializeField] private Map _map;

    public void SendEditVoxelRPC(NetworkVoxelData data)
    {
        EditVoxelClientRPC(data);
    }
    public void SendUpdateMapRPC()
    {
        UpdateMapClientRPC();
    }

    [ClientRpc()]
    public void EditVoxelClientRPC(NetworkVoxelData data)
    {
        if (IsHost)
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
    public void UpdateMapClientRPC()
    {
        if (IsHost)
            return;

        if (_map == null)
            return;

        _map.UpdateModifiedChunks();
    }
}
