using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSheet : MonoBehaviour
{
    private Character _character;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private SerializableDictionary<AbilityScore, TextMeshProUGUI> _abilityScoresSerializable = new SerializableDictionary<AbilityScore, TextMeshProUGUI>();
    private Dictionary<AbilityScore, TextMeshProUGUI> _abilityScores = new Dictionary<AbilityScore, TextMeshProUGUI>();

    public void Start()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAA");
        _character = new Character("Borys, Knight of the Vale", new Health(10));

        if (_name != null)
            _name.text = _character.Name;

        _abilityScores = _abilityScoresSerializable.ToDictionary();

        foreach (var score in _abilityScores)
        {
            score.Value.text = ((_character.AbilityScores[score.Key] >= 10) ? "+" : "") + ((_character.AbilityScores[score.Key] - 10) / 2).ToString();
            Debug.Log("Ability Score: " + score.Key + " | " + score.Value);
        }
    }
}
