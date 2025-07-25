using Unity.Netcode;

public struct TileData : INetworkSerializable
{
    public byte Id;
    public float Height;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref Height);
    }
}
