using TMPro;
using UnityEngine;

public class PlainMessage : ChatMessage
{
    public void Init(string sender, string message)
    {
        if (!HasSetup())
            return;

        _sender.text = sender;
        _message.text = message;
    }

    private bool HasSetup()
    {
        bool hasSetup = true;
        if (!_sender)
        {
            Debug.LogWarning("Plain Message Prefab is missing _sender");
            hasSetup = false;
        }
            
        if (!_message)
        {
            Debug.LogWarning("Plaing Message Prefab is missing _message");
            hasSetup = false;
        }

        return hasSetup;
    }
}
