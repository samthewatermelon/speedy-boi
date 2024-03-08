using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ducky : MonoBehaviour
{
    public Vector3 cog;
    private bool quacking = false;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(cog.x, cog.y, cog.z);
    }

    private IEnumerator quack()
    {
        quacking = true;
        yield return new WaitForSeconds(12);
        GetComponent<AudioSource>().Play();
        quacking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!quacking)
            StartCoroutine(quack());
    }
}
