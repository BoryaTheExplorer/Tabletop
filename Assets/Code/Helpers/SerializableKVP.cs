using System;
using UnityEngine;


[Serializable]
public class SerializableKVP<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}
