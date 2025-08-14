using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientServerVoxelEditorUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectToDestroyOnClients = new List<GameObject>(); 
    private void Start()
    {
        if (!NetworkManager.Singleton.IsServer)
            foreach (var obj in _objectToDestroyOnClients)
                Destroy(obj);

        Destroy(this);
    }
}
