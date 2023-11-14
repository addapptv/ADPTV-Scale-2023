using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody Rigid;

    float delta;
    public Vector2 _moveInput;

    [Header("Physics")]
    private float gravityForce = 7.8f; //the gravity applied to our character

    public LayerMask groundLayers; //what layers the ground can be

    [Header("Stats")]
    public float Speed = 15f; //max speed for basic movement
    public float Acceleration = 4f; //how quickly we build speed
    public float turnSpeed = 5f; //how quickly we turn/rotate player
    private Vector3 moveDirection, movepos, targetDir, groundDir; //where to move to

    private Quaternion playerRotation, groundRotation;

    private bool isGrounded = false;
    Vector3 groundAngle;

    void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        groundDir = -transform.up;
        playerRotation = Quaternion.LookRotation(transform.forward, transform.up);

/*        //detatch rigidbody so it can move freely 
        Rigid.transform.parent = null;*/
    }

    void Update()
    {

        delta = Time.deltaTime;

        float Spd = Speed;


        /*        transform.position = Rigid.position;*/

        CheckGrounded();

        GroundAngleCheck();

        MovePlayer(delta, Spd, Acceleration);

        /*        RotateSelf(groundAngle, delta);*/

        /*        RotatePlayer(playerRotation, groundRotation);*/


    }

    //input system receiver
    public void ReceiveMoveInput(Vector2 input)
    {
        _moveInput = input;
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
        if (Physics.Raycast(transform.position, -transform.up, out HitDown, 5f, groundLayers))
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
        RaycastHit hit;

        if(Physics.Raycast(transform.position, groundDir, out hit, 1f, groundLayers))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    //move player character
    void MovePlayer(float delta, float speed, float acceleration)
    {
        
        float _yMov = _moveInput.y;
/*        float _xMov = _moveInput.x;*/
        float gravityAmt;
        float spd = speed;
        
        Vector3 currentVelocity = Rigid.velocity;

/*        Vector3 h = transform.right * _xMov;*/
        Vector3 v = transform.forward * _yMov;

        Vector3 moveDirection = (v.normalized);
        

/*        if (_xMov == 0 && _yMov == 0)
        {
            targetDir = transform.forward;
        }
        else
        {
            targetDir = moveDirection;
        }*/

        //find values for turning player
/*        Quaternion lookDir = Quaternion.LookRotation(targetDir);*/
/*        Vector3 lookDir = targetDir;
        float TurnSpd = turnSpeed;

        groundDir = Vector3.Lerp(groundDir, groundAngle, delta * turnSpeed);*/


        //set gravity if not grounded
        if (isGrounded)
        {
            gravityAmt = 0f;
        }
        else
        {
            gravityAmt = gravityForce;
        }


        //reduce speed if not inputting
/*        if (_moveInput.x == 0 && _moveInput.y == 0)
        {
            //we are not moving, lerp to a walk speed
            Spd = 0f;
        }
*/

        //if we are not pressing a move input we move towards velocity
       /* if (!MoveInput) 
        {
*//*            spd = speed * 0.8f; //less speed is applied to our character*//*
            moveDirection = Vector3.Lerp(transform.forward, Rigid.velocity.normalized, 12f * delta);
        }
        else
        {
            moveDirection = transform.forward;
        }*/

/*        Vector3 targetVelocity = moveDirection * spd;*/
        Vector3 targetVelocity;

        //push downwards in downward direction of mesh (apply gravity)
        targetVelocity = -groundAngle * gravityAmt;

        //apply accelerate factor
        Vector3 dir = Vector3.Lerp(currentVelocity, targetVelocity, delta * acceleration);
        
        Rigid.velocity = dir;

/*        RotateSelf(groundAngle, delta);*/
/*        RotateMesh(delta, lookDir, TurnSpd);*/
    }

    //rotate player to the floor angle
    void RotateSelf(Vector3 groundAngle, float delta)
    {
        Vector3 lerpDirection = Vector3.Lerp(transform.up, groundAngle, delta);
/*        transform.rotation = Quaternion.FromToRotation(transform.up, lerpDirection) * transform.rotation;*/
        groundRotation = Quaternion.FromToRotation(transform.up, lerpDirection) * transform.rotation;
        transform.rotation = groundRotation;
    }
    
    //rotate player to the direction we face forwards
    void RotateMesh(Vector3 lookDirection, float speed, float delta)
    {
        Quaternion slerpRotation = Quaternion.LookRotation(lookDirection, transform.up);
        /*        transform.rotation = Quaternion.Slerp(transform.rotation, slerpRotation, speed * d);*/
        playerRotation = Quaternion.Slerp(transform.rotation, slerpRotation, speed * delta);
        transform.rotation = playerRotation;
    }

    //move player character
    void RotatePlayer(Quaternion playerRotation, Quaternion groundRotation)
    {
        transform.rotation = groundRotation * playerRotation;
    }

}