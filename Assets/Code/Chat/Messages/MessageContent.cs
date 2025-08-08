using Unity.Netcode;
using UnityEngine;

public struct MessageContent : INetworkSerializable
{
    public string Name;
    public MessageType MessageType;
    public PlainMessageData PlainMessage;
    public RollMessageData RollMessage;

    public MessageContent(string name, MessageType type, PlainMessageData plainData = default, RollMessageData rollData = default)
    {
        Name = name;
        MessageType = type;

        switch (MessageType)
        {
            case MessageType.PlainMessage:
                PlainMessage = plainData;
                RollMessage = default;
                return;
            case MessageType.RollMessage:
                RollMessage = rollData;
                PlainMessage = default;
                break;
            default:
                PlainMessage = default;
                RollMessage = default;
                break;
        }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref MessageType);
        
        switch(MessageType)
        {
            case MessageType.PlainMessage:
                serializer.SerializeValue(ref PlainMessage);
                break;
            case MessageType.RollMessage:
                serializer.SerializeValue(ref RollMessage);
                break;
        }
    }
}
