using Unity.Netcode;
using UnityEngine;

public class NetworkMessageReceiver : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ReceiveChatMessage", HandleChatMessage);
    }

    public void HandleChatMessage(ulong senderId, FastBufferReader reader)
    {
        MessageContent content;
        reader.ReadValueSafe(out content);

        Debug.Log(content.Sender);
        Debug.Log(content.RollMessage.Outcomes.Length);
        Debug.Log("Sum: " + content.RollMessage.Outcomes[0] + content.RollMessage.Outcomes[1]);
    }
}
