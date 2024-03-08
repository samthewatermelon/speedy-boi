using UnityEngine;
using System.Collections;

public class GrapplingGun : MonoBehaviour {

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    public float maxDistance = 100f;
    private SpringJoint joint;

    public bool grappling = false;

    private GameObject currentGrappleObject;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !grappling)
        {
            StartGrapple();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopGrapple();
            currentGrappleObject = null;
        }
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable) && !grappling)
        {
            currentGrappleObject = hit.collider.gameObject;
            currentGrappleObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        RaycastHit hit2;
        if (Physics.Raycast(camera.position, camera.forward, out hit2))
        {
            if (hit2.collider.gameObject != currentGrappleObject)
            {
                currentGrappleObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                //currentGrappleObject = null;
            }
        }
        grapplePoint = currentGrappleObject.transform.position;
        if (joint != null)
            joint.connectedAnchor = grapplePoint;
    }
            

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) 
        {
            grappling = true;

            grapplePoint = hit.collider.gameObject.transform.position;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.5f; /// was 0.8f
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }
    private IEnumerator resetOrientation()
    {
        do
        {
            player.eulerAngles = new Vector3(Mathf.LerpAngle(player.eulerAngles.x, 0, 0.01f),
                player.eulerAngles.y,
                    Mathf.LerpAngle(player.eulerAngles.z, 0, 0.01f));
            yield return new WaitForEndOfFrame();
        }
        while
        (((player.eulerAngles.x - 0) > 0.01f) && ((player.eulerAngles.z - 0) > 0.01f));
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        grappling = false;
        lr.positionCount = 0;
        Destroy(joint);
        StartCoroutine(resetOrientation());
        //player.eulerAngles = new Vector3(0, player.eulerAngles.y, 0); /// reset player orientation
    }

    private Vector3 currentGrapplePosition;
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;

        //currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 14f);
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, currentGrappleObject.transform.position, Time.deltaTime * 14f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
        //lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
