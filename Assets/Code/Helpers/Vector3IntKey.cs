using UnityEngine;

public struct Vector3IntKey
{
    public int x;
    public int y; 
    public int z;

    public Vector3IntKey(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3IntKey(Vector3Int vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(x, y, z);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector3IntKey other && x == other.x && y == other.y && z == other.z; 
    }

    public override int GetHashCode()
    {
        return x * 73856093 ^ y * 19349663 ^ z * 83492791;
    }
    public override string ToString()
    {
        return $"{x},{y},{z}";
    }
}
