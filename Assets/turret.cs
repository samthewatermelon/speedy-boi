using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public Transform turretTip;
    public Transform turretBase;
    public float timeBetweenShots;
    public float bulletPower;
    public GameObject targetedPlayer;
    public GameObject bullet;

    private bool canShoot = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.layer == 10)
            targetedPlayer = other.transform.root.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject == targetedPlayer)
            targetedPlayer = null;
    }

    private IEnumerator shoot()
    {
        canShoot = false;
        GameObject b = Instantiate(bullet, turretTip.position, turretBase.rotation);
        b.GetComponent<Rigidbody>().AddForce(b.transform.forward * bulletPower);
        yield return new WaitForSeconds(timeBetweenShots);
        Destroy(b);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedPlayer != null)
            turretBase.LookAt(targetedPlayer.transform.position);

        if (canShoot && targetedPlayer != null)
            StartCoroutine(shoot());
    }
}
