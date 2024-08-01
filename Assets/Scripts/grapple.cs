using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapple : MonoBehaviour
{
    [Header("refs")]
    private movement m;
    public Transform cam;
    public Transform orentation;
    public Transform tip;
    public LayerMask whatGrapple;
    public LineRenderer lr;

    [Header("grappling")]
    public float maxDist;
    public float grappleDelay;
    public float overshootY;

    private Vector3 grapplePoint;
    [Header("cooldown")]
    public float Cool;
    private float coolTimer;
    bool grappling;

    [Header("inputs")]
    public KeyCode grappleKey = KeyCode.Mouse2;

    private void Start()
    {
        m = GetComponent<movement>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(grappleKey)) StartGrapple();
        if (coolTimer > 0) 
        {
            coolTimer -= Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        if(grappling) lr.SetPosition(0,tip.position);
    }
    private void StartGrapple()
    {
        if(coolTimer > 0) return;

        grappling = true;
        m.freeze = true;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, orentation.forward, out hit, maxDist, whatGrapple))
        {
            grapplePoint = hit.point;
            Invoke(nameof(whileGrapple), grappleDelay);
        }
        else 
        { 
            //grapplePoint = cam.position + orentation.forward * maxDist;
            //Invoke(nameof(EndGrapple), grappleDelay);
            grappling = false;
            m.freeze = false;
        }
        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }
    private void whileGrapple()
    {
        m.freeze = false;
        Vector3 playerlowest = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float gpRelYpos = grapplePoint.y - playerlowest.y;
        float highestPoint = gpRelYpos + overshootY;
        if(gpRelYpos < 0) highestPoint = overshootY;
        m.jumpToPos(grapplePoint,highestPoint);
        Invoke(nameof(EndGrapple), 1f);
    }
    public void EndGrapple()
    {
        grappling = false;
        coolTimer = Cool;
        lr.enabled = false;
        

    }
}
