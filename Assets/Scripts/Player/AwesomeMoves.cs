using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 movement;
    Rigidbody rigidBody;
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] float xClamp = 3f;
    [SerializeField] float zClamp = 3f;

    
    //	•	Awake() is called before Start(), which means it ensures rigidbody is assigned before any other script or function might use it.
    // •	Use Awake() when you need to initialize references to components early.
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();

    }

    void HandleMovement()
    {
        Vector3 currentPosition = rigidBody.position;
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);
        Vector3 newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);
        
        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);
        
        rigidBody.MovePosition(newPosition);
    }


}
