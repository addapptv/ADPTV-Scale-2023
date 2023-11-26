using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    public bool stickToGround = false;
    public bool rotateToGround = false;
    public bool movePlayer = true;
    public bool rotateToMoveDirection= false;

    public Camera playerCamera;
    [HideInInspector]
    public Rigidbody rb;
    float xRotation;

    float delta;
    private Vector2 _moveInput;
    private Vector3 _playerMoveInput;
    private Vector3 _moveVector;

    [Header("InputSmoothing")]
    Vector3 _smoothInputVelocity;
    [SerializeField]
    float inputSmoothTime = 0.2f;

    [Header("Physics")]
    public float gravity = 7.8f; //the gravity applied to our character

    public LayerMask groundLayers; //what layers the ground can be

    [Header("Stats")]
    public float speed = 25f; //max speed for basic movement
    public float acceleration = 4f; //how quickly we build speed
    public float turnSpeed = 5f; //how quickly we turn/rotate player
    private Vector3 moveDirection, movepos, targetDir, groundDir; //where to move to

    private Quaternion playerRotation, groundRotation;
    private Vector3 lookDirection;

    public bool isGrounded;
    Vector3 groundAngle;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundDir = -transform.up;
        playerRotation = Quaternion.LookRotation(transform.forward, transform.up);
    }

    void Update()
    {

        delta = Time.deltaTime;

        PlayerMoveInput();
        CheckGrounded();

        if (stickToGround)
        {
            ApplyDownforce();
        }

        if (rotateToGround)
        {
            GroundAngleCheck();
            RotateToGround(groundAngle);
        }

        if (movePlayer)
        {
            MovePlayer();
        }


        if (rotateToMoveDirection)
        {
            RotatePLayer();
/*            RotateMesh(lookDirection);*/
        }

    }

    //input system receiver
    public void ReceiveMoveInput(Vector2 input)
    {
        _moveInput = input;
    }

    void PlayerMoveInput()
    {
        _playerMoveInput = Vector3.SmoothDamp(_playerMoveInput, _moveInput, ref _smoothInputVelocity, inputSmoothTime);
/*        _moveVector = new Vector3(_playerMoveInput.x, 0, _playerMoveInput.y);*/
    }

    //check the angle of the ground below the player
    private void GroundAngleCheck()
    {
/*        RaycastHit HitFront;*/
        RaycastHit HitDown;
        /*        RaycastHit HitBack;*/

        /*        Physics.Raycast(transform.position, transform.forward, out HitFront, 2f, groundLayers);*/

        /*        Physics.Raycast(transform.position, -transform.forward, out HitBack, 2f, groundLayers);*/

        Vector3 hitNormal;

/*        if (HitFront.transform != null)
        {
            HitDir += HitFront.normal;
        }*/
        if (Physics.Raycast(transform.position, -transform.up, out HitDown, 20f, groundLayers))
        {
            hitNormal = HitDown.normal.normalized;
        }
        /*        if (HitBack.transform != null)
                {
                    HitDir += HitBack.normal;
                }*/
        else
        {
            hitNormal = transform.up;
        }

        groundAngle = hitNormal.normalized;

    }

    //check if the player is ground
    void CheckGrounded()
    {
        if(Physics.Raycast(transform.position, -transform.up, 0.3f, groundLayers))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    void ApplyDownforce()
    {
        if (isGrounded)
        {
            _moveVector.y = 0.0f;
        }

        else
        {
            _moveVector.y = -gravity;
        }
    }

    void MovePlayer()
    {

        Vector3 moveVector = transform.TransformDirection(_playerMoveInput) * speed;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveVector.y);


    }

    void RotateToGround(Vector3 groundAngle)
    {
        Vector3 lerpDirection = Vector3.Lerp(transform.up, groundAngle, 0.2f);
        groundRotation = Quaternion.FromToRotation(transform.up, lerpDirection) * transform.rotation;
        
        //TESTING//
        transform.rotation = groundRotation;
    }
    
    void RotateMesh(Vector3 lookDirection)
    {
        Quaternion slerpRotation = Quaternion.LookRotation(lookDirection, transform.up);
        /*        transform.rotation = Quaternion.Slerp(transform.rotation, slerpRotation, speed * d);*/
        playerRotation = Quaternion.Slerp(transform.rotation, slerpRotation, speed * delta);
        transform.rotation = playerRotation;
    }

    void RotatePLayer()
    {
        xRotation -= _playerMoveInput.x * turnSpeed;

        transform.Rotate(0f, _playerMoveInput.x * turnSpeed, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    }

}