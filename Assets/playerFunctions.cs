using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class playerFunctions : NetworkBehaviour
{
    public TMPro.TMP_Text uiMessage;
    public bool autoDeath;
    [SyncVar(hook = nameof(setPlayerName))]
    public string playerName;

    public Vector3 worldScale;

    void setPlayerName(string oldName, string newName)
    {
        transform.Find("PlayerName").Find("Name").GetComponent<TMPro.TMP_Text>().text = newName;
    }

    private void Start()
    {
        worldScale = transform.lossyScale;

        uiMessage = NetworkManager.singleton.transform.Find("UI").Find("message").GetComponent<TMPro.TMP_Text>();
        uiMessage.text = null;

        if (!isLocalPlayer || SceneManager.GetActiveScene().name == "Lobby")
            transform.Find("camera").Find("Main Camera").GetComponent<Camera>().targetDisplay = 2;

        if (!isLocalPlayer && SceneManager.GetActiveScene().name != "Lobby")
            transform.Find("camera").Find("Main Camera").GetComponent<Camera>().transform.tag = "Untagged";

        if (isLocalPlayer && SceneManager.GetActiveScene().name != "Lobby")
            transform.Find("PlayerName").gameObject.SetActive(false);

        if (isLocalPlayer)
            setPlayerName(NetworkManager.singleton.GetComponent<playerprefs>().playerName);
    }

    [Command]
    private void setPlayerName(string newName)
    {
        playerName = newName;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8) //death layer
        {
            StartCoroutine(respawnPlayer("You Died."));
        }
        if (collision.gameObject.layer == 11) //reset layer
        {
            StartCoroutine(loadScene(SceneManager.GetActiveScene().name, "You Died."));
        }
        if (collision.gameObject.layer == 7 && uiMessage.text != "You Died.")
        {
            int.TryParse(SceneManager.GetActiveScene().name.Replace("level",""), out int lvl);
            StartCoroutine(loadScene("level" + (lvl + 1), "You Win!"));
        }
        if (collision.gameObject.layer == 3) //moving platform fix
        {
            transform.SetParent(collision.transform.root, true);
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(worldScale.x / transform.lossyScale.x, worldScale.y / transform.lossyScale.y, worldScale.z / transform.lossyScale.z);
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3) //moving platform fix
        {
            transform.SetParent(null);
            transform.localScale = Vector3.one;
            transform.localScale = worldScale;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -50f && autoDeath)
        {
            StartCoroutine(respawnPlayer("You Died."));
        }
    }

    public IEnumerator respawnPlayer(string message)
    {
        freezePlayer(RigidbodyConstraints.FreezeAll);
        uiMessage.text = message;
        yield return new WaitForSeconds(3f);
        transform.position = NetworkManager.singleton.GetStartPosition().position;
        transform.rotation = NetworkManager.singleton.GetStartPosition().rotation;
        uiMessage.text = "";
        freezePlayer(RigidbodyConstraints.FreezeRotation);
    }

    public IEnumerator loadScene(string sceneName, string message)
    {        
        uiMessage.text = message;
        GetComponent<timer>().complete = true;
        yield return new WaitForSeconds(3f);
        if (isServer)
            NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    public void freezePlayer(RigidbodyConstraints status)
    {
        GetComponent<Rigidbody>().constraints = status;
    }
}
