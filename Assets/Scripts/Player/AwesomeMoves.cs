/*
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript2 : MonoBehaviour
{
    Vector2 movement;
    Rigidbody rigidBody;
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] float xClamp = 3f;
    [SerializeField] float zClamp = 3f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravityStrength = 25f;

    private float yVelocity = 0f;
    private bool isJumping = false;
    private float floorY;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        floorY = transform.position.y;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumping)
        {
            isJumping = true;
            yVelocity = jumpForce;
        }
    }

    void HandleMovement()
    {
        if (isJumping)
            yVelocity -= gravityStrength * Time.fixedDeltaTime;

        Vector3 currentPosition = rigidBody.position;
        Vector3 newPosition = currentPosition + new Vector3(movement.x, 0f, movement.y) * (moveSpeed * Time.fixedDeltaTime);
        newPosition.y = currentPosition.y + yVelocity * Time.fixedDeltaTime;

        if (newPosition.y <= floorY)
        {
            newPosition.y = floorY;
            yVelocity = 0f;
            isJumping = false;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);

        rigidBody.MovePosition(newPosition);
    }
}
*/
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysicalProgress : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 8f;
    [SerializeField] private float steerSpeed = 80f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckOffset = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.3f;

    private float currentDistance = 0f;
    private float jumpBufferTimer = 0f;
    private const float jumpBufferTime = 0.15f;
    private Vector2 inputMovement;
    private Rigidbody rigidBody;
    private bool wasGrounded = true;
    private Animator animator;

    public float CurrentDistance => currentDistance;
    public bool Grounded => IsGrounded();

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX
                               | RigidbodyConstraints.FreezeRotationY
                               | RigidbodyConstraints.FreezeRotationZ;
        animator = GetComponentInChildren<Animator>();
    }

    bool IsGrounded()
    {
        int mask = groundLayer != 0 ? (int)groundLayer : Physics.DefaultRaycastLayers;
        return Physics.CheckSphere(transform.position + Vector3.up * groundCheckOffset, groundCheckRadius, mask);
    }

    void FixedUpdate()
    {
        bool grounded = IsGrounded();
        if (!wasGrounded && grounded)
            CameraEffect.Instance?.TriggerShake(0.15f, 0.15f);
        wasGrounded = grounded;

        if (animator != null)
        {
            animator.SetFloat("Speed", baseMoveSpeed);
            animator.SetBool("IsGrounded", grounded);
        }

        // Steer left/right
        if (Mathf.Abs(inputMovement.x) > 0.01f)
            rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(0f, inputMovement.x * steerSpeed * Time.fixedDeltaTime, 0f));

        // Jump buffer — keeps the request alive for jumpBufferTime seconds so
        // a press just before landing still fires on the next grounded frame
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.fixedDeltaTime;
            if (IsGrounded())
            {
                rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x, 0f, rigidBody.linearVelocity.z);
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                jumpBufferTimer = 0f;
            }
        }

        // Control horizontal movement; preserve Y so gravity and jump work uninterrupted
        rigidBody.linearVelocity = new Vector3(0f, rigidBody.linearVelocity.y, 0f)
                                 + transform.forward * baseMoveSpeed;

        currentDistance += baseMoveSpeed * Time.fixedDeltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed) jumpBufferTimer = jumpBufferTime;
    }

    public void AlterSpeed(float amount)
    {
        baseMoveSpeed = Mathf.Max(2f, baseMoveSpeed + amount);
    }
}