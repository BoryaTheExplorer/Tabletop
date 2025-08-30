using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniaturesSO", menuName = "Scriptable Objects/MiniaturesSO")]
public class MiniaturesSO : ScriptableObject
{
    public SerializableDictionary<int, GridObject> SerializableDictionary = new SerializableDictionary<int, GridObject>();
    public Dictionary<int, GridObject> Dictionary { get { return SerializableDictionary.ToDictionary(); } }
    public GridObject GetGridObject(int id)
    {
        if (Dictionary.ContainsKey(id))
            return Dictionary[id];

        return null;
    }
}
