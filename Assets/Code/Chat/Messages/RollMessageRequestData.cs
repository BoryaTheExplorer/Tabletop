using Unity.Netcode;
using UnityEngine;

public struct RollMessageRequestData : INetworkSerializable
{
    public RollType RollType;
    public string Dice;
    public byte[] Modifiers;

    public RollMessageRequestData(RollType rollType, string dice, byte[] modifiers = default)
    {
        RollType = rollType;
        Dice = dice;
        Modifiers = modifiers;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref RollType);
        serializer.SerializeValue(ref Dice);

        if (Modifiers == null || Modifiers.Length == 0)
            return;

        serializer.SerializeValue(ref Modifiers);
    }
}
