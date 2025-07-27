using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMap : NetworkBehaviour
{
    [SerializeField] private Map _map;

    public void SendUpdateMapRPC(NetworkVoxelData data)
    {
        UpdateMapClientRPC(data);
    }

    [ClientRpc()]
    public void UpdateMapClientRPC(NetworkVoxelData data)
    {
        if (IsHost)
            return;

        if (_map == null) 
            return;


        ChunkData chunkData;
        _map.ChunkDataDictionary.TryGetValue(new Vector3Int(data.WorldX, data.WorldY, data.WorldZ), out chunkData);

        if (chunkData == null)
            return;

        Vector3Int voxelPosition = new Vector3Int(data.VoxelX, data.VoxelY, data.VoxelZ);

        Debug.Log(data.Voxel);

        Chunk.SetVoxel(chunkData, voxelPosition, data.Voxel);
        chunkData.Modified = true;

        _map.ReconstructModifiedChunk(chunkData, voxelPosition);
    }
}
