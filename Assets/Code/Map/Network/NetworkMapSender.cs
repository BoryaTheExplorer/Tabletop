using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkMapSender : NetworkBehaviour
{
    private const int MAX_PAYLOAD = 1200;
    private const float ACK_TIMEOUT = 5f;

    private readonly Dictionary<Vector3Int, Coroutine> _pendingAcks = new Dictionary<Vector3Int, Coroutine>();

    public void SendMapKeyToClient(ulong clientId, string mapKey)
    {
        using var writer = new FastBufferWriter(FastBufferWriter.GetWriteSize(mapKey, true), Allocator.Temp);
        writer.WriteValueSafe(mapKey);
        NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("OnReceiveMapKey", clientId, writer);
    }
    public void SendMapToClient(ulong clientId, List<SerializableChunkData> chunksData, string mapKey)
    {
        SendMapKeyToClient(clientId, mapKey);
     
        int chunkSize = MapRegister.Map.ChunkSize;
        int chunkHeight = MapRegister.Map.ChunkHeight;

        foreach (var chunk in chunksData)
        {
            byte[] raw = chunk.SerializeToBytes(chunkSize, chunkHeight);
            byte[] compressed = Compressor.Compress(raw);

            _pendingAcks[chunk.WorldPosition] = StartCoroutine(WaitForAck(chunk.WorldPosition, clientId));
            SendCompressedBlob(chunk.WorldPosition, compressed, clientId);
        }
    }

    private void SendCompressedBlob(Vector3Int chunkPosition, byte[] blob, ulong clientId)
    {
        int totalMsgs = Mathf.CeilToInt((float)blob.Length / MAX_PAYLOAD);

        for (int i = 0; i < totalMsgs; i++)
        {
            int offset = i * MAX_PAYLOAD;
            int size = Mathf.Min(MAX_PAYLOAD, blob.Length - offset);
            byte[] slice = new byte[size];

            Array.Copy(blob, offset, slice, 0, size);

            using var writer = new FastBufferWriter(size + 32, Allocator.Temp);

            writer.WriteValueSafe(chunkPosition.x);
            writer.WriteValueSafe(chunkPosition.y);
            writer.WriteValueSafe(chunkPosition.z);

            writer.WriteValueSafe(i);
            writer.WriteValueSafe(totalMsgs);

            writer.WriteBytesSafe(slice, size);

            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("OnReceiveChunk", clientId, writer);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientAckChunkServerRpc(int x, int y, int z, ServerRpcParams rpcParams = default)
    {
        var pos = new Vector3Int(x, y, z);
        if (_pendingAcks.TryGetValue(pos, out var coroutine))
        {
            StopCoroutine(coroutine);
            _pendingAcks.Remove(pos);

            Debug.Log("Client Received chunk");
        }
    }

    private IEnumerator WaitForAck(Vector3Int pos, ulong clientId)
    {
        yield return new WaitForSeconds(ACK_TIMEOUT);

        _pendingAcks.Remove(pos);
        Debug.Log("Client didn't receive chunk");
    }
}
