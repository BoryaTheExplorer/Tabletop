using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    public List<SerializableKVP<TKey, TValue>> List = new List<SerializableKVP<TKey, TValue>>();

    public Dictionary<TKey, TValue> ToDictionary()
    {
        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        foreach (var kv in List)
        {
            if (dict.ContainsKey(kv.Key))
            {
                Debug.LogWarning("Repeating Keys in Serializable Dictionary. Returning default");
                return default;
            }

            dict.Add(kv.Key, kv.Value);
        }

        return dict;
    }
}
