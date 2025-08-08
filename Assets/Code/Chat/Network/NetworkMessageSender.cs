using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkMessageSender : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void SendMessageServerRpc(MessageContent content)
    {
        SendChatMessageToClients(content);
    }

    public void SendChatMessageToClients(MessageContent content)
    {
        using var writer = new FastBufferWriter(256, Allocator.Temp);

        writer.WriteValueSafe(content);
        NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll("ReceiveChatMessage" ,writer);
    }
}
