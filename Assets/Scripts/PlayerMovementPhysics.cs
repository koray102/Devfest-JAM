using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPhysics : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    [Header("Movement")]
    [SerializeField] private float speedInput;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxSpeed;
    public Transform groundCheckTransform;
    public float radiusOfSphere = 0.3f;
    public LayerMask groundMask;

    [Header("Grab")]
    [SerializeField] private float maxGrabDistance = 10;
    [SerializeField] private LayerMask ignoreRaycast;
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;
    [SerializeField] private ProceduralChain proceduralChain;

    private float restLength;
    private float lastLength;
    private bool grapInput;
    private bool isGrabbing;
    private Vector3 grabPoint;
    private  Vector3 grabLastPosition;
    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private float speed;
    private bool isJumping;
    private Rigidbody playerRb;
    private float horizontal;
    private float vertical;
    private bool isGrounded;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        grapInput = Input.GetKeyDown(KeyCode.E);

        isGrounded = Physics.CheckSphere(groundCheckTransform.position, radiusOfSphere, groundMask);
      
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }else
        {
            speed = speedInput;
        }
        
        if(Input.GetKeyDown("space") && isGrounded)
        {
            //Debug.Log("jump");
            isJumping = true;
        }

        
        if(isGrabbing && grapInput)
        {
            grabbedObject = null;
            grabbedRb = null;
            grabLastPosition = Vector3.zero;
            lastLength = 0;

            isGrabbing = false;
            grapInput = false;
        }

        if(!isGrabbing && grapInput && Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxGrabDistance, ~ignoreRaycast))
        {
            grabbedObject = hit.transform.gameObject;
            grabbedRb = grabbedObject.GetComponent<Rigidbody>();

            Debug.Log(grabbedObject.name + " " + grabbedObject.layer);

            isGrabbing = true;
            restLength = hit.distance;
            grabPoint = hit.point;

            Vector3 surfaceNormal = hit.normal;

            proceduralChain.DetectHookshotCollision(grabPoint, grabbedObject, surfaceNormal);
        }
    }


    void FixedUpdate()
    {   
        PhysicalMovement();

        Jump();

        if(isGrabbing)
        {
            Vector3 grabOffset = Vector3.zero;

            // Calculate the direction from pointA to pointB
            Vector3 direction = grabPoint - transform.position;

            // Draw the line as a ray
            Debug.DrawRay(transform.position, direction, Color.green);

            if(grabbedObject != null && grabbedRb != null && !grabbedObject.isStatic)
            {
                Vector3 grabCurrentPosition = grabbedObject.transform.position;

                if(grabLastPosition.magnitude != 0)
                {
                    grabOffset = grabCurrentPosition - grabLastPosition;
                }
                
                grabLastPosition = grabCurrentPosition;

                grabPoint += grabOffset;
            }
            
            if(proceduralChain.didThrowed)
            {
                proceduralChain.SetLastChainPosition(grabPoint);
            }
            
            SpringSystem(grabPoint);
        }else
        {
            proceduralChain.DestroyChain();
        }
    }


    private void PhysicalMovement()
    {
        Vector3 verticalMoveDirection = cam.transform.forward * vertical;
        Vector3 horizontalMoveDirection = cam.transform.right * horizontal;

        Vector3 totalMove = verticalMoveDirection + horizontalMoveDirection;

        playerRb.AddForce(totalMove.normalized * speed);

        playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed);
    }


    private void Jump()
    {
        if(isJumping)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }


    private void SpringSystem(Vector3 grabPoint)
    {
        float springVelocity = 0;
        Vector3 suspensionForce = Vector3.zero;

        // Damper için yayın hızını hesapla
        float currentSpringLength = Vector3.Distance(transform.position, grabPoint);

        if(lastLength != 0)
        {
            springVelocity = (lastLength - currentSpringLength) / Time.fixedDeltaTime;
        }

        lastLength = currentSpringLength;

        // Yay ve damper kuvvetini hesapla
        float springForce = springStiffness * (restLength - currentSpringLength);
        float damperForce = damperStiffness * springVelocity;
        

        Vector3 springDir = transform.position - grabPoint;
        springDir.Normalize();
            
        if(springForce < 0)
        {
            suspensionForce = (springForce + damperForce) * springDir;
        }

        playerRb.AddForce(suspensionForce);

        if(grabbedRb != null && !grabbedRb.isKinematic)
        {
            grabbedRb.AddForceAtPosition(-suspensionForce, grabPoint);
        }
    }
}
