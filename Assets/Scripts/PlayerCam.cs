using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("References")]
    public Transform orentation;    
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationspeed;

    public GameObject regularCam;
    public GameObject zoomCam;

    public Transform zoom;

    public cameraStlye cst;
    public enum cameraStlye
    {
        Regular,zoomed
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        
        
        Vector3 direction = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orentation.forward = direction.normalized;
        
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orentation.forward * vInput + orentation.right * hInput;
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationspeed);
        }
    

        
    }
    
}
