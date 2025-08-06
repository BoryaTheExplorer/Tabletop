using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    [SerializeField] private List<DiceType> _diceTypes = new List<DiceType>();
    [SerializeField] private List<GameObject> _prefabs = new List<GameObject>();
    private Dictionary<DiceType, GameObject> _dicePrefabs = new Dictionary<DiceType, GameObject>();

    private void Awake()
    {
        Build();
    }

    private void Build()
    {
        if (_diceTypes.Count != _prefabs.Count)
        {
            Debug.Log("ERROR: WRONG AMOUNT OF DICE PREFABS");
            return;
        }

        for (int i = 0; i < _diceTypes.Count; i++)
        {
            _dicePrefabs.Add(_diceTypes[i], _prefabs[i]);
        }
    }

    public Dice SpawnDice(DiceType type, Vector3 position, Quaternion rotation)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            Debug.Log("NOT ALLOWED TO SPAWN DICE");
            return default;
        }

        GameObject die = Instantiate(_dicePrefabs[type], position, rotation);
        NetworkObject networkDie = die.GetComponent<NetworkObject>();
        networkDie.Spawn();

        Dice dice = die.GetComponent<Dice>();
        dice.Roll(Vector3.zero, Vector3.up);

        return dice;
    }
}
