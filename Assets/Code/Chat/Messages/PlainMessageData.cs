using Unity.Netcode;
using UnityEngine;

public struct PlainMessageData : INetworkSerializable, IMessageData
{
    public string Message;

    public PlainMessageData(string message)
    {
        Message = message;
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Message);
    }
}
