using System.Collections.Generic;
using UnityEngine;

public class MessageContainer : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlainMessage _plainMessagePrefab;
    public static MessageContainer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnPlainMessage(string sender, string message)
    {
        if (!HasSetup())
            return;

        PlainMessage plainMessage = Instantiate(_plainMessagePrefab, _content);
        plainMessage.Init(sender, message);
    }
    public void SpawnRollMessage(Dictionary<DiceType, int[]> rolls, string sender, RollType rollType)
    {

    }
    private bool HasSetup()
    {
        bool hasSetup = true;

        if (!_content)
        {
            hasSetup = false;
            Debug.LogWarning("MessageContainer is missing _content");
        }

        if (!_plainMessagePrefab)
        {
            hasSetup = false;
            Debug.LogWarning("MessageContainer is missing _plainMessagePrefab");
        }

        return hasSetup;
    }
}
