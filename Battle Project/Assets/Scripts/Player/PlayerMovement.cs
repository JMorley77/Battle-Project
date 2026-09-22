using System.Text.RegularExpressions;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]


public class PlayerMovement : NetworkBehaviour
{
    [Header("Input")]
    [SerializeField] private InputController input;
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private PlayerPickUp pickUp;


    [Space(10)]
    [Header("Movement")]
    [SerializeField][Min(0)] public float moveSpeed = 4f;
    [SerializeField][Min(0)] public float origionalMoveSpeed = 4f;
    [SerializeField][Min(0)] public float sprintSpeed = 8f;
    [SerializeField][Min(0)] public float origionalSprintSpeed = 8f;
    [SerializeField][Min(0)] public float attackSprintSpeed = 6f;
    [SerializeField][Min(0)] private float rotateSpeed = 100f;

    [Header("Jump")]
    [SerializeField][Min(0)] private float jumpForce = 4f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck; 
    [SerializeField][Min(0)] private float groundCheckRadius = 0.2f; 
    [SerializeField] private LayerMask groundLayer;


    public bool isSprinting;
    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public Vector2 moveInput;

    public bool canMove = true;
    private Rigidbody rb;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        origionalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if(!IsOwner)
            return;

        if (combat.isDead)
        {
            return;
        }

        moveInput = input.moveAction.action.ReadValue<Vector2>();
        if (input.jumpAction.action.triggered)
        {
            jumpPressed = true;
        }
        isSprinting = input.sprintAction.action.IsPressed();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        Sprint();
        Move();
        Rotate();
        GroundCheck();
        Jump();

    }


    private void Move()
    {
        if (!canMove) 
            return;
        float forward = moveInput.y;
        Vector3 movement = transform.forward * forward;
        movement.Normalize();
        Vector3 targetVelocity = movement* moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; 
        rb.linearVelocity = targetVelocity;

    }
    private void Rotate()
    {
        if(!canMove) 
            return;
        float horizontal = moveInput.x;
        float rotationAmount = horizontal * rotateSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    private void Jump()
    {
        if(!canMove) 
            return;
        if (!jumpPressed) return; 

        jumpPressed = false; 

        if (!isGrounded) return; 

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Sprint()
    {
        if (!canMove)
            return;

        if (!isGrounded)
            return;

        if (isSprinting)
        {
            if (combat.isAttacking)
            {
                moveSpeed = attackSprintSpeed;
            }
            else
            {
                moveSpeed = sprintSpeed;
            }
        }
        else
        {
            moveSpeed = origionalMoveSpeed;
        }
    }

    private void GroundCheck() 
    { 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer); 
    }
}