using System.Collections.Generic;
using UnityEngine;

public static class DiceParser
{
    public static Dictionary<DiceType, int> ParseDiceFromString(string dice)
    {
        Dictionary<DiceType, int> diceDictionary = new Dictionary<DiceType, int>();

        string lowerCase = dice.ToLower();
        string[] parts = lowerCase.Split("+");

        int amount = 0;
        int sides = 0;

        DiceType type;
        
        string[] split;

        foreach (string s in parts)
        {
            split = s.Split('d');
            int.TryParse(split[0], out amount);
            int.TryParse(split[1], out sides);
            
            type = (DiceType)sides;

            if (diceDictionary.ContainsKey(type))
                diceDictionary[type] += amount;
            else
                diceDictionary[type] = amount;
        }

        return diceDictionary;
    }
}
