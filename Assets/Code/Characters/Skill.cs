using UnityEngine;

public class Skill
{
    public string Name { get; private set; } = "Skill";
    public AbilityScore ScalingAbility { get; private set; } = AbilityScore.Strength;
    public ProficiencyType Proficiency { get; private set; } = ProficiencyType.None;
    public int RollBonus { get; private set; } = 0;

    public Skill()
    {

    }
    public Skill(string name, AbilityScore scalingAbility, ProficiencyType proficiency = ProficiencyType.None, int rollBonus = 0)
    {
        this.Name = name;
        this.ScalingAbility = scalingAbility;
        this.Proficiency = proficiency;
        this.RollBonus = rollBonus;
    }
    public Skill(BasicSkill basicSkill, AbilityScore scalingAbility, ProficiencyType proficiency = ProficiencyType.None, int rollBonus = 0)
    {
        string name = basicSkill.ToString();
    }
    public void SetName(string name)
    {
        if (name != string.Empty)
            this.Name = name;
    }
    public void SetScalingAbility(AbilityScore scalingAbility)
    {
        this.ScalingAbility = scalingAbility;
    }
    public void SetProficiency(ProficiencyType proficiency)
    {
        this.Proficiency = proficiency;
    }
    public void SetRollBonus(int rollBonus)
    {
        this.RollBonus = rollBonus;
    }
}
