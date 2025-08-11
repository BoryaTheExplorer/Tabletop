using UnityEngine;

public class ChatTester : MonoBehaviour
{
    [SerializeField] private NetworkMessageSender _messageSender;

    public void SendTestMessage()
    {
        MessageRequest request = new MessageRequest("Test Sender", MessageType.RollMessage, rollData: new RollMessageRequestData(RollType.PlainRoll, "2d8+3d6+4d4"));
        _messageSender.SendMessageServerRpc(request);
    }
}
