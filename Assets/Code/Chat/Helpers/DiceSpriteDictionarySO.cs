using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DiceSpriteDictionarySO", menuName = "SOs/DiceSpriteDictionarySO")]
public class DiceSpriteDictionarySO : ScriptableObject
{
    [Header("Sprites")]
    [SerializeField] private List<DiceType> _diceTypes = new List<DiceType>();
    [SerializeField] private List<Sprite> _diceSprites = new List<Sprite>();
    public Dictionary<DiceType, Sprite> DiceSpritesDictionary { get; private set; } = new Dictionary<DiceType, Sprite>();

    public Sprite GetSprite(DiceType type)
    {
        if (_diceTypes.Count != _diceSprites.Count)
            return null;

        if (DiceSpritesDictionary.Count < _diceTypes.Count)
        {
            for (int i = 0; i < _diceTypes.Count; i++)
            {
                DiceSpritesDictionary.Add(_diceTypes[i], _diceSprites[i]);
            }
        }

        if (DiceSpritesDictionary.ContainsKey(type))
            return DiceSpritesDictionary[type];

        return null;
    }
}
