using Unity.Netcode;
using UnityEngine;

public class ChatTester : MonoBehaviour
{
    [SerializeField] private NetworkMessageSender _messageSender;

    public void SendTestMessage()
    {
        MessageRequest request = new MessageRequest(GameSession.ClientName, MessageType.RollMessage, rollData: new RollMessageRequestData(RollType.PlainRoll, "2d8+3d6+4d4"));
        _messageSender.SendMessageServerRpc(request);
    }
}
