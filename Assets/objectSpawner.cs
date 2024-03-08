using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSpawner : MonoBehaviour
{
    public GameObject spawnedObject;
    public int timeBeforeDespawn;
    public int timerStartDelay;
    private bool allowedToSpawn = false;
    public bool overrideProjectObjects;
    public Vector3 overrideDirection;

    private void Start()
    {
        StartCoroutine(timerDelay());
    }

    IEnumerator timerDelay()
    {
        yield return new WaitForSeconds(timerStartDelay);
        allowedToSpawn = true;
    }

    private IEnumerator spawnObject(GameObject obj, int time)
    {
        allowedToSpawn = false;
        if (overrideProjectObjects)
        {
            GameObject o = Instantiate(obj, transform.position, transform.rotation);
            o.GetComponent<projectObject>().moveDirection = overrideDirection;
            yield return new WaitForSeconds(time);
            o.transform.DetachChildren();
            Destroy(o);
            allowedToSpawn = true;
        }
        else
        {
            GameObject o = Instantiate(obj, transform.position, transform.rotation);
            yield return new WaitForSeconds(time);
            o.transform.transform.DetachChildren();
            Destroy(o);
            allowedToSpawn = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (allowedToSpawn)
            StartCoroutine(spawnObject(spawnedObject, timeBeforeDespawn));
    }
}
