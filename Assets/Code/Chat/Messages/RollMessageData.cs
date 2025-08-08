using Unity.Netcode;
using UnityEngine;

public struct RollMessageData : INetworkSerializable, IMessageData
{
    public RollType RollType;
    public string Dice;
    public byte[] Outcomes;

    public RollMessageData(RollType rollType, string dice, int[] outcomes)
    {
        RollType = rollType;
        Dice = dice;

        byte[] holder = new byte[outcomes.Length];

        for (int i = 0; i < holder.Length; i++)
        {
            holder[i] = (byte)outcomes[i];
        }

        Outcomes = holder;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref RollType);
        serializer.SerializeValue(ref Dice);
        serializer.SerializeValue(ref Outcomes);
    }
}
