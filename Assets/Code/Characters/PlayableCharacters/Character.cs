using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character
{
    public string Name { get; private set; } = "Character Name";
    public int Level { get; private set; } = 0;
    public Dictionary<AbilityScore, int> AbilityScores { get; private set; } = new Dictionary<AbilityScore, int>() {
        { AbilityScore.Strength,     10 },
        { AbilityScore.Dexterity,    10 },
        { AbilityScore.Constitution, 10 },
        { AbilityScore.Intelligence, 10 },
        { AbilityScore.Wisdom,       10 },
        { AbilityScore.Charisma,     10 }
    };
    public List<Skill> SkillList { get; private set; } = new List<Skill>();

    public Character()
    {
        SkillList = BasicSkillsProvider.BasicSkills;
    }
    public Character(string name, int level, Dictionary<AbilityScore, int> abilityScores, List<Skill> skillList)
    {
        this.Name = name;
        this.Level = level;
        this.AbilityScores = abilityScores;
        this.SkillList = skillList;
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
}
