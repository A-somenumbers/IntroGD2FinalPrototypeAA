using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallrunning : MonoBehaviour
{
    [Header("wallrunning params")]
    public LayerMask WhatGround;
    public LayerMask WhatWall;
    public float wallrunF;
    public float wallJumpUpF;
    public float walljumpSideF;
    public float wallrunT;
    private float walltimer;

    private float hInput;
    private float vInput;

    [Header("wall detect")]
    public float wallCheckDist;
    public float minJumpH;
    private RaycastHit leftcolide;
    private RaycastHit rightcolide;
    private bool left;
    private bool right;
    private bool exiting;
    public float exitwallTime;
    private float exitwallTimer;

    [Header("player references")]
    public Transform orentation;
    private movement m;
    public Rigidbody body;

    public KeyCode jumpKey = KeyCode.Space;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>(); 
        m = GetComponent<movement>();
    }

    // Update is called once per frame
    void Update()
    {
        checkWall();
        states();
    }
    private void FixedUpdate()
    {
        if (m.wallrun)
        {
            wallrunMove();

        }
    }

    private void checkWall()
    {
        right = Physics.Raycast(transform.position, orentation.right, out rightcolide, wallCheckDist, WhatWall);
        left = Physics.Raycast(transform.position, -orentation.right, out leftcolide, wallCheckDist, WhatWall);
    }
    private bool aboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpH,WhatGround);
    }

    private void states()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        //wallrunning state
        if ((left || right) && vInput > 0 && aboveGround() && !exiting)
        {
            if (!m.wallrun)
            {
                startWall();
            }
            /*if(walltimer > 0) wallrunT -= Time.deltaTime;
            if(walltimer > 0 && m.wallrun)
            {
                exiting = true;
                exitwallTimer = exitwallTime;
            } */
            if (Input.GetKeyDown(jumpKey)) walljump();
        } else if (exiting)
        {
            if (m.wallrun) endWall();
            if (exitwallTimer > 0) exitwallTimer -= Time.deltaTime;
            if (exitwallTimer <= 0) exiting = false;
            
        }
        else 
        {
            if (m.wallrun)
            {
                endWall();
            }
        }


    }
    private void startWall()
    {
        m.wallrun = true;
        //walltimer = wallrunT;
    }
    private void wallrunMove()
    {
        body.useGravity = false;
        body.velocity = new Vector3(body.velocity.x, 0f, body.velocity.z);

        Vector3 wallNormal = right ? rightcolide.normal : leftcolide.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal,transform.up);
        if((orentation.forward - wallForward).magnitude > (orentation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        body.AddForce(wallForward * wallrunF, ForceMode.Force);
        if(!(left && hInput > 0) && !(right && hInput < 0))
        {
            body.AddForce(-wallNormal * 100, ForceMode.Force);
        }
        
    }
    private void endWall()
    {
        body.useGravity = true;
        m.wallrun = false;
    }
    private void walljump()
    {
        exiting = true;
        exitwallTimer = exitwallTime;
        Vector3 wallNormal = right ? rightcolide.normal : leftcolide.normal;

        Vector3 appliedForce = transform.up * wallJumpUpF + wallNormal * walljumpSideF;
        body.velocity = new Vector3(body.velocity.x, 0f, body.velocity.z);
        body.AddForce(appliedForce, ForceMode.Impulse);
    }
}
