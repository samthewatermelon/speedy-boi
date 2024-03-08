using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostPad : MonoBehaviour
{
    public float boostSpeed;
    private float boostMultiplier = 500;

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, -0.01f * boostSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Vector3 dir = transform.up;
            other.attachedRigidbody.AddForce(dir * boostSpeed * boostMultiplier);
        }
    }
}

