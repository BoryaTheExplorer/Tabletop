using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BasicSkillsProvider
{
    public static List<Skill> BasicSkills = new List<Skill> {
        new Skill(BasicSkill.Acrobatics,     AbilityScore.Dexterity),
        new Skill(BasicSkill.AnimalHandling, AbilityScore.Wisdom),
        new Skill(BasicSkill.Athletics,      AbilityScore.Strength),
        new Skill(BasicSkill.Arcana,         AbilityScore.Intelligence),
        new Skill(BasicSkill.Deception,      AbilityScore.Charisma),
        new Skill(BasicSkill.History,        AbilityScore.Intelligence),
        new Skill(BasicSkill.Insight,        AbilityScore.Wisdom),
        new Skill(BasicSkill.Intimidation,   AbilityScore.Charisma),
        new Skill(BasicSkill.Investigation,  AbilityScore.Intelligence),
        new Skill(BasicSkill.Nature,         AbilityScore.Wisdom),
        new Skill(BasicSkill.Medicine,       AbilityScore.Wisdom),
        new Skill(BasicSkill.Perception,     AbilityScore.Wisdom),
        new Skill(BasicSkill.Performance,    AbilityScore.Charisma),
        new Skill(BasicSkill.Persuasion,     AbilityScore.Charisma),
        new Skill(BasicSkill.Religion,       AbilityScore.Intelligence),
        new Skill(BasicSkill.SleightOfHand,  AbilityScore.Dexterity),
        new Skill(BasicSkill.Stealth,        AbilityScore.Dexterity),
        new Skill(BasicSkill.Survival,       AbilityScore.Wisdom)
    };
    public static List<Skill> GetBasicSkills()
    {
        return BasicSkills.Select(s => new Skill(s)).ToList();
    }
}
