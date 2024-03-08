using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportPlayer : MonoBehaviour
{

    public Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.layer == 10)
            other.transform.root.position = destination.position;
    }
}
