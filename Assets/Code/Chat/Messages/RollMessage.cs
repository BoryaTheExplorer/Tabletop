using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollMessage : ChatMessage
{
    [SerializeField] private Transform _diceSpriteParent;
    [SerializeField] private DiceSpriteDictionarySO _diceSpriteDictionary;
    [SerializeField] private Image _dice_UIPrefab;

    public void Init(string sender, Dictionary<DiceType, int[]> diceData, int[] modifiers = null)
    {
        _sender.text = sender;
        int sum = 0;
        Image dice_ui;

        foreach (var dice in diceData)
        {
            dice_ui = Instantiate(_dice_UIPrefab, _diceSpriteParent);
            dice_ui.sprite = _diceSpriteDictionary.GetSprite(dice.Key);

            foreach (var outcome in dice.Value)
            {
                sum += outcome;
            }
        }

        _message.text = sum.ToString();
    }
}
