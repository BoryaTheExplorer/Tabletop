using System;
using Unity.Netcode;

public class TileDataArray : INetworkSerializable, IEquatable<TileDataArray>
{
    public TileData[] TileData;
    public int GridSize;
    public TileData Get(int x, int y) => TileData[y * GridSize + x];
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        int length = TileData?.Length ?? 0;
        serializer.SerializeValue(ref length);

        if (serializer.IsWriter)
        {
            for (int i = 0; i < length; i++)
                TileData[i].NetworkSerialize(serializer);
        }
        else
        {
            TileData = new TileData[length];

            for (int i = 0; i < length; i++)
            {
                TileData[i] = new TileData();
                TileData[i].NetworkSerialize(serializer);
            }
        }
    }
    public bool Equals(TileDataArray other)
    {
        if (TileData == null && other == null) return true;
        if (TileData == null || other == null) return false;
        if (TileData.Length != other.TileData.Length) return false;

        for (int i = 0; i < TileData.Length;i++)
        {
            if (!TileData[i].Equals(other.TileData[i])) return false;
        }

        return true;
    }
}
