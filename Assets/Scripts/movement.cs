using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movement : MonoBehaviour
{
    [Header("movement")]
    private float moveSpeed;
    public float walkS;
    public float runS;
    public float crouchS;
    public float slideS;
    public float wallrunS;

    private float desiredMovespeed;
    private float lastDesiredMoveSpeed;
    

    public float drag;

    [Header("grounded")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask WhatGround;
    bool grounded;
    public float maxSlope;
    private RaycastHit slopeHit;

    [Header("jump")]
    public float jForce;
    public float jCool;
    public float airMult;
    bool jumpAvaill;

    [Header("Crouching")]
    
    public float cYscale;
    private float startYscale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    [Header("health")]
    public float maxHealth;
    public float health=1;
    public Transform respawn;
    public healthBarScript hbs;
    public float damageTaken;
    public bool winCondition;

    public Transform orentation;
    float hInput;
    float vInput;
    Vector3 moveD;
    Rigidbody rb;
    public Animator anim;
    public string move_cont = "MoveCont";
    public string run_anim = "IsRunning";
    private bool isJumping;
    private bool isAttacking;

   

    public moveState state;

    public enum moveState
    {
        freeze,walk,sprint,crouch,air,slide,wallrun
    }
    public bool sliding;
    public bool wallrun;
    public bool freeze;
    public bool activeGrap;
    // Start is called before the first frame update
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        winCondition = false;
        health = maxHealth;
        hbs.setMax(maxHealth);
        rb.freezeRotation = true;
        jumpAvaill = true;
        sliding = false;
        startYscale = transform.localScale.y;


    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, WhatGround);

        MyInput();
        SpeedControl();
        sHandle();
        animHandle();
        
        
        if (grounded && !activeGrap)
        {
            rb.drag = drag;
        }
        else
        {
            rb.drag = 0;
        }
         

    }
    private void FixedUpdate()
    {
        playerMove();
    }
    private void LateUpdate()
    {
        if (health <= 0)
        {
            transform.position = respawn.position;

            health = maxHealth;
            hbs.setMax(maxHealth);
        }
    }
    private void MyInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && jumpAvaill && grounded)
        {
            jumpAvaill = false;
            jump();
            Invoke(nameof(resetjump), jCool);
        }
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, cYscale, transform.localScale.z);
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x,startYscale , transform.localScale.z);
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }
        if(Input.GetKey(KeyCode.Mouse0) && grounded)
        {

        }

    }

    private void playerMove()
    {
        if(activeGrap) return;
        moveD = orentation.forward * vInput + orentation.right * hInput;
        //grounded movement
        if (grounded)
        {
            rb.AddForce(moveD.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveD.normalized * moveSpeed * 10f * airMult, ForceMode.Force);
        }
        

    }
   

    private void SpeedControl()
    {
        if (activeGrap) return;
        Vector3 flat = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flat.magnitude > moveSpeed)
        {
            Vector3 lim = flat.normalized * moveSpeed; 
            rb.velocity = new Vector3(lim.x,rb.velocity.y, lim.z);
        }
    }

    private void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jForce, ForceMode.Impulse);
    }
    private void resetjump()
    {
        jumpAvaill = true;
    }
    private bool enableMovement;
    public void jumpToPos(Vector3 targetPosition, float trajectoryH)
    {
        activeGrap = true;
        velocityToBe= CalculateJumpVelocity(transform.position,targetPosition,trajectoryH);
        Invoke(nameof(SetVelo), 0.1f);
        Invoke(nameof(restrictionReset), 3f);
    }
    private Vector3 velocityToBe;
    private void SetVelo()
    {
        enableMovement = true;
        rb.velocity = velocityToBe;
    }
    public void restrictionReset()
    {
        activeGrap = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovement) 
        {
            enableMovement = false;
            restrictionReset();
            GetComponent<grapple>().EndGrapple();
        }
    }
    public void resetAttacks() 
    { 
        anim.SetBool("isAttack",false);
    }

    private void sHandle()
    {
        if (freeze) 
        {
            state = moveState.freeze;
            moveSpeed = 0f;
            rb.velocity = Vector3.zero;
        }
        else if (wallrun)
        {
            state = moveState.wallrun;
            desiredMovespeed = wallrunS;

        }
        else if (sliding)
        {
            state = moveState.slide;
            desiredMovespeed = slideS;
        }
        else if ((grounded && Input.GetKey(sprintKey)))
        {
            state = moveState.sprint;
            desiredMovespeed = runS;
            
        }
        else if(grounded && Input.GetKey(crouchKey))
        {
            state = moveState.crouch;
            desiredMovespeed = crouchS;
        }
        else if(grounded)
        {
            state = moveState.walk;
            desiredMovespeed = walkS;
        }
        else
        {
            state = moveState.air;
        }

        if(Mathf.Abs(desiredMovespeed - lastDesiredMoveSpeed) > 20f && moveSpeed !=0)
        {
            StopAllCoroutines();
            StartCoroutine(momentumCl());
        } else
        {
            moveSpeed = desiredMovespeed;
        }

        lastDesiredMoveSpeed = desiredMovespeed;

    }
    private void animHandle()
    {
        float mag = Mathf.Clamp(moveSpeed, 0f, runS) / runS; ;
        if(Mathf.Abs(hInput+vInput) > 0)
        {
            mag = Mathf.Clamp(moveSpeed, 0f, runS) / runS;
            anim.SetFloat(move_cont, mag,0.1f,Time.deltaTime); 
        }
        else
        {
            
            anim.SetFloat(move_cont, 0,0.1f,Time.deltaTime);
        }
        if (!grounded)
        {
            anim.SetBool("isGrounded", false);
            if (rb.velocity.y >= 0f)
            {
               anim.SetBool("isJumping",true);
               anim.SetBool("isFalling",false);
            }
            else
            {
                
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", true);
            }
        }
        if (grounded) 
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isGrounded", true);
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("isAttack", true);
            }
            else 
            {
                Invoke(nameof(resetAttacks), 0.2f);
            }
        }
    }
    
    private IEnumerator momentumCl()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMovespeed - moveSpeed);
        float startValue = moveSpeed;
        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMovespeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }
    }
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }


    //getters
    public float getMoveSpeed()
    {
    return moveSpeed; 
    }

    public bool isGrounded()
    {
    return grounded; 
    }

    public void healthManager()
    {
        if(health <=0)
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            health -= damageTaken;
            hbs.setHealth(health);
        }
        if (other.gameObject.CompareTag("death barrier"))
        {
            health = 0;
            hbs.setHealth(health);
        }
        if (other.gameObject.CompareTag("goal") && GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
        {
            winCondition = true;
        }
    }
}
