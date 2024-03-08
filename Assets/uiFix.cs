using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Intro" || SceneManager.GetActiveScene().name == "Lobby")
        {
            Mirror.NetworkManager.singleton.transform.Find("UI").Find("timer").gameObject.SetActive(false);
            Mirror.NetworkManager.singleton.transform.Find("UI").Find("message").gameObject.SetActive(false);
        }
        else
        {
            Mirror.NetworkManager.singleton.transform.Find("UI").Find("timer").gameObject.SetActive(true);
            Mirror.NetworkManager.singleton.transform.Find("UI").Find("message").gameObject.SetActive(true);
        }
    }
}
