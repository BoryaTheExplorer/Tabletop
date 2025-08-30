using System;
using UnityEngine;

public class CharacterClass
{
    public CharacterClassKey Key { get; private set; }
    public bool IsPrimary { get; private set; } = true;
    public string SubclassKey { get; private set; } = string.Empty;
    public int Level { get; private set; } = 0;

    public event Action<CharacterClassKey, int> OnClassLevelUp;

    public CharacterClass(CharacterClassKey key, bool isPrimary, int level)
    {
        Key = key;
        IsPrimary = isPrimary;
        Level = level;
    }
    public void SetSubclassKey(string subclassKey)
    {
        SubclassKey = subclassKey;
    }
    public void LevelUp()
    {
        Level++;
        OnClassLevelUp.Invoke(Key, Level);
    }
}
