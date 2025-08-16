using Unity.Netcode;
using UnityEngine;

public struct MessageRequest : INetworkSerializable
{
    public ulong Sender;
    public MessageType MessageType;

    public PlainMessageRequestData PlainMessageRequestData;
    public RollMessageRequestData RollMessageRequestData;
    
    public MessageRequest(ulong sender, MessageType type,  PlainMessageRequestData plainData = default, RollMessageRequestData rollData = default)
    {
        Sender = sender;
        MessageType = type;

        switch (MessageType)
        {
            case MessageType.PlainMessage:
                PlainMessageRequestData = plainData;
                RollMessageRequestData = default;
                break;
            case MessageType.RollMessage:
                RollMessageRequestData = rollData;
                PlainMessageRequestData = default;
                break;
            default:
                PlainMessageRequestData = default;
                RollMessageRequestData = default;
                break;
        }
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Sender);
        serializer.SerializeValue(ref MessageType);

        switch (MessageType)
        {
            case MessageType.PlainMessage:
                serializer.SerializeValue(ref  PlainMessageRequestData);
                break;
            case MessageType.RollMessage:
                serializer.SerializeValue(ref RollMessageRequestData);
                break;
        }
    }
}
