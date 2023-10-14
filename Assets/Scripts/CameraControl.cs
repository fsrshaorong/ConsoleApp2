using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraControl : NetworkBehaviour
{
    public GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        var netObject = GetComponent<NetworkObject>();

        if (netObject != null /*&& netObject.OwnerClientId == netObject.NetworkManager.ConnectedClientsIds*/)
        {
            // 这是本地玩家
            playerCamera.SetActive(true);
        }
        else
        {
            // 这不是本地玩家
            playerCamera.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
