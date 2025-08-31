using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassSpriteDictionarySO", menuName = "Scriptable Objects/ClassSpriteDictionarySO")]
public class ClassSpriteDictionarySO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<CharacterClassKey, Sprite> _serializableClassSprites;
    private Dictionary<CharacterClassKey, Sprite> _classSprites;
    public Dictionary<CharacterClassKey, Sprite> ClassSprites { 
        get 
        {
            if (_classSprites == null)
                _classSprites = _serializableClassSprites.ToDictionary();
            
            return _classSprites;
        } }
}
