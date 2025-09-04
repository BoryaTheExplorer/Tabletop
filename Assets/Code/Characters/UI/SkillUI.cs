using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Button _rollButton;
    [SerializeField] private ProficiencyUI _proficiencyUI;
    [SerializeField] private TextMeshProUGUI _skillName;
    private string _skillNameKey;

    public event Action<ProficiencyType, string> OnProficiencyButtonPressed;
    public event Action<string> OnRollButtonPressed;
    private void Start()
    {
        _proficiencyUI.OnButtonPressed += _proficiencyUI_OnButtonPressed;
        _rollButton.onClick.AddListener(_rollButton_OnClick);
    }

    public void Init(ProficiencyType proficiency, string skillName)
    {
        _proficiencyUI.FlipTo(proficiency);
        _skillName.text = skillName;
        _skillNameKey = skillName;
    }
    private void _rollButton_OnClick()
    {
        OnRollButtonPressed?.Invoke(_skillNameKey);
    }
    private void _proficiencyUI_OnButtonPressed(ProficiencyType obj)
    {
        OnProficiencyButtonPressed?.Invoke(obj, _skillNameKey);
    }
}
