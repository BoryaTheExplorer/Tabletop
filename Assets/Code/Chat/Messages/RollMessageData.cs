using System;
using Unity.Netcode;
using UnityEngine;

public struct RollMessageData : INetworkSerializable, IMessageData
{
    public RollType RollType;
    public string Dice;
    public byte[] Outcomes;
    public byte[] Modifiers;

    public RollMessageData(RollType rollType, string dice, int[] outcomes, byte[] modifiers = default)
    {
        RollType = rollType;
        Dice = dice;

        byte[] holder = new byte[outcomes.Length];

        for (int i = 0; i < holder.Length; i++)
        {
            holder[i] = (byte)outcomes[i];
        }

        Outcomes = holder;
        Modifiers = modifiers;
    }
    public RollMessageData(RollMessageRequestData data, int[] outcomes)
    {
        RollType = data.RollType;
        Dice = data.Dice;


        byte[] holder = new byte[outcomes.Length];

        for (int i = 0; i < holder.Length; i++)
        {
            holder[i] = (byte)outcomes[i];
        }

        Outcomes = holder;
        
        if (data.Modifiers != default)
        {
            Modifiers = data.Modifiers;
            return;
        }

        Modifiers = Array.Empty<byte>();
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref RollType);
        serializer.SerializeValue(ref Dice);
        serializer.SerializeValue(ref Outcomes);
        serializer.SerializeValue(ref Modifiers);
    }
}
