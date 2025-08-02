using Unity.Netcode;
using UnityEngine;

public struct NetworkVoxelData : INetworkSerializable
{
    public VoxelType Voxel;
    public NetworkVector3IntWrapper ChunkPos;
    public NetworkVector3IntWrapper VoxelPos;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue<VoxelType>(ref  Voxel);
        serializer.SerializeValue(ref ChunkPos);
        serializer.SerializeValue(ref VoxelPos);
    }
}
