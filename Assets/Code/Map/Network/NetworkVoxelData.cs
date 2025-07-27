using Unity.Netcode;
using UnityEngine;

public class NetworkVoxelData : INetworkSerializable
{
    public VoxelType Voxel;

    public int WorldX;
    public int WorldY;
    public int WorldZ;

    public int VoxelX;
    public int VoxelY;
    public int VoxelZ;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue<VoxelType>(ref  Voxel);

        serializer.SerializeValue<int>(ref WorldX);
        serializer.SerializeValue<int>(ref WorldY);
        serializer.SerializeValue<int>(ref WorldZ);

        serializer.SerializeValue<int>(ref VoxelX);
        serializer.SerializeValue<int>(ref VoxelY);
        serializer.SerializeValue<int>(ref VoxelZ);
    }
}
