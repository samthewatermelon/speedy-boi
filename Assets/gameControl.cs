using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class gameControl : NetworkBehaviour
{
    private void Start()
    {
        if (!isServer)
            gameObject.SetActive(false);
    }

    public void startFirstLevel()
    {
        if (isServer)
            NetworkManager.singleton.ServerChangeScene("level1");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            startFirstLevel();
    }
}
