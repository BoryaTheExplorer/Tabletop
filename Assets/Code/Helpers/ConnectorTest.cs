using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectorTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_InputField _joinCodeInputField;

    private const int MIN_NAME_LENGTH = 2;
    public void OnHostButton()
    {
        GameSession.IsHost = true;
        GameSession.ClientName = "GM";
        SceneManager.LoadScene("Table");
    }
    public void OnClientButton()
    {
        if (_nameInputField.text.Length < MIN_NAME_LENGTH)
            return;
        if (_joinCodeInputField.text == string.Empty)
            return;

        GameSession.IsHost = false;
        GameSession.ClientName = _nameInputField.text;
        GameSession.JoinCode = _joinCodeInputField.text;
        SceneManager.LoadScene("Table");
    }
}
