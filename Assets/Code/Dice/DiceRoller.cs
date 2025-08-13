using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public static DiceRoller Instance { get; private set; }

    [SerializeField] private float _minAngular;
    [SerializeField] private float _maxAngular;

    [SerializeField] private List<DiceType> _diceTypes = new List<DiceType>();
    [SerializeField] private List<GameObject> _prefabs = new List<GameObject>();
    private Dictionary<DiceType, GameObject> _dicePrefabs = new Dictionary<DiceType, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

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

    public Dice SpawnDie(DiceType type, Vector3 position, Quaternion rotation)
    {
        Debug.Log(type);

        GameObject die = Instantiate(_dicePrefabs[type], position, rotation);
        NetworkObject networkDie = die.GetComponent<NetworkObject>();
        networkDie.Spawn();

        Dice dice = die.GetComponent<Dice>();
        dice.Roll(Vector3.zero, GetRandomAngularVelocity(_minAngular, _maxAngular));

        return dice;
    }

    public async Task<int[]> RollDice(DiceType diceType, int amount)
    {
        Debug.Log("In Dice Roller");
        await Awaitable.WaitForSecondsAsync(0);

        List<int> diceResults = new List<int>();
        var tasks = new List<Task>();

        Vector3 position = new Vector3(32f, 32f, 32f);
        
        for (int i = 0; i < amount; i++)
        {
            var tcs = new TaskCompletionSource<int>();
            var die = SpawnDie(diceType, position, Quaternion.identity);
            
            void HandleDie(int result)
            {
                diceResults.Add(result);
                die.OnDiceRolled -= HandleDie;
                tcs.TrySetResult(result);
            }

            die.OnDiceRolled += HandleDie;
            tasks.Add(tcs.Task);
        }

        await Task.WhenAll(tasks);
        return diceResults.ToArray();
    }


    private Vector3 GetRandomAngularVelocity(float min, float max)
    {
        float x = UnityEngine.Random.Range(min, max);
        float y = UnityEngine.Random.Range(min, max);
        float z = UnityEngine.Random.Range(min, max);

        Vector3 angular = new Vector3(x, y, z);

        Debug.Log(angular);

        return angular;
    }
}
