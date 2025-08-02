using Unity.Netcode;
using UnityEngine;

public struct NetworkVector3IntWrapper : INetworkSerializable
{
    public int X;
    public int Y;
    public int Z;

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(X, Y, Z);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref X);
        serializer.SerializeValue(ref Y);
        serializer.SerializeValue(ref Z);
    }
}
