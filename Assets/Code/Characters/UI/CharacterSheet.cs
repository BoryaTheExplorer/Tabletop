using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CharacterSheet : MonoBehaviour
{
    private Character _character;
    [SerializeField] private NetworkMessageSender _messageSender;
    //###############MAIN PAGE###################
    [Header("Name")]
    [SerializeField] private TMP_InputField _name;

    [Header("Class Icons")]
    [SerializeField] private ClassIcon _classIconPrefab;
    [SerializeField] private Transform _iconTarget;

    [Header("Ability Scores")]
    [SerializeField] private SerializableDictionary<AbilityScore, TextMeshProUGUI> _abilityScoresSerializable = new SerializableDictionary<AbilityScore, TextMeshProUGUI>();
    private Dictionary<AbilityScore, TextMeshProUGUI> _abilityScores = new Dictionary<AbilityScore, TextMeshProUGUI>();

    [Header("Combat Stats")]
    [SerializeField] private TextMeshProUGUI _armorClass;
    [SerializeField] private TextMeshProUGUI _speed;
    [SerializeField] private TextMeshProUGUI _initiative;

    //################SKILLS PAGE#####################
    [Header("Skills")]
    [SerializeField] private SkillUI _skillUIPrefab;
    [SerializeField] private Transform _skillUITarget;
    private List<SkillUI> _skillUIs = new List<SkillUI>();

    public void Start()
    {
        _character = new Character("Borys, Knight of the Vale", new Health(10));
        
        _character.AddClassLevel(CharacterClassKey.Paladin);
        _character.AddClassLevel(CharacterClassKey.Paladin);
        _character.AddClassLevel(CharacterClassKey.Paladin);

        _character.AddClassLevel(CharacterClassKey.Rogue);
        //MAIN PAGE
        {
            if (_name != null)
            _name.SetTextWithoutNotify(_character.Name);

            // Ability Scores
            {
                _abilityScores = _abilityScoresSerializable.ToDictionary();

                foreach (var score in _abilityScores)
                {
                    score.Value.text = ((_character.AbilityScores[score.Key] >= 10) ? "+" : "") + ((_character.AbilityScores[score.Key] - 10) / 2).ToString();
                    Debug.Log("Ability Score: " + score.Key + " | " + score.Value);
                }
            }
            //Character Classes
            {
                ClassIcon icon;

                foreach (var characterClass in _character.CharacterClasses)
                {
                    icon = Instantiate(_classIconPrefab, _iconTarget);
                    icon.Init(characterClass.Key, characterClass.Level);
                }
            }
            //Combat Stats
            {
                _armorClass.text = _character.ArmorClass.Total.ToString();
                _speed.text = _character.Speed.ToString();
                _initiative.text = ((_character.Initiative >= 0) ? "+" : "") + _character.Initiative.ToString();
            }
        }

        //SKILLS PAGE 
        {
            SkillUI skillUI;

            foreach (var skill in _character.Skills.Values)
            {
                skillUI = Instantiate(_skillUIPrefab, _skillUITarget);
                skillUI.Init(skill.Proficiency, skill.Name, _character.GetSkillBonus(skill.Name));
                skillUI.OnRollButtonPressed += SkillUI_OnRollButtonPressed;
                skillUI.OnProficiencyButtonPressed += SkillUI_OnProficiencyButtonPressed;
                _skillUIs.Add(skillUI);
            }
        }
    }

    private void SkillUI_OnProficiencyButtonPressed(ProficiencyType proficiency, string skillName)
    {
        _character.SetSkillProficiency(skillName, proficiency);
        _skillUIs.Where(q => q.SkillNameKey == skillName).FirstOrDefault().SetRollBonusText(_character.GetSkillBonus(skillName));
    }

    private void SkillUI_OnRollButtonPressed(string obj)
    {
        MessageRequest request = new MessageRequest(NetworkManager.Singleton.LocalClientId,
                                                    MessageType.RollMessage,
                                                    rollData: new RollMessageRequestData(RollType.SkillCheck, "1d20", new byte[] { (byte)_character.GetSkillBonus(obj) }));
        _messageSender.SendMessageServerRpc(request);
    }
}
