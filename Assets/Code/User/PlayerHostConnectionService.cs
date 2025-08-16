using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Services.Relay.Models;

public class PlayerHostConnectionService : MonoBehaviour
{
    [SerializeField] private NetworkMessageSender _messageSender;
    [SerializeField] private int _maxPlayers = 6;
    private const string CONNECTION_TYPE = "dtls";

    private async void Start()
    {
        if (GameSession.IsHost)
        {
            string code = await StartRelayHostAsync();
            
            MessageRequest request = new MessageRequest(NetworkManager.Singleton.LocalClientId, MessageType.PlainMessage,
                                                        plainData: new PlainMessageRequestData(code));
            _messageSender.SendMessageServerRpc(request);
        }
        else
        {
            await StartRelayClientAsync(GameSession.JoinCode);
        }
    }

    private async Task InitializeServices()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized) 
            await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    private async Task<string> StartRelayHostAsync()
    {
        await InitializeServices();

        var allocation = await RelayService.Instance.CreateAllocationAsync(_maxPlayers);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, CONNECTION_TYPE));
        
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        NetworkManager.Singleton.StartHost();

        return joinCode;
    }

    private async Task<bool> StartRelayClientAsync(string joinCode)
    {
        await InitializeServices();

        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, CONNECTION_TYPE));

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}
