using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerprefs : MonoBehaviour
{
    public string playerName;

    private void Start()
    {
        playerName = PlayerPrefs.GetString("savedName");
        transform.Find("UI").Find("PlayerName").GetComponent<TMPro.TMP_InputField>().text = playerName;
    }

    public void updatePlayerName()
    {
        playerName = transform.Find("UI").Find("PlayerName").GetComponent<TMPro.TMP_InputField>().text;
        PlayerPrefs.SetString("savedName", playerName);
        GetComponent<Mirror.Discovery.NetworkDiscoveryHUD>().enabled = true;
        transform.Find("UI").Find("PlayerName").gameObject.SetActive(false);
        transform.Find("UI").Find("Go").gameObject.SetActive(false);
    }
}
