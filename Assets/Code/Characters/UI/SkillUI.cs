using System;
using TMPro;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private ProficiencyUI _proficiencyUI;
    [SerializeField] private TextMeshProUGUI _skillName;
    private string _skillNameKey;

    public event Action<ProficiencyType, string> OnButtonPressed;
    private void Start()
    {
        _proficiencyUI.OnButtonPressed += _proficiencyUI_OnButtonPressed;
    }

    public void Init(ProficiencyType proficiency, string skillName)
    {
        _proficiencyUI.FlipTo(proficiency);
        _skillName.text = skillName;
        _skillNameKey = skillName;
    }
    private void _proficiencyUI_OnButtonPressed(ProficiencyType obj)
    {
        OnButtonPressed?.Invoke(obj, _skillNameKey);
    }
}
