using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Character
{
    public string Name { get; private set; } = "Character Name";

    public Health Health { get; private set; } = new Health();
    public ArmorClass ArmorClass { get; private set; } = new ArmorClass();
    public int Speed { get; private set; } = 30;
    public int Initiative => (AbilityScores[AbilityScore.Dexterity] - 10) / 2;
    public List<CharacterClass> CharacterClasses { get; private set; } = new List<CharacterClass>();
    public int Level { get; private set; } = 0;
    public Dictionary<AbilityScore, int> AbilityScores { get; private set; } = new Dictionary<AbilityScore, int>() {
        { AbilityScore.Strength,     10 },
        { AbilityScore.Dexterity,    20 },
        { AbilityScore.Constitution, 10 },
        { AbilityScore.Intelligence, 10 },
        { AbilityScore.Wisdom,       10 },
        { AbilityScore.Charisma,     10 }
    };
    public int ProficiencyBonus => 2 + (Level - 1) / 4;
    public Dictionary<string, Skill> Skills { get; private set; } = new Dictionary<string, Skill>();

    public Character()
    {
        ArmorClass.OnPrimaryScalingAbilityChanged += ArmorClass_OnPrimaryScalingAbilityChanged;
        ArmorClass.OnSecondaryScalingAbilityChanged += ArmorClass_OnSecondaryScalingAbilityChanged;
    }
    public Character(string name, Health health, Dictionary<AbilityScore, int> abilityScores = null, List<Skill> skillList = null) : this()
    {
        this.Name = name;
        this.Health = health;

        if (abilityScores != null) 
            this.AbilityScores = abilityScores;

        if (skillList == null)
            skillList = BasicSkillsProvider.BasicSkills.Select(s => new Skill(s)).ToList();    

        foreach (var skill in skillList)
        {
            Skills.Add(skill.Name, skill);
        }
    }
    public void AddClassLevel(CharacterClassKey characterClassKey)
    {
        CharacterClass characterClass = CharacterClasses.Where(q => q.Key == characterClassKey).FirstOrDefault();

        if (characterClass == null)
        {
            characterClass = new CharacterClass(characterClassKey, CharacterClasses.Count == 0, 1);
            CharacterClasses.Add(characterClass);
            characterClass.OnClassLevelUp += CharacterClass_OnClassLevelUp;
            return;
        }

        characterClass.LevelUp();
    }

    private void CharacterClass_OnClassLevelUp(CharacterClassKey arg1, int arg2)
    {
        //Add features depending on class, it's level and it's subclass
    }

    public void SetAbilityScore(AbilityScore abilityScore, int score)
    {
        if (AbilityScores.ContainsKey(abilityScore))
        {
            AbilityScores[abilityScore] = score;
            return;
        }

        Debug.LogWarning($"Ability Score {abilityScore} not found in {Name}'s Ability Scores Dictionary");
    }
    //PROF BONUS
    public int GetProficiencyBonus(ProficiencyType proficiency)
    {
        switch (proficiency)
        {
            case ProficiencyType.None:
                return 0;
            case ProficiencyType.Half:
                return ProficiencyBonus / 2;
            case ProficiencyType.Proficient:
                return ProficiencyBonus;
            case ProficiencyType.Expertise:
                return ProficiencyBonus * 2;
        }
        return 0;
    }
    //SKILLS
    public void SetSkillProficiency(string skillName, ProficiencyType proficiency)
    {
        if (!Skills.ContainsKey(skillName))
            return;
        
        Skills[skillName].SetProficiency(proficiency);
    }
    public void SetSkillRollBonus(string skillName, int bonus)
    {
        if (!Skills.ContainsKey(skillName))
            return;

        Skills[skillName].SetRollBonus(bonus);
    }
    public int GetSkillBonus(string skillName)
    {
        int result = 0;

        if (!Skills.ContainsKey(skillName))
            return 0;

        Skill skill = Skills[skillName];
        result += (AbilityScores[skill.ScalingAbility] - 10) / 2 + skill.RollBonus + GetProficiencyBonus(skill.Proficiency);

        return result;
    }
    //COMBAT
    public void Damage(Damage damage)
    {
        if (damage == null)
            return;

        Health.Damage(damage.Type, damage.Points);
    }

    //AC
    private void ArmorClass_OnPrimaryScalingAbilityChanged(AbilityScore obj)
    {
        if (!AbilityScores.ContainsKey(obj))
            return;

        ArmorClass.SetPrimaryModifier(AbilityScores[obj]);
    }
    private void ArmorClass_OnSecondaryScalingAbilityChanged(AbilityScore obj)
    {
        if (!AbilityScores.ContainsKey(obj))
        {
            ArmorClass.SetSecondaryModifier(0);
            return;
        }

        ArmorClass.SetSecondaryModifier(AbilityScores[obj]);
    }
}
