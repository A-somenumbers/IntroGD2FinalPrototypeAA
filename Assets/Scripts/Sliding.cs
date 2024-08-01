using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private movement m;
    private float vel;

    [Header("slide controller")]
    public float maxSlide;
    public float slForce;
    private float slideTime;

    public float slideYscale;
    private float startYscale;

    [Header("inputs")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float hInput;
    private float vInput;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m = GetComponent<movement>();
  

        startYscale = playerObj.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (hInput != 0 || vInput != 0) && m.isGrounded())
        {
            SlideStart();
        }
        if (Input.GetKeyUp(slideKey) && m.sliding)
        {
            SlideEnd();
        }
        
       
    }
    private void FixedUpdate()
    {
        if (m.sliding)
        {
            slideMovem();
        }
           
    }

    private void SlideStart()
    {
        m.sliding = true;
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYscale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 10f,ForceMode.Impulse);

        slideTime = maxSlide;

    }
    private void SlideEnd() 
    {
        m.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYscale, playerObj.localScale.z);
        if (m.isGrounded())
        {
            rb.AddForce(Vector3.down * 80f, ForceMode.Impulse);
        }
        
    }
    private void slideMovem()
    {
        Vector3 inputD = orientation.forward * vInput + orientation.right * hInput;

        rb.AddForce(inputD.normalized * slForce, ForceMode.Force);
        slideTime -= Time.deltaTime;
        if (slideTime <= 0)
        {
            SlideEnd();
        }
    }

}
