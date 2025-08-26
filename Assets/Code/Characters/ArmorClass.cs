using System;
using UnityEngine;

public class ArmorClass
{
    public int BaseAC { get; private set; } = 10;
    public AbilityScore PrimaryScalingAbility { get; private set; } = AbilityScore.Dexterity;
    public int PrimaryModifier {  get; private set; } = 0;
    public int PrimaryLimit { get; private set; } = 0;
    public AbilityScore SecondaryScalingAbility { get; private set; } = AbilityScore.Misc;
    public int SecondaryModifier { get; private set; }
    public int SecondaryLimit { get; private set; } = 0;
    public int FlatBonus { get; private set; } = 0;

    public int Total => BaseAC + ((PrimaryLimit > 0) ? Mathf.Min(PrimaryModifier, PrimaryLimit) : PrimaryModifier) 
                               + ((SecondaryLimit > 0) ? Mathf.Min(SecondaryModifier, SecondaryLimit) : SecondaryModifier);

    public event Action<AbilityScore> OnPrimaryScalingAbilityChanged;
    public event Action<AbilityScore> OnSecondaryScalingAbilityChanged;

    public event Action<int> OnPrimaryModifierChanged;
    public event Action<int> OnSecondaryModifierChanged;

    public event Action<int> OnPrimaryLimitChanged;
    public event Action<int> OnSecondaryLimitChanged;

    public void SetBaseAC(int ac)
    {
        if (ac < 0)
            return;

        BaseAC = ac;
    }
    public void SwitchPrimaryScalingAbility(AbilityScore abilityScore)
    {
        if (abilityScore == AbilityScore.Misc)
            return;

        if (abilityScore != PrimaryScalingAbility)
        {
            PrimaryScalingAbility = abilityScore;
            OnPrimaryScalingAbilityChanged?.Invoke(abilityScore);
        }
    }
    public void SetPrimaryModifier(int modifier)
    {
        PrimaryModifier = modifier;
        OnPrimaryModifierChanged?.Invoke(modifier);
    }
    public void SetPrimaryLimit(int limit)
    {
        if (limit < 0)
            return;

        PrimaryLimit = limit;
        OnPrimaryLimitChanged?.Invoke(limit);
    }
    public void SwitchSecondaryScalingAbility(AbilityScore abilityScore)
    {
        if (abilityScore == SecondaryScalingAbility) 
            return;

        SecondaryScalingAbility = abilityScore;
        OnSecondaryScalingAbilityChanged?.Invoke(abilityScore);
    }
    public void SetSecondaryModifier(int modifier)
    {
        SecondaryModifier = modifier;
        OnSecondaryModifierChanged?.Invoke(modifier);
    }
    public void SetSecondaryLimit(int limit)
    {
        if (limit < 0)
            return;

        SecondaryLimit = limit;
        OnSecondaryLimitChanged?.Invoke(limit);
    }
    public void AddFlatBonus(int bonus)
    {
        if (bonus <= 0)
            return;

        FlatBonus += bonus;
    }
    public void SubtractFlatBonus(int bonus)
    {
        if (bonus <= 0) 
            return;

        FlatBonus -= bonus;
    }
}
