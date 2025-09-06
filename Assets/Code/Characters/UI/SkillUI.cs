using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Button _rollButton;
    [SerializeField] private ProficiencyUI _proficiencyUI;
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _rollBonus;
    private string _skillNameKey;
    public string SkillNameKey {  get { return _skillNameKey; } }

    public event Action<ProficiencyType, string> OnProficiencyButtonPressed;
    public event Action<string> OnRollButtonPressed;
    private void Start()
    {
        _proficiencyUI.OnButtonPressed += _proficiencyUI_OnButtonPressed;
        _rollButton.onClick.AddListener(_rollButton_OnClick);
    }

    public void Init(ProficiencyType proficiency, string skillName, int rollBonus)
    {
        _proficiencyUI.FlipTo(proficiency);
        _skillName.text = skillName;
        _skillNameKey = skillName;
        SetRollBonusText(rollBonus);
    }
    public void SetRollBonusText(int rollBonus)
    {
        _rollBonus.text = ((rollBonus >= 0) ? "+" : "") + rollBonus.ToString();
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
