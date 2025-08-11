using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkMessageSender : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsServer)
            MessageDataConstructorAndSender.MessageSender = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendMessageServerRpc(MessageRequest request)
    {
        MessageDataConstructorAndSender.BuildAndSendMessage(request);
    }

    public void SendChatMessageToClients(MessageContent content)
    {
        using var writer = new FastBufferWriter(256, Allocator.Temp);

        writer.WriteValueSafe(content);
        NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll("ReceiveChatMessage" ,writer);
    }
}
