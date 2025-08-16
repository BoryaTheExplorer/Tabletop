using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersData : NetworkBehaviour
{
    public static PlayersData Instance { get; private set; }

    private Dictionary<ulong, string> _playerNames = new Dictionary<ulong, string>();
    public Dictionary<ulong, string> PlayerNames {  get { return _playerNames; } }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public override void OnNetworkSpawn()
    {
        SetPlayerNameServerRpc(GameSession.ClientName);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerNameServerRpc(string name, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        _playerNames[clientId] = name;
        Debug.Log(clientId + " | " +  name);
    }
    [ClientRpc]
    public void UpdatePLayerNameClientRpc(ulong clientId, string name)
    {
        _playerNames[clientId] = name;
    }
}
