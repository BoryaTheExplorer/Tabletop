using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _sender;
    [SerializeField] protected TextMeshProUGUI _message;

    private void Awake()
    {
        if (!_sender)
            Debug.LogWarning("Prefab is missing _sender");
        if (!_message)
            Debug.LogWarning("Prefab is missing _message");
    }
}
