using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    [SerializeField] private MessageParser _messageParser;
    [SerializeField] private TMP_InputField _inputField;
    
    public void SendChatMessage()
    {
        Debug.Log(_inputField.text.Length == 0);
        if (_inputField.text.Length == 0)
            return;

        _messageParser.TryParseAndSendMessage(NetworkManager.Singleton.LocalClientId, _inputField.text);
        _inputField.text = string.Empty;
    }
}
