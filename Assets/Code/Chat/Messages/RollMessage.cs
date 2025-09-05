using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollMessage : ChatMessage
{
    [SerializeField] private Transform _diceSpriteParent;
    [SerializeField] private DiceSpriteDictionarySO _diceSpriteDictionary;
    [SerializeField] private RollMessageDiceRolledUI _dice_UIPrefab;

    public void Init(string sender, Dictionary<DiceType, int[]> diceData, int[] modifiers = null)
    {
        _sender.text = sender;
        
        int sum = 0;
        RollMessageDiceRolledUI dice_ui;

        foreach (var dice in diceData)
        {
            dice_ui = Instantiate(_dice_UIPrefab, _diceSpriteParent);
            dice_ui.Setup(_diceSpriteDictionary.GetSprite(dice.Key), dice.Value.Length);

            foreach (var outcome in dice.Value)
            {
                sum += outcome;
            }
        }

        _message.text = sum.ToString();
        
        if (modifiers == null) 
            return;

        int mod = 0;
        foreach (int modifier in modifiers)
            mod += modifier;

        _message.text += " + " + mod.ToString();
    }
}
