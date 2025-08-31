using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSheet : MonoBehaviour
{
    private Character _character;
    [Header("Name")]
    [SerializeField] private TMP_InputField _name;
    [Header("Class Icons")]
    [SerializeField] private ClassIcon _classIconPrefab;
    [SerializeField] private Transform _iconTarget;
    [Header("Ability Scores")]
    [SerializeField] private SerializableDictionary<AbilityScore, TextMeshProUGUI> _abilityScoresSerializable = new SerializableDictionary<AbilityScore, TextMeshProUGUI>();
    private Dictionary<AbilityScore, TextMeshProUGUI> _abilityScores = new Dictionary<AbilityScore, TextMeshProUGUI>();

    public void Start()
    {
        _character = new Character("Borys, Knight of the Vale", new Health(10));
        
        _character.AddClassLevel(CharacterClassKey.Paladin);
        _character.AddClassLevel(CharacterClassKey.Paladin);
        _character.AddClassLevel(CharacterClassKey.Paladin);

        _character.AddClassLevel(CharacterClassKey.Rogue);

        if (_name != null)
            _name.SetTextWithoutNotify(_character.Name);

        _abilityScores = _abilityScoresSerializable.ToDictionary();

        foreach (var score in _abilityScores)
        {
            score.Value.text = ((_character.AbilityScores[score.Key] >= 10) ? "+" : "") + ((_character.AbilityScores[score.Key] - 10) / 2).ToString();
            Debug.Log("Ability Score: " + score.Key + " | " + score.Value);
        }

        ClassIcon icon;

        foreach (var characterClass in _character.CharacterClasses)
        {
            icon = Instantiate(_classIconPrefab, _iconTarget);
            icon.Init(characterClass.Key, characterClass.Level);
        }
    }
}
