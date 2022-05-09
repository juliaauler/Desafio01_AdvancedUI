using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
        
    private float playerHeight = 2f;

    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float airMultiplier = 0.4f;
    private float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode useKey = KeyCode.E;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;

    private float _horizontalMovement;
    private float _verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.2f;
    private bool IsGrounded { get; set; }

    private Vector3 _moveDirection;
    private Vector3 _slopeMoveDirection;

    private Rigidbody _rb;

    private RaycastHit _slopeHit;

    public Animator anim;

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight / 2 + 0.5f)) {
            if (_slopeHit.normal != Vector3.up) {
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update() {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();
        Animation();

        if (Input.GetKeyDown(jumpKey) && IsGrounded) {
            Jump();
        }

        _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal);
    }

    private void MyInput() {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _moveDirection = orientation.forward * _verticalMovement + orientation.right * _horizontalMovement;
    }

    private void Jump() {
        if (IsGrounded) {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ControlSpeed() {
        if (Input.GetKey(sprintKey) && IsGrounded) {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        } else {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    private void ControlDrag() {
        if (IsGrounded) {
            _rb.drag = groundDrag;
        } else {
            _rb.drag = airDrag;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MovePlayer() {
        if (IsGrounded && !OnSlope()) {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        } else if (IsGrounded && OnSlope()) {
            _rb.AddForce(_slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        } else if (!IsGrounded) {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    
    public void Animation(){
        if(Input.GetKeyDown(KeyCode.S)){
            anim.SetBool("RunBackward",true);
        }

        else if(Input.GetKeyUp(KeyCode.S)){
            anim.SetBool("RunBackward",false);
        }

        if(Input.GetKeyDown(KeyCode.D)){
            anim.SetBool("RunLeft",true);
        }

        else if(Input.GetKeyUp(KeyCode.D)){
            anim.SetBool("RunLeft",false);
        }

        if(Input.GetKeyDown(KeyCode.A)){
            anim.SetBool("RunRight",true);
        }

        else if(Input.GetKeyUp(KeyCode.A)){
            anim.SetBool("RunRight",false);
        }

        if(Input.GetKeyDown(KeyCode.W)){
            anim.SetBool("RunForward",true);
        }

        else if(Input.GetKeyUp(KeyCode.W)){
            anim.SetBool("RunForward",false);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            anim.SetBool("RunForward",true);
        }

        else if(Input.GetKeyUp(KeyCode.Space)){
            anim.SetBool("RunForward",false);
        }

    }
}