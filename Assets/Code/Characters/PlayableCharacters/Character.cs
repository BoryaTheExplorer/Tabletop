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
    public List<Skill> SkillList { get; private set; } = BasicSkillsProvider.BasicSkills.Select(s => new Skill(s)).ToList();

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
        if (skillList != null)
            this.SkillList = skillList;
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
    public void SetSkillProficiency(string skillName, ProficiencyType proficiency)
    {
        Skill skill = SkillList.Where(q => q.Name == skillName).FirstOrDefault();

        if (skill != null)
        {
            skill.SetProficiency(proficiency);
        }
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
