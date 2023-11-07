using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class MovementController : MonoBehaviour
{

    private CharacterController _controller;
    
    //Movement inputs
    [SerializeField]
    Vector2 _moveInput;   //Vector of input
    public Vector3 _moveVector = new Vector3(0, 0, 0);  //Vector of planned movement
    [SerializeField] float _moveSpeed = 9f;
    [SerializeField] float _gravityForce = -7.8f;

    //Input Smoothing
    Vector3 _currentInputVector;
    Vector3 _smoothInputVelocity;
    [SerializeField]
    float smoothInputSpeed = 0.2f;

    //Player rotation
    private Vector3 _lookVector;
    public Vector3 _lookTarget;
    [SerializeField]
    private float _lookSpeed = 5f;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {

        _currentInputVector = Vector3.SmoothDamp(_currentInputVector, _moveInput, ref _smoothInputVelocity, smoothInputSpeed);
        
        _moveVector = new Vector3(_currentInputVector.x, 0, _currentInputVector.y);

        if (_controller.isGrounded)
        {
            _moveVector.y = 0.0f;
        }

        else
        {
            _moveVector.y = _gravityForce;
        }

        //Move
        _controller.Move(Time.deltaTime * _moveSpeed * _moveVector);



        //Rotate

        _lookTarget = transform.position + _lookVector;
        Quaternion rotation = Quaternion.LookRotation(_lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _lookSpeed);
        _lookVector = new Vector3(_currentInputVector.x, 0, _currentInputVector.y);  //Resetting to 0, 0, 0 when no input pressed - FIX


    }

    //Input receivers
    public void ReceiveMoveInput(Vector2 input)
    {
        _moveInput = input;
    }


}