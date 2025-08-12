using Unity.Netcode;
using UnityEngine;

public class RollDiceButtonUI : MonoBehaviour
{
    [SerializeField] private NetworkMessageSender _messageSender;
    public void SendRollMessage(string dice)
    {
        MessageRequest request = new MessageRequest(NetworkManager.Singleton.LocalClientId.ToString(),
                                                    MessageType.RollMessage,
                                                    rollData: new RollMessageRequestData(RollType.PlainRoll, dice));
        _messageSender.SendMessageServerRpc(request);
    }
}
