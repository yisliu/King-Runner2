using UnityEngine;
using UnityEngine.InputSystem;

public class IslandPlayerMover : MonoBehaviour
{
    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float maxForwardSpeed = 18f;
    [SerializeField] private float speedIncreaseRate = 0.3f;

    [Header("Steering")]
    [SerializeField] private float rotationSpeed = 80f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravityStrength = 25f;

    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 3f;

    private Rigidbody rb;
    private Animator animator;
    private float steerInput;
    private float yVelocity;
    private bool isJumping;
    private float airTime;
    private float currentDistance;

    public float CurrentDistance => currentDistance;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        bool wasJumping = isJumping;
        forwardSpeed = Mathf.Min(forwardSpeed + speedIncreaseRate * Time.fixedDeltaTime, maxForwardSpeed);
        HandleMovement();
        currentDistance += forwardSpeed * Time.fixedDeltaTime;

        if (wasJumping && !isJumping)
            CameraEffect.Instance?.TriggerShake(0.15f, 0.15f);

        if (animator != null)
        {
            animator.SetFloat("Speed", forwardSpeed);
            animator.SetBool("IsGrounded", !isJumping);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumping)
        {
            isJumping = true;
            airTime = 0f;
            yVelocity = jumpForce;
        }
    }

    void HandleMovement()
    {
        if (Mathf.Abs(steerInput) > 0.01f)
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, steerInput * rotationSpeed * Time.fixedDeltaTime, 0f));

        if (isJumping)
        {
            airTime += Time.fixedDeltaTime;
            yVelocity -= gravityStrength * Time.fixedDeltaTime;
        }

        Vector3 currentPos = rb.position;
        Vector3 newPos = currentPos + transform.forward * forwardSpeed * Time.fixedDeltaTime;
        newPos.y = currentPos.y + yVelocity * Time.fixedDeltaTime;

        // Raycast to find actual terrain height.
        // If groundLayer is unassigned (mask = 0), fall back to everything except the player's own layer.
        int groundMask = groundLayer != 0 ? (int)groundLayer : ~(1 << gameObject.layer);
        Vector3 rayOrigin = new Vector3(newPos.x, currentPos.y + 2f, newPos.z);
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance + 2f, groundMask))
        {
            float groundY = hit.point.y;
            bool landing = !isJumping || (newPos.y <= groundY && yVelocity <= 0f && airTime >= 0.15f);
            if (landing)
            {
                newPos.y = groundY;
                yVelocity = 0f;
                isJumping = false;
            }
        }

        rb.MovePosition(newPos);
    }

    public float ForwardSpeed => forwardSpeed;

    public void AlterSpeed(float amount)
    {
        forwardSpeed = Mathf.Clamp(forwardSpeed + amount, 2f, maxForwardSpeed);
    }
}
