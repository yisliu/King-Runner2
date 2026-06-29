using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private LevelCooker levelCooker;
    private Rigidbody rb;

    void Start()
    {
        levelCooker = FindFirstObjectByType<LevelCooker>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (levelCooker == null) return;

        if (rb != null)
        {
            // Drive Z via velocity so physics (gravity, tumbling) still applies on Y/X
            Vector3 vel = rb.linearVelocity;
            vel.z = -levelCooker.MoveSpeed;
            rb.linearVelocity = vel;
        }
        else
        {
            transform.Translate(Vector3.back * (levelCooker.MoveSpeed * Time.deltaTime), Space.World);
        }
    }
}
