using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character
{
    public string Name { get; private set; } = "Character Name";

    public Health Health { get; private set; } = new Health();
    public ArmorClass ArmorClass { get; private set; } = new ArmorClass();

    public Dictionary<CharacterClass, int> CharacterClassLevels { get; private set; } = new Dictionary<CharacterClass, int>();
    public Dictionary<AbilityScore, int> AbilityScores { get; private set; } = new Dictionary<AbilityScore, int>() {
        { AbilityScore.Strength,     10 },
        { AbilityScore.Dexterity,    10 },
        { AbilityScore.Constitution, 10 },
        { AbilityScore.Intelligence, 10 },
        { AbilityScore.Wisdom,       10 },
        { AbilityScore.Charisma,     10 }
    };
    public List<Skill> SkillList { get; private set; } = BasicSkillsProvider.BasicSkills.Select(s => new Skill(s)).ToList();

    public Character()
    {
        ArmorClass.OnPrimaryScalingAbilityChanged += ArmorClass_OnPrimaryScalingAbilityChanged;
        ArmorClass.OnSecondaryScalingAbilityChanged += ArmorClass_OnSecondaryScalingAbilityChanged;
    }
    public Character(string name, Health health, Dictionary<AbilityScore, int> abilityScores, List<Skill> skillList) : this()
    {
        this.Name = name;
        this.Health = health;
        this.AbilityScores = abilityScores;
        this.SkillList = skillList;
    }
    public void AddClassLevel(CharacterClass characterClass)
    {
        if (CharacterClassLevels.ContainsKey(characterClass))
        {
            CharacterClassLevels[characterClass] += 1;
            return;
        }

        CharacterClassLevels.Add(characterClass, 1);
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
