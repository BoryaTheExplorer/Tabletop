using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkMapReceiver : NetworkBehaviour
{
    private class Assembly
    {
        public byte[][] Chunks;
        public int Received;
        public int Total;

        public Assembly(int total)
        {
            this.Total = total;
            Received = 0;
            Chunks = new byte[total][];
        }

        public bool IsComplete => Received == Total;

        public byte[] Reassemble()
        {
            int size = 0;
            foreach (var chunk in Chunks) 
                size += chunk.Length;

            var all = new byte[size];
            int offset = 0;

            foreach (var chunk in Chunks)
            {
                Buffer.BlockCopy(chunk, 0, all, offset, chunk.Length);
                offset += chunk.Length;
            }
            return all;
        }

    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            //Destroy(gameObject);
            Debug.Log("Server Removes Map Receiver");
            return;
        }

        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("OnReceiveMapKey", SetCurrentKey);
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("OnReceiveChunk", HandleSlice);
        Debug.Log("Subbed to messsages");
    }

    private readonly Dictionary<string, Dictionary<Vector3Int, Assembly>> _inflight = new Dictionary<string, Dictionary<Vector3Int, Assembly>>();
    private readonly Dictionary<string, Dictionary<Vector3Int, ChunkData>> _mapData = new Dictionary<string, Dictionary<Vector3Int, ChunkData>>();
    private string _mapKey = string.Empty;
    private void Start()
    {
        
    }

    private void SetCurrentKey(ulong senderId, FastBufferReader reader)
    {
        reader.ReadValueSafe(out _mapKey);
        _inflight[_mapKey] = new Dictionary<Vector3Int, Assembly>();
        _mapData[_mapKey] = new Dictionary<Vector3Int, ChunkData>();
        Debug.Log($"Id: {OwnerClientId}, Received Key: {_mapKey}");
    }
    private void HandleSlice(ulong senderId, FastBufferReader reader)
    {
        reader.ReadValueSafe(out int x);
        reader.ReadValueSafe(out int y);
        reader.ReadValueSafe(out int z);

        Vector3Int position = new Vector3Int(x, y, z);

        reader.ReadValueSafe(out int index);
        reader.ReadValueSafe(out int total);

        int remaining = reader.Length - reader.Position;
        byte[] data = new byte[remaining];
        
        reader.ReadBytesSafe(ref data, remaining);

        var inflightMap = _inflight[_mapKey];

        if (!inflightMap.TryGetValue(position, out var asm))
        {
            asm = new Assembly(total);
            inflightMap[position] = asm;
        }

        asm.Chunks[index] = data;
        asm.Received++;
        Debug.Log(_mapKey + " | Received: " + asm.Received);

        if (asm.IsComplete)
        {
            byte[] compressedBlob = asm.Reassemble();
            byte[] raw = Compressor.Decompress(compressedBlob);
            var chunk = SerializableChunkData.DeserializeFromBytes(raw, MapRegistry.Map.ChunkSize, MapRegistry.Map.ChunkHeight);

            ChunkData chunkData = chunk.ToChunkData(MapRegistry.Map);
            _mapData[_mapKey].Add(position, chunkData);
            transform.parent.GetComponentInChildren<NetworkMapSender>().ClientAckChunkServerRpc(x, y, z);

            inflightMap.Remove(position);

            HandleMap();
        }
    }

    private void HandleMap()
    {
        MapData mapData = new MapData(new Dictionary<Vector3Int, ChunkData>(_mapData[_mapKey]), _mapKey);
        int size = MapRegistry.Map.MapSizeInChunks * MapRegistry.Map.MapSizeInChunks;


        if (_mapData[_mapKey].Values.Count == size)
        {
            MapRegistry.SavedMaps.Add(_mapKey, mapData);

            _mapData.Clear();
            _mapKey = string.Empty;
        }
    }
}
