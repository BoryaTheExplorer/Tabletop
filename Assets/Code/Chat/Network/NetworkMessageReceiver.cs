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
        Debug.Log(content.Name);
    }
}
