using Unity.Netcode;
using UnityEngine;

public struct RollMessageRequestData : INetworkSerializable
{
    public RollType RollType;
    public string Dice;

    public RollMessageRequestData(RollType rollType, string dice)
    {
        RollType = rollType;
        Dice = dice;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref RollType);
        serializer.SerializeValue(ref Dice);
    }
}
