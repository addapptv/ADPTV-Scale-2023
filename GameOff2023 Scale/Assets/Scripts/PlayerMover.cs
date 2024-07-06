using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    public bool checkGrounded = false;
    public bool stickToGround = false;
    public bool groundAngleCheck = false;
    public bool rotateToGround = false;
    public bool movePlayer = false;
    public bool turnPlayer = false;


    [Header("Transforms")]
    [HideInInspector]
    public Rigidbody rb;
    [SerializeField]
    public Transform groundCheck;
    
    
    float xRotation;
    float yRotation;

    [Header("Input")]
    private Vector2 _moveInput;
    private Vector3 _playerMoveInput;
    Vector3 _smoothInputVelocity;
    [SerializeField]
    float inputSmoothTime = 0.2f;

    [Header("Physics")]
    Vector3 moveForward;
    public Vector3 moveDown;
    public float gravity = 10f; //the gravity applied to our character
    private float downForce;

    public LayerMask groundLayers; //what layers the ground can be

    [Header("Stats")]
    public float speed = 50f; //max speed for basic movement
    public float acceleration = 4f; //how quickly we build speed
    public float rotationSpeed = 3f; //how quickly we turn/rotate player

    private Quaternion groundRotation;

    public bool isGrounded;
    public Vector3 groundAngle;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMoveInput();
    }

    void FixedUpdate()
    {
        
        if (checkGrounded)
        {
            CheckGrounded();
        }

        if (groundAngleCheck)
        {
            GroundAngleCheck();
        }

        if (rotateToGround)
        {
            RotateToGround(groundAngle);
        }

        if (stickToGround)
        {
            ApplyDownforce();
        }

        if (movePlayer)
        {
            MovePlayer();
        }

        if (turnPlayer)
        {
            RotatePlayer();
        }

    }

    //input system receiver
    public void ReceiveMoveInput(Vector2 input)
    {
        _moveInput = input;
    }

    //smooth player move input
    void PlayerMoveInput()
    {
        _playerMoveInput = Vector3.SmoothDamp(_playerMoveInput, _moveInput, ref _smoothInputVelocity, inputSmoothTime);
    }

    //check the angle of the ground below the player
    void GroundAngleCheck()
    {
        RaycastHit HitFront;
        RaycastHit HitDown;

        Vector3 downHitNormal;
        Vector3 frontHitNormal;


        if (Physics.Raycast(transform.position, transform.forward, out HitFront, 2f, groundLayers))
        {
            frontHitNormal = HitFront.normal.normalized;
        }

        else
        {
            frontHitNormal = Vector3.zero;
        }

        if (Physics.Raycast(transform.position, -transform.up, out HitDown, 20f, groundLayers))
        {
            downHitNormal = HitDown.normal.normalized;
        }

        else
        {
            downHitNormal = transform.up;
        }


        groundAngle = downHitNormal.normalized + frontHitNormal.normalized;

    }

    //check if the player is on the ground
    void CheckGrounded()
    {
        if (Physics.CheckSphere(groundCheck.position, .2f, groundLayers))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    //apply force downwards
    void ApplyDownforce()
    {

        Vector3 down = -transform.up;

        if (isGrounded)
        {
            downForce = 0.0f;
        }

        else
        {
            downForce = gravity;
        }

        moveDown = down * downForce;


    }

    //apply player movement
    void MovePlayer()
    {

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 desiredMoveDirection = forward * _moveInput.y;

        {
            if (desiredMoveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            moveForward = desiredMoveDirection * speed;
            
            rb.velocity = moveForward + moveDown;
        }

    }

    //rotate player to ground normal
    void RotateToGround(Vector3 groundAngle)
    {
        Vector3 lerpedGroundAngle = Vector3.Lerp(transform.up, groundAngle, 0.5f);

        groundRotation = Quaternion.FromToRotation(transform.up, lerpedGroundAngle) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, groundRotation, rotationSpeed);
    }
    
    //rotate player to 
    void RotatePlayer()
    {
        xRotation -= _playerMoveInput.x * rotationSpeed;

        transform.Rotate(0f, _playerMoveInput.x * rotationSpeed, 0f);

    }

}