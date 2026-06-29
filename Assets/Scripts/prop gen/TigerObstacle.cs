using UnityEngine;

public class TigerObstacle : MonoBehaviour
{
    public enum MovementType { Lunge, Circle, OutAndBack }

    [Header("General")]
    [SerializeField] private bool randomizeType = true;
    [SerializeField] private MovementType movementType = MovementType.Lunge;
    [SerializeField] private float triggerDistance = 14f;

    [Header("Lunge")]
    [SerializeField] private float lungeSpeed = 10f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float sideOffset = 5f;

    [Header("Circle")]
    [SerializeField] private float circleRadius = 3f;
    [SerializeField] private float circleSpeed = 90f;

    [Header("Out and Back")]
    [SerializeField] private float patrolDistance = 4f;
    [SerializeField] private float patrolSpeed = 6f;

    private bool active;
    private Transform player;

    // Lunge
    private Vector3 lungeStart, lungeEnd;
    private float lungeProgress, lungeDuration;

    // Circle
    private Vector3 circleCenter, circleRight, circleForward;
    private float circleAngle;

    // Out and Back
    private Vector3 patrolOrigin, patrolTarget;
    private bool patrollingOut = true;
    private float patrolProgress;

    void Start()
    {
        player = FindFirstObjectByType<PlayerPhysicalProgress>()?.transform;

        if (randomizeType)
            movementType = (MovementType)Random.Range(0, 3);

        SetupMovement();
    }

    void SetupMovement()
    {
        switch (movementType)
        {
            case MovementType.Lunge:
            {
                float side = Random.value > 0.5f ? 1f : -1f;
                Vector3 offset = transform.right * sideOffset * side;
                lungeStart = transform.position + offset;
                lungeEnd   = transform.position - offset;
                transform.position = lungeStart;
                transform.LookAt(new Vector3(lungeEnd.x, lungeStart.y, lungeEnd.z));
                lungeDuration = Vector3.Distance(lungeStart, lungeEnd) / lungeSpeed;
                break;
            }
            case MovementType.Circle:
            {
                circleCenter  = transform.position;
                circleRight   = transform.right;
                circleForward = transform.forward;
                circleAngle   = Random.Range(0f, 360f);
                ApplyCirclePosition();
                break;
            }
            case MovementType.OutAndBack:
            {
                patrolOrigin = transform.position;
                float side   = Random.value > 0.5f ? 1f : -1f;
                patrolTarget = patrolOrigin + transform.right * patrolDistance * side;
                break;
            }
        }
    }

    void Update()
    {
        if (!active)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) < triggerDistance)
                active = true;
            return;
        }

        switch (movementType)
        {
            case MovementType.Lunge:      UpdateLunge();      break;
            case MovementType.Circle:     UpdateCircle();     break;
            case MovementType.OutAndBack: UpdateOutAndBack(); break;
        }
    }

    void UpdateLunge()
    {
        lungeProgress = Mathf.MoveTowards(lungeProgress, 1f, Time.deltaTime / lungeDuration);

        Vector3 pos = Vector3.Lerp(lungeStart, lungeEnd, lungeProgress);
        pos.y += Mathf.Sin(lungeProgress * Mathf.PI) * jumpHeight;
        transform.position = pos;

        if (lungeProgress >= 1f)
            Destroy(gameObject);
    }

    void UpdateCircle()
    {
        circleAngle += circleSpeed * Time.deltaTime;
        ApplyCirclePosition();

        // Face the direction of travel (tangent to the circle)
        float rad = (circleAngle + 90f) * Mathf.Deg2Rad;
        Vector3 tangent = circleRight * Mathf.Cos(rad) + circleForward * Mathf.Sin(rad);
        tangent.y = 0f;
        if (tangent != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(tangent);
    }

    void ApplyCirclePosition()
    {
        float rad = circleAngle * Mathf.Deg2Rad;
        transform.position = circleCenter
            + circleRight   * Mathf.Cos(rad) * circleRadius
            + circleForward * Mathf.Sin(rad) * circleRadius;
    }

    void UpdateOutAndBack()
    {
        Vector3 from = patrollingOut ? patrolOrigin : patrolTarget;
        Vector3 to   = patrollingOut ? patrolTarget : patrolOrigin;

        float segLength = Vector3.Distance(from, to);
        patrolProgress = Mathf.MoveTowards(patrolProgress, 1f, patrolSpeed / segLength * Time.deltaTime);
        transform.position = Vector3.Lerp(from, to, patrolProgress);

        Vector3 dir = (to - from); dir.y = 0f;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        if (patrolProgress >= 1f)
        {
            patrolProgress = 0f;
            patrollingOut = !patrollingOut;
        }
    }
}