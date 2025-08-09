using Unity.Netcode;
using UnityEngine;

public struct PlainMessageRequestData : INetworkSerializable
{
    public string Message;
    public PlainMessageRequestData(string message)
    {
        Message = message;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Message);
    }
}
